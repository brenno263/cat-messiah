using __Scripts;
using UnityEngine;

namespace Testing.Scripts
{
    public class Enemy_Anim_1 : MonoBehaviour
    {
        public Animator animator;

        // Update is called once per frame

        private void Start()
        {
            Enemy enemy = GetComponent<Enemy>();
            enemy.onDeath = () => { animator.SetTrigger("Death"); };
        }
    }
}
