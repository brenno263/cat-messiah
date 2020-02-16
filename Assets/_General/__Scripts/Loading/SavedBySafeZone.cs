using System;
using UnityEngine;

namespace __Scripts.Loading
{
	public class SavedBySafeZone : MonoBehaviour
	{
		[TextArea] 
		public string msg =
			"This will only work if there is a root GameObject in the scene titled 'Enemy Anchor'";
		
		#region monobehavior methods
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.CompareTag("LoadingSafeZone"))
			{
				SceneSwapper.singleton.AddSavedGO(gameObject);
			}
		}

		private void OnDestroy()
		{
			SceneSwapper.singleton.RemoveSavedGO(gameObject);
		}

		#endregion
	}
}
