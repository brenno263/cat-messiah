using UnityEngine;

namespace _General._Scripts
{
	public class FollowCamera : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public Player player;

		public float zCoordinate;

		public float maxSpeed;

		public float smoothTime;

		public float lookAhead;
		//[Header("Set Dynamically")]
		//[Header("Fetched on Init")]

		private Vector2 _currentVelocity = Vector3.zero;

		#endregion

		#region monobehavior methods

		void Start()
		{
			transform.position = player.transform.position;
		}

		void Update()
		{
			Vector2 targetPos = player.transform.position;
			targetPos.x += lookAhead * player.playerState.direction();

			Vector3 pos = Vector2.SmoothDamp(
				transform.position,
				targetPos,
				ref _currentVelocity,
				smoothTime,
				maxSpeed,
				Time.deltaTime
				);

			pos.z = zCoordinate;

			transform.position = pos;
		}

		#endregion

		#region private methods

		#endregion
	}
}