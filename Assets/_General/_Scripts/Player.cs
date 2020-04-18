using System;
using System.Collections;
using UnityEngine;

namespace _General._Scripts
{
	using static PlayerState;

	public class Player : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public float speed;

		public float climbHeight;
		public float climbSpeed;

		public Rigidbody2D rigid;

		[Header("Set Dynamically")]
		public bool facingRight = true;

		public bool climbing = false;

		public PlayerState playerState = Idling;

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

		private void OnTriggerStay2D(Collider2D other)
		{
			print(Input.GetAxis("Fire1"));
			Interactable interactable;
			if (Input.GetAxis("Fire1") > 0)
			{
				interactable = other.gameObject.GetComponent<Interactable>();
				if (interactable != null)
				{
					Interact(interactable);
				}
			}
		}

		#endregion

		#region private methods

		private void Interact(Interactable interactable)
		{
			interactable.onInteract?.Invoke();
			
			switch (interactable.type)
			{
				case InteractionType.Item:
					break;
				case InteractionType.StairsUp:
					StartCoroutine(Climb(true));
					break;
				case InteractionType.StairsDown:
					StartCoroutine(Climb(false));
					break;
				case InteractionType.Environment:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
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

		private IEnumerator Climb(bool up)
		{
			climbing = true;
			rigid.simulated = false;
			float startY = transform.position.y;
			float y = 0;
			while (up && y < startY + climbHeight || !up && y > startY - climbHeight)
			{
				Transform trans = transform;
				Vector2 pos = trans.position;
				pos.y += climbSpeed * Time.deltaTime * (up ? 1 : -1);
				trans.position = pos;
				y = pos.y;
				yield return null;
			}

			climbing = false;
			rigid.simulated = true;
		}
		
		#endregion
	}
}