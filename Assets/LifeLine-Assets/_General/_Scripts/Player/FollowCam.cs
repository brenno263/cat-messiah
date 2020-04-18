using _General._Scripts.Level;
using UnityEngine;

namespace _General._Scripts.Player
{
    public class FollowCam : MonoBehaviour
    {
        #region variables

        [Header("Set in Inspector")]
        public new Camera camera; //new keyword is cause there's some obsolete engine code that needs to be suppressed

        public Transform playerTrans;
        public float acceleration;
        public float maxSpeed;
        public float zoomAcceleration;

        [Header("Set Dynamically")] 
        private CameraZone _cameraZone;
        private bool _cameraZoneIsSet;

        private float _cameraZoomVelocity;
        private Vector3 _cameraVelocity;

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

            if (_cameraZoneIsSet)
            {
                targetPos.x = Mathf.Clamp(targetPos.x, _cameraZone.xBounds.x, _cameraZone.xBounds.y);
                targetPos.y = Mathf.Clamp(targetPos.y, _cameraZone.yBounds.x, _cameraZone.yBounds.y);
            }

            targetPos.z = pos.z;

            pos = Vector3.SmoothDamp(pos, targetPos, ref _cameraVelocity, 1 / acceleration, maxSpeed);

            transform.position = pos;

            if (_cameraZoneIsSet)
            {
                camera.orthographicSize =
                    Mathf.SmoothDamp(camera.orthographicSize, _cameraZone.cameraSize, ref _cameraZoomVelocity,
                        1 / zoomAcceleration);
            }
        }


        private void OnDrawGizmos()
        {
            if (_cameraZone == null) return;
            Utils.Utils.GizmosDrawRect2D(
                _cameraZone.yBounds.y, 
                _cameraZone.xBounds.y, 
                _cameraZone.yBounds.x, 
                _cameraZone.xBounds.x);
        }

        #endregion
        
        #region public methods

        public void SetCameraZone(CameraZone cZone)
        {
            _cameraZone = cZone;
            _cameraZoneIsSet = true;

            if (camera)
            {
                cZone.aspect = camera.aspect;
            }
        }
        
        #endregion
    }
}