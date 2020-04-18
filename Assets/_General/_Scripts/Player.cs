using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _General._Scripts
{
	using static PlayerState;
	using static InteractionType;

	public class Player : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public float speed;

		public float climbHeight;
		public float climbSpeed;

		public float interactionCooldownMax;

		public Rigidbody2D rigid;

		public Animator anim;

		[Header("Set Dynamically")]
		public bool facingRight = true;

		public bool climbing = false;

		public PlayerState playerState = Idling;


		public Item currentItem = null;
		public bool carryingItem = false;

		public float interactionCooldown = 0;

		public List<Interactable> interactables;

		//[Header("Fetched on Init")]

		public bool CanInteract
		{
			get => interactionCooldown < 0;
		}

		#endregion

		#region monobehavior methods

		void Start()
		{
			anim = GetComponent<Animator>();
		}

		private void Update()
		{
			if (CanInteract)
			{
				if (ManageInteractions()) interactionCooldown = interactionCooldownMax;
			}
			else { interactionCooldown -= Time.deltaTime; }
		}

		void FixedUpdate()
		{
			Move();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			Interactable interactable = other.GetComponent<Interactable>();
			if (interactable != null) { interactables.Add(interactable); }
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			Interactable interactable = other.GetComponent<Interactable>();
			if (interactable != null && interactables.Contains(interactable)) { interactables.Remove(interactable); }
		}

		#endregion

		#region private methods

		private bool ManageInteractions()
		{
			if (Input.GetAxis("Vertical") < -0.1 && carryingItem)
			{
				DropItem();
				return true;
			}

			if (Input.GetAxis("Vertical") < -0.1 && !carryingItem)
			{
				foreach (Interactable interactable in interactables)
				{
					if (interactable.type == InteractionType.Item)
					{
						Item item = interactable.gameObject.GetComponent<Item>();
						PickUp(item);
						interactable.onInteract?.Invoke(this);
						return true;
					}
				}
			}

			if (Input.GetAxis("Fire1") > 0)
			{
				foreach (Interactable interactable in interactables)
				{
					switch (interactable.type)
					{
						case StairsUp:
							interactable.onInteract?.Invoke(this);
							StartCoroutine(Climb(true));
							return true;
						case StairsDown:
							interactable.onInteract?.Invoke(this);
							StartCoroutine(Climb(false));
							return true;
						case Door:
							interactable.onInteract?.Invoke(this);
							//do door thing
							return true;
						case InteractionType.Item:
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}

			return false;
		}

		private void PickUp(Item item)
		{
			item.PickUp(this);
			currentItem = item;
			carryingItem = true;
		}

		private void DropItem()
		{
			currentItem.Drop();
			currentItem = null;
			carryingItem = false;
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
				anim.SetBool("run", true);
				transform.localScale = new Vector2(3, this.transform.localScale.y);

			}
			else if (horizontalInput < -0.1)
			{
				playerState = WalkingLeft;
				vel.x = -speed;
				anim.SetBool("run", true);
				transform.localScale = new Vector2(-3, this.transform.localScale.y);
			}
			else
			{
				playerState = Idling;
				vel.x = 0;
				anim.SetBool("run", false);
			}

			rigid.velocity = vel;
		}

		private IEnumerator Climb(bool up)
		{
			if (climbing) yield break;
			climbing = true;
			playerState = up ? ClimbingUp : ClimbingDown;
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
			//playerstate is reset in move() now that climbing is false
			rigid.simulated = true;
		}

		#endregion
	}
}