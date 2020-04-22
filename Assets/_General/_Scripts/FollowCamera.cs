using _General._Scripts.Player;
using UnityEngine;

namespace _General._Scripts
{
	public class FollowCamera : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public Player.Player player;

		public float zCoordinate;

		public float maxSpeed;

		public float smoothTime;

		public float lookAhead;
		public float normalSize;

		public bool lookAtFocus;
		public Vector2 focus;
		public float focusSize;

		public float minHeight;

		public Camera thisCamera;

		//[Header("Set Dynamically")]
		//[Header("Fetched on Init")]

		private Vector2 _currentVelocity = Vector3.zero;
		private float _sizeVelocity = 0;

		#endregion

		#region monobehavior methods

		private void Start()
		{
			Vector3 pos = player.transform.position;
			pos.y = Mathf.Max(pos.y, minHeight);
			transform.position = pos;
		}

		private void Update()
		{
			Vector2 targetPos;
			float targetSize;
			if (lookAtFocus)
			{
				targetPos = focus;
				targetSize = focusSize;
			}
			else
			{
				targetPos = player.transform.position;
				targetPos.x += lookAhead * player.PlayerState.MoveDirection();
				targetSize = normalSize;
			}

			targetPos.y = Mathf.Max(targetPos.y, minHeight);

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

			thisCamera.orthographicSize = Mathf.SmoothDamp(
				thisCamera.orthographicSize,
				targetSize,
				ref _sizeVelocity,
				smoothTime,
				maxSpeed,
				Time.deltaTime
			);
		}

		#endregion

		#region private methods

		#endregion
	}
}