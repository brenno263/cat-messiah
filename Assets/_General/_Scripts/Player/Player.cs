using System;
using System.Collections;
using System.Collections.Generic;
using _General._Scripts.Building;
using _General._Scripts.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _General._Scripts.Player
{
	using static PlayerState;
	using static InteractionType;
	using static SoundFx;

	public class Player : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public float speed;

		public float climbHeight;
		public float climbSpeed;

		public float maxFallSpeed;
		
		public float interactionCooldownMax;

		public Rigidbody2D rigid;

		public Animator anim;

		public RoomTracker roomTracker;

		public PlayerAudioManager audioMananager;

		[Header("Set Dynamically")]
		public bool facingRight = true;

		public bool climbing = false;

		public Item currentItem = null;
		public bool carryingItem = false;

		public float interactionCooldown = 0;

		public List<Interactable> interactables;

		private static readonly int AnimatorPlayerState = Animator.StringToHash("PlayerState");

		//[Header("Fetched on Init")]

		public PlayerState PlayerState
		{
			get => _playerState;
			private set
			{
				_playerState = value;
				anim.SetInteger(AnimatorPlayerState, (int) value);
				audioMananager.WalkLoopPlaying = (int) value > 1;
			}
		}

		private PlayerState _playerState;


		public bool CanInteract
		{
			get => interactionCooldown < 0;
		}

		#endregion

		#region monobehavior methods

		private void Awake()
		{
			//QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = 50;
		}

		private void Update()
		{
			if (CanInteract)
			{
				if (ManageInteractions()) interactionCooldown = interactionCooldownMax;
			}
			else { interactionCooldown -= Time.deltaTime; }
		}

		private void FixedUpdate()
		{
			Move();
			if(TestFalling()) FallDeath();
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
			if (Input.GetAxis("Vertical") < -0.05 && carryingItem)
			{
				DropItem();
				return true;
			}

			if (Input.GetAxis("Vertical") < -0.05 && !carryingItem)
			{
				foreach (Interactable interactable in interactables)
				{
					if (interactable.type != InteractionType.Item) continue;
					// ReSharper disable once Unity.PerformanceCriticalCodeInvocation
					Item item = interactable.gameObject.GetComponent<Item>();
					PickUp(item);
					interactable.onInteract?.Invoke(this);
					return true;
				}
			}

			if (Input.GetAxis("Vertical") > 0.05 && carryingItem)
			{
				foreach (Interactable interactable in interactables)
				{
					if (interactable.type != InteractionType.Item) continue;
					Item item = interactable.gameObject.GetComponent<Item>();
					DropItem();
					PickUp(item);
					interactable.onInteract?.Invoke(this);
					return true;
				}
			}

			if (Input.GetAxis("Fire1") > 0)
			{
				//Extinguisher takes priority
				if (carryingItem && currentItem.type == ItemType.Extinguisher && roomTracker.inRoom &&
				    roomTracker.currentRoom.FireLevel > 0)
				{
					audioMananager.Play(Extinguish);
					roomTracker.currentRoom.Extinguish();
					currentItem.UseUp(this);
					return true;
				}
				
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
							// ReSharper disable once Unity.PerformanceCriticalCodeInvocation
							Door door = interactable.GetComponent<Door>();
							if (carryingItem && currentItem.type == ItemType.Axe && !door.IsOpen)
							{
								audioMananager.Play(SmashDoor);
								door.IsOpen = true;
								currentItem.UseUp(this);
							}
							else if (carryingItem && currentItem.type == ItemType.Board && door.IsOpen)
							{
								audioMananager.Play(BuildDoor);
								door.IsOpen = false;
								currentItem.UseUp(this);
							}

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
			if(item.type == ItemType.Cat) audioMananager.Play(Cat);
			item.PickUp(this);
			currentItem = item;
			carryingItem = true;
		}

		private void DropItem()
		{
			if (!carryingItem) return;
			if(currentItem.type == ItemType.Cat) audioMananager.Play(Cat);
			currentItem.Drop(this);
			currentItem = null;
			carryingItem = false;
		}

		private void Move()
		{
			if (climbing) return;

			Vector2 vel = rigid.velocity;

			float horizontalInput = Input.GetAxis("Horizontal");
			Transform trans = transform;
			Vector3 localScale = trans.localScale;

			if (horizontalInput > 0.1)
			{
				PlayerState = WalkingRight;
				vel.x = speed;
				//performs better with items, but makes the player judder
				//trans.rotation = Quaternion.AngleAxis(0, Vector3.up);
				trans.localScale = new Vector2(Mathf.Abs(localScale.x), localScale.y);
			}
			else if (horizontalInput < -0.1)
			{
				PlayerState = WalkingLeft;
				vel.x = -speed;
				//trans.rotation = Quaternion.AngleAxis(180, Vector3.up); 

				trans.localScale = new Vector2(-Mathf.Abs(localScale.x), localScale.y);
			}
			else
			{
				if (PlayerState.Direction() < 0) { PlayerState = FacingLeft; }
				else PlayerState = FacingRight;
				vel.x = 0;
			}

			rigid.velocity = vel;
		}

		public void Burn()
		{
			//burn to death
			print("Player burned to death, oh no!");
			SceneManager.LoadScene("DeathScreen");
		}

		private IEnumerator Climb(bool up)
		{
			if (climbing) yield break;
			interactables.Clear();
			climbing = true;
			PlayerState = up ? ClimbingUp : ClimbingDown;
			rigid.simulated = false;
			float startY = transform.position.y;
			float y = startY;
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
		
		private bool TestFalling() => (Mathf.Abs(rigid.velocity.y) > maxFallSpeed);

		private void FallDeath()
		{
			DropItem();
			
			if (PlayerState.Direction() < 0) { PlayerState = FacingLeft; }
			else PlayerState = FacingRight;
			transform.rotation = Quaternion.AngleAxis(-90 * PlayerState.Direction(), Vector3.forward);
			this.enabled = false;
		}

		#endregion
	}
}