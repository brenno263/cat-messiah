using UnityEngine;

namespace _General._Scripts
{
	public class FollowCamera : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public Transform focus;

		public Player player;

		public float zCoordinate;

		public float maxSpeed;

		public float smoothTime;
		//[Header("Set Dynamically")]
		//[Header("Fetched on Init")]

		private Vector2 _currentVelocity = Vector3.zero;

		#endregion

		#region monobehavior methods

		void Start()
		{
			transform.position = focus.position;
		}

		void Update()
		{
			Vector3 pos = Vector2.SmoothDamp(
				transform.position,
				focus.position,
				ref _currentVelocity,
				smoothTime,
				maxSpeed,
				Time.deltaTime);

			pos.z = zCoordinate;

			transform.position = pos;
		}

		#endregion

		#region private methods

		#endregion
	}
}