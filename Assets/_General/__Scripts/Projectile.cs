using UnityEngine;

namespace __Scripts
{
    public class Projectile : MonoBehaviour
    {
        #region variables
        [Header("Set in Inspector")]
        public float damage;
        public float speed;
        //[header("Set Dynamically")]
        //[header("Fetched on Init")]
        #endregion

        #region monobehavior methods
        void Start()
        {
            var rigid = GetComponent<Rigidbody2D>();
            rigid.velocity = transform.TransformDirection(new Vector2(1,0) * speed);
            awaketime = Time.time;
        }
        float awaketime;
        void Update()
        {
            if(Time.time - awaketime > 10)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch(collision.transform.tag)
            {
                case "Enemy":
                    var e = collision.transform.GetComponent<Enemy>();
                    if(e != null) OnEnemyHit(e);
                    Destroy(gameObject);
                    break;
                default:
                    Destroy(gameObject);
                    break;
            }
        }
        #endregion

        #region private methods
        protected void OnEnemyHit(Enemy e)
        {
            e.Damage(damage);
        }
        #endregion
    }
}
