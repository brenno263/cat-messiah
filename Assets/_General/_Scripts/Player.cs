using UnityEngine;

namespace _General._Scripts
{
	using static PlayerState;

	public class Player : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public float speed;

		public Rigidbody2D rigid;

		[Header("Set Dynamically")]
		public bool facingRight = true;

		public bool climbing = false;

		public PlayerState playerState;

		//[Header("Fetched on Init")]

		#endregion

		#region monobehavior methods

		void Start()
		{
		}

		void FixedUpdate() 
		{
			Move();
		}

		private void Move()
		{
			if (climbing) return;

			Vector2 vel = rigid.velocity;

			float horizontalInput = Input.GetAxis("Horizontal");

			if (horizontalInput > 0.1)
			{
				playerState = WalkingRight;
				vel.x = speed;
			}
			else if (horizontalInput < -0.1)
			{
				playerState = WalkingLeft;
				vel.x = -speed;
			}
			else
			{
				playerState = Idling;
				vel.x = 0;
			}

			rigid.velocity = vel;
		}

		#endregion

		#region private methods

		#endregion
	}
}