using UnityEngine;

namespace Testing.Scripts
{
    public class Enemy_Walk_1 : MonoBehaviour
    {
        //set in inspector
        public float speed;
        public float bodyWidth;
  

        //references
        private Rigidbody2D rigid;
        private IsGrounded isGrounded;

        //set dynamically
        private bool facingRight;


        // Start is called before the first frame update
        void Start()
        {
            rigid = GetComponent<Rigidbody2D>();
            isGrounded = GetComponent<IsGrounded>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isGrounded) Walk();
        }

        private void Walk()
        {
            Vector2 direction = facingRight ? Vector2.right : Vector2.left;
            RaycastHit2D hitWall = Physics2D.Raycast((Vector2)transform.position + direction * bodyWidth / 2, direction, 0.1f);

            if (hitWall.collider != null || (facingRight ? !isGrounded.isGroundedRight : !isGrounded.isGroundedLeft)) {
            
                facingRight = !facingRight;
            }

            Vector2 vel = rigid.velocity;

            vel.x = speed * (facingRight ? 1 : -1);

            rigid.velocity = vel;

        }

        private void OnDrawGizmos()
        {
            Vector2 direction = facingRight ? Vector2.right : Vector2.left;
            Gizmos.DrawLine((Vector2)transform.position + direction * bodyWidth / 2, (Vector2)transform.position + direction * bodyWidth / 2 + (direction * 0.1f));
        }
    }
}
