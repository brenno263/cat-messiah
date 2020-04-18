using UnityEngine;

namespace _General._Scripts.Loading
{
    public class LoadingZone : MonoBehaviour
    {
        #region variables

        [Header("Set in Inspector")]
        public string destinationScene;
        public Vector2 transitionOffset;
        public LoadingSafeZone safeZone;

        [Header("Set Dynamically")] 
        public SceneRepresentative sceneRepresentative;
        /// <summary>
        /// If active, collision with player will load the next area. Otherwise, it will set this scene as current.
        /// </summary>
        public bool primed = false;

        #endregion

        #region monobehavior methods

        private void Start()
        {
            SceneSwapper swapper = SceneSwapper.singleton;

            //if this scene is the current one, it's already been fetched properly
            if (swapper.GetCurrentSceneRep() != sceneRepresentative) 
            {
                //if not, make sure the swapper knows about this scene
                swapper.SetOldSceneRep(sceneRepresentative); 

            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
            {
                SceneSwapper swapper = SceneSwapper.singleton;
                
                swapper.SetCurrentSceneRep(sceneRepresentative); //set this loader's home scene as current

                if (!primed) return; //below are primed only items
                
                //activate the safe zone
                safeZone.gameObject.SetActive(true); 
                //tell the swapper to load the next scene
                swapper.SwapScene(destinationScene, transitionOffset); 
                primed = false; //de-prime
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
            	//same as above, but this redundancy fixes bugs
                SceneSwapper.singleton.SetCurrentSceneRep(sceneRepresentative);
            }
        }

        #endregion
    }
}
