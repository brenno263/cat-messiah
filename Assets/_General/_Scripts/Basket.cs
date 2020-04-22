using UnityEngine;

namespace _General._Scripts
{
	public class Basket : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public Vector2 catPosition;
		//[Header("Set Dynamically")]
		//[Header("Fetched on Init")]
		#endregion

		public void PlaceCat(Item item)
		{
			if (item.type != ItemType.Cat) return;
			GameObject catGO = item.gameObject;
			catGO.transform.parent = transform;
			catGO.transform.localPosition = catPosition;
			item.rigid.simulated = false;
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
}
