using UnityEngine;

namespace __Scripts.Player
{
    public class FollowCam : MonoBehaviour
    {
        #region variables

        [Header("Set in Inspector")] 
        public new Camera camera; //new keyword is cause there's some obsolete engine code that needs to be suppressed
        public Transform playerTrans;
        public float acceleration;
        public float zoomAcceleration;

        [Header("Set Dynamically")] public Vector2 xBounds;
        public Vector2 yBounds;
        public float targetCameraSize;

        private float _cameraZoomVelocity;

        #endregion

        #region monobehavior methods

        private void Start()
        {
            if (playerTrans == null) playerTrans = Player.singleton.transform;
            camera = GetComponent<Camera>();
        }

        private void FixedUpdate()
        {
            var fdt = Time.fixedDeltaTime;

            Vector3 pos = transform.position;

            Vector3 targetPos = playerTrans.position;
            
            targetPos.x = Mathf.Clamp(targetPos.x, xBounds.x, xBounds.y);
            targetPos.y = Mathf.Clamp(targetPos.y, yBounds.x, yBounds.y);
            targetPos.z = pos.z;

            pos = Vector3.Lerp(pos, targetPos, acceleration * Time.fixedDeltaTime);

            transform.position = pos;

            var cameraSize = camera.orthographicSize;
            camera.orthographicSize =
                Mathf.SmoothDamp(cameraSize, targetCameraSize, ref _cameraZoomVelocity,
                    1 / zoomAcceleration); //Mathf.Lerp(cameraSize, targetCameraSize, zoomAcceleration * fdt);
        }
        

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(new Vector3(xBounds.x, -100, 0), new Vector3(xBounds.x, 100, 0));
            Gizmos.DrawLine(new Vector3(xBounds.y, -100, 0), new Vector3(xBounds.y, 100, 0));


            Gizmos.DrawLine(new Vector3(-100, yBounds.x, 0), new Vector3(100, yBounds.x, 0));
            Gizmos.DrawLine(new Vector3(-100, yBounds.y, 0), new Vector3(100, yBounds.y, 0));
        }

        #endregion

        #region public methods

        public void SetCameraSize(float size)
        {
            targetCameraSize = size;
        }

        #endregion

        #region private methods

        #endregion
    }
}