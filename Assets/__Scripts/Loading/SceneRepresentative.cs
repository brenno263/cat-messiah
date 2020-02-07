using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace __Scripts.Loading
{
	public class SceneRepresentative : MonoBehaviour
	{
		#region variables
		[Header("Set in Inspector")]
		public string sceneName;

		[Header("Set Dynamically")]
		public HashSet<GameObject> safeEnemies = new HashSet<GameObject>();
		public Action onLoad;

		[Header("Fetched on Init")] 
		public Scene scene;
		public HashSet<GameObject> roots;
		private HashSet<GameObject> _playerRoots;
		public HashSet<LoadingZone> loadingZones;
		public GameObject enemyRoot;
		
		//private
		private Vector2 _offset;
		
		#endregion

		#region monobehavior methods
		private void Awake()
		{
			if(sceneName == null) Debug.Log("sceneName not set in rep. Very naughty. Fix pronto.");

			//fetch scene info
			scene = SceneManager.GetSceneByName(sceneName);
			roots = new HashSet<GameObject>(scene.GetRootGameObjects());
			_playerRoots = new HashSet<GameObject>(SceneManager.GetSceneByName("Player").GetRootGameObjects());
			
			//gather all of this scene's loading zones
			loadingZones = new HashSet<LoadingZone>();
			foreach(GameObject root in roots)
			{
				if (root.name == "Enemy Anchor") enemyRoot = root; //catch the enemy root while we're in here

				if (root.CompareTag("LoadingZone"))
				{
					var lZ = root.GetComponent<LoadingZone>();
					if (lZ != null)
					{
						loadingZones.Add(lZ);
						lZ.sceneRepresentative = this;
					}
				}
			};

			//If this is the first scene loaded, set this as the current rep and activate loading zones
			if (SceneSwapper.singleton.GetCurrentSceneRep() == null)
			{
				SceneSwapper.singleton.SetCurrentSceneRep(this);
				PlayerLeftSafeZone();
			}
			
			onLoad?.Invoke();
		}
		
		#endregion

		#region public methods

		/// <summary>
		/// Shift this room (and the player) by a specified distance
		/// </summary>
		/// <param name="offset">Distance to shift this room</param>
		public void Shift(Vector2 offset)
		{
			_offset = offset;

			//get everything that should be shifted
			var shiftedGOs = new List<GameObject>(roots);
			shiftedGOs.AddRange(_playerRoots);
			
			//move this scene aside
			foreach (GameObject go in shiftedGOs)
			{
				var tform = go.GetComponent<Transform>();
				Vector3 pos = tform.position;
				pos -= (Vector3)offset;
				tform.position = pos;
			}
		}

		/// <summary>
		/// Undo any shifting that's been done to this room
		/// </summary>
		public void Unshift()
		{
			if (_offset == Vector2.zero) return; //If we're not offset, we're done here.
			
			Shift(-_offset);
			_offset = Vector2.zero;
		}

		/// <summary>
		/// Activate all of this room's loading zones such that when collided with they will load the next room
		/// </summary>
		public void ActivateLoadingZones()
		{
			foreach (LoadingZone lZ in loadingZones)
			{
				lZ.primed = true;
			}
		}
		
		/// <summary>
		/// If the player left the safe zone, activate this room's loading zones and remove any offsets
		/// </summary>
		public void PlayerLeftSafeZone()
		{
			Unshift();
			ActivateLoadingZones();
		}

		#endregion
	}
}
