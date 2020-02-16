using UnityEngine;

namespace Testing.Scripts
{
    public class Enemy_Walk_2 : MonoBehaviour
    {

        public float jumpCooldown;
        public float jumpCooldownMax;
        public float jumpForce;
        public float verticalCorrection;
        private Rigidbody2D rigid;
        private IsGrounded isGrounded;


        // Start is called before the first frame update
        void Start()
        {
            rigid = GetComponent<Rigidbody2D>();
            isGrounded = GetComponent<IsGrounded>();
            jumpCooldown = jumpCooldownMax;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            jumpCooldown -= Time.fixedDeltaTime;

            if(jumpCooldown <= 0)
            {
                jumpCooldown =  Random.value * jumpCooldownMax + 1;

                if (isGrounded)
                {
                    rigid.AddForce((__Scripts.Player.Player.singleton.transform.position - this.transform.position + Vector3.up * verticalCorrection) * jumpForce);
                }
            }
        }
    }
}
