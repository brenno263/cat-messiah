using UnityEngine;

namespace _General._Scripts
{
	public enum ItemType
	{
		Axe, Board, Cat, Extinguisher
	}
	
	public class Item : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public Collider2D trigger;

		public Collider2D collider;

		public Rigidbody2D rigid;

		public Interactable interactable;

		public float groundRotation;

		public Vector2 groundScale;

		public float carryingRotation;
		
		public Vector2 carryingPosition;

		public Vector2 carryingScale;

		[Header("Set Dynamically")]
		public bool carrying;
		
		//[Header("Fetched on Init")]
		#endregion

		public void PickUp(Player player)
		{
			
			Transform trans = transform;
			trans.SetParent(player.transform);
			trans.localPosition = carryingPosition;
			trans.rotation = Quaternion.AngleAxis(carryingRotation, Vector3.forward);
			trans.SetGlobalScale(carryingScale);
			collider.enabled = false;
			rigid.simulated = false;
			carrying = true;
		}

		public void Drop()
		{
			Transform trans = transform;
			trans.SetParent(null);
			trans.rotation = Quaternion.AngleAxis(groundRotation, Vector3.forward);
			trans.SetGlobalScale(groundScale);
			collider.enabled = true;
			rigid.simulated = true;
			carrying = false;
		}

		#region monobehavior methods
		void Start()
		{
        
		}

		void Update()
		{
        
		}
		#endregion

		#region private methods
		#endregion
	}

	public static class TransformExtensions
	{
		public static void SetGlobalScale(this Transform transform, Vector3 newScale)
		{
			transform.localScale = Vector3.one;
			Vector3 lossyScale = transform.lossyScale;
			transform.localScale = new Vector3(newScale.x/lossyScale.x, newScale.y/lossyScale.y, newScale.z/lossyScale.z);
		}
	}
}
