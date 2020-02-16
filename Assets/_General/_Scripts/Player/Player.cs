using System.Collections.Generic;
using System.Linq;
using _General._Scripts.Level;
using UnityEngine;

namespace _General._Scripts.Player
{
    public class Player : MonoBehaviour
    {

        [Header("Set in Inspector")]
        public float acceleration;
        public float topSpeed;
        public GameObject weapAnchor;


        [Header("Set Dynamically")]
        private HashSet<Weapon> _weapons;
        public Vector3 moveInput;
        public Vector3 fireInput;
        

        [Header("Fetched on Init")]
        public static Player singleton;
        private Rigidbody2D _rigid;
        private FollowCam _followCam;
        
        // Start is called before the first frame update
        private void Start()
        {
            if (singleton == null) singleton = this;

            _rigid = GetComponent<Rigidbody2D>();
            if (Camera.main != null) _followCam = Camera.main.GetComponent<FollowCam>();

            _weapons = new HashSet<Weapon>(GetComponentsInChildren<Weapon>());
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            moveInput.x = Input.GetAxis("MoveX");
            moveInput.y = Input.GetAxis("MoveY");
            fireInput.x = Input.GetAxis("FireX");
            fireInput.y = Input.GetAxis("FireY");
            Move();
            Aim();
            foreach (Weapon weap in _weapons.Where(weap => fireInput.magnitude > 0))
            {
                weap.fire();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("CameraZone"))
            {
                var cZone = other.GetComponent<CameraZone>();
                if (cZone != null)
                {
                    _followCam.SetCameraZone(cZone);
                }
            }
        }

        void Move()
        {
            //string[] joyNames = Input.GetJoystickNames();
            //foreach(string name in joyNames)
            //{
            //    print(name);
            //}
            Vector2 vel = _rigid.velocity;
            Vector2 input = new Vector2(Input.GetAxis("MoveX"), Input.GetAxis("MoveY"));
            vel = Vector2.Lerp(vel, input * topSpeed, acceleration);
            _rigid.velocity = vel;
        }

        void Aim()
        {
            Vector3 YDir = new Vector3(-Input.GetAxis("FireY"), Input.GetAxis("FireX"), 0);
            weapAnchor.transform.localRotation = Quaternion.LookRotation(Vector3.forward, YDir);
        }
    }
}
