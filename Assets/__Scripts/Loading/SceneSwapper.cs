using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace __Scripts.Loading
{
    public class SceneSwapper : MonoBehaviour
    {
        #region variables

        [Header("Set in Inspector")] 
        public string startingScene;
        
        [Header("Set Dynamically")] 
        private readonly HashSet<GameObject> _safeZoneGOs = new HashSet<GameObject>();
        private SceneRepresentative _currentSceneRep;
        private SceneRepresentative _oldSceneRep;

        [Header("Set on Init")]
        public static SceneSwapper singleton;
        #endregion

        #region monobehavior methods
        private void Start()
        {
            if(!singleton)
            { singleton = this; }
            LoadScene("Player");
            LoadScene(startingScene);
        }
        #endregion
        
        #region public methods

        public SceneRepresentative GetCurrentSceneRep()
        {
            return _currentSceneRep;
        }
        
        public void SetCurrentSceneRep(SceneRepresentative sceneRep)
        {
            if (_currentSceneRep != null && _currentSceneRep != sceneRep)
            {
                _oldSceneRep = _currentSceneRep;
            }

            _currentSceneRep = sceneRep;

            //this whole convoluted chunk vv just sets the current scene properly in the engine when it's been all loaded
            IEnumerator OnSceneLoad() 
            {
                while (!sceneRep.scene.isLoaded) yield return null;
                SceneManager.SetActiveScene(sceneRep.scene);
            }

            StartCoroutine(OnSceneLoad());
        }

        public void SetOldSceneRep(SceneRepresentative sceneRep)
        {
            _oldSceneRep = sceneRep;
        }

        public void SwapScene(string sceneName, Vector2 transitionOffset)
        {
            //move the current scene aside
            if(_currentSceneRep != null) _currentSceneRep.Shift(transitionOffset);

            //load in the selected scene
            LoadScene(sceneName);
        }
        
        //triggered when player leaves loading safe zone
        public void PlayerLeftSafeZone()
        {
            foreach(GameObject go in _safeZoneGOs.Where(go => go != null))
            {
                go.transform.SetParent(_currentSceneRep.enemyRoot.transform);
            }
            
            _safeZoneGOs.Clear();
            
            UnloadScene(_oldSceneRep.sceneName, _currentSceneRep.PlayerLeftSafeZone);
        }

        public void AddSavedGO(GameObject go)
        {
            if (_safeZoneGOs.Contains(go)) return;
            
            _safeZoneGOs.Add(go);
        }

        public void RemoveSavedGO(GameObject go)
        {
            _safeZoneGOs.Remove(go);
        }
        #endregion

        #region private methods
        private void LoadScene(string sceneName, Action postLoad = null)
        {
            StartCoroutine(SceneLoading(sceneName, true, postLoad));
        }

        private void UnloadScene(string sceneName, Action postLoad = null)
        {
            StartCoroutine(SceneLoading(sceneName, false, postLoad));
        }

        /// <summary>
        /// loads or unloads a scene, invoking post when done
        /// </summary>
        /// <param name="sceneName">name of the scene to load/unload</param>
        /// <param name="loading">true to load a scene, false to unload it</param>
        /// <param name="post">a function to be run once loading is finished</param>
        /// <returns></returns>
        private static IEnumerator SceneLoading(string sceneName, bool loading, Action post = null)
        {
            AsyncOperation loadOp = loading ? 
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive) 
                : SceneManager.UnloadSceneAsync(sceneName);
            
            if (post == null) yield break;

            while(!loadOp.isDone)
            {
                yield return null;
            }
            post.Invoke();
        }
        #endregion
        
        //should be removed probably... We'll see...
        //If it does end up being useful, I never want to have to rewrite this abomination.
        #region obsolete
        private bool IsSceneNameValid(params string[] sceneNames)
        {
            var scenesInBuild = new List<string>();

            for(var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                var lastSlashNdx = scenePath.LastIndexOf('/');
                var lastDotNdx = scenePath.LastIndexOf('.');
                scenesInBuild.Add(scenePath.Substring(lastSlashNdx + 1, lastDotNdx - lastSlashNdx - 1));
            }

            if (sceneNames.Any(sceneName => !scenesInBuild.Contains(sceneName)))
            {
                Debug.Log("Incorrect Scene Name passed to SceneSwapper");
                return false;
            }
            return true;
        }
        #endregion
    }
}
