using System.Collections.Generic;
using UnityEngine;

namespace __Scripts.Player
{
    public class Player : MonoBehaviour
    {

        [Header("Set in Inspector")]
        public float acceleration;
        public float topSpeed;
        public GameObject WeapAnchor;


        [Header("Set Dynamically")]
        private Rigidbody2D rigid;
        private HashSet<Weapon> weapons;
        public Vector3 moveInput;
        public Vector3 fireInput;
        public static Player singleton;

        // Start is called before the first frame update
        void Start()
        {
            if (singleton == null) singleton = this;

            rigid = GetComponent<Rigidbody2D>();
            weapons = new HashSet<Weapon>(GetComponentsInChildren<Weapon>());
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            moveInput.x = Input.GetAxis("MoveX");
            moveInput.y = Input.GetAxis("MoveY");
            fireInput.x = Input.GetAxis("FireX");
            fireInput.y = Input.GetAxis("FireY");
            Move();
            Aim();
            foreach(Weapon weap in weapons)
            {
                if(fireInput.magnitude > 0)
                {
                    weap.fire();
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
            Vector2 vel = rigid.velocity;
            Vector2 input = new Vector2(Input.GetAxis("MoveX"), Input.GetAxis("MoveY"));
            vel = Vector2.Lerp(vel, input * topSpeed, acceleration);
            rigid.velocity = vel;
        }

        void Aim()
        {
            Vector3 YDir = new Vector3(-Input.GetAxis("FireY"), Input.GetAxis("FireX"), 0);
            WeapAnchor.transform.localRotation = Quaternion.LookRotation(Vector3.forward, YDir);
        }
    }
}
