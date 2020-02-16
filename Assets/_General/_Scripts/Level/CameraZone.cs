using System.Collections.Generic;
using _General._Scripts.Loading;
using UnityEngine;

namespace _General._Scripts.Level
{
	public class CameraZone : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")] 
		public Vector2 xBounds;
		public Vector2 yBounds;
		public float cameraSize;
		public float aspect;
		public Color color;

		#endregion

		#region monobehaviors

		private void Start()
		{
			SceneSwapper.singleton.onShift.AddListener(UpdateBounds);
		}

		private void OnDrawGizmos()
		{
			var corners = new List<Vector2>();

			var cameraWidth = cameraSize * aspect;
			
			var adjustedXBounds = new Vector2(xBounds.x, xBounds.y);
			adjustedXBounds.x -= cameraWidth;
			adjustedXBounds.y += cameraWidth;
			var adjustedYBounds = new Vector2(yBounds.x, yBounds.y);
			adjustedYBounds.x -= cameraSize;
			adjustedYBounds.y += cameraSize;

			Color innerBoxColor = Color.Lerp(color, Color.white, 0.5f);
			
			Utils.Utils.GizmosDrawRect2D(
				adjustedYBounds.y, 
				adjustedXBounds.y, 
				adjustedYBounds.x, 
				adjustedXBounds.x,
				color);
			
			Utils.Utils.GizmosDrawRect2D(
				yBounds.y,
				xBounds.y,
				yBounds.x,
				xBounds.x,
				innerBoxColor
			);
		}

		#endregion
		
		#region private methods
		
		private void UpdateBounds(Vector2 offset)
		{
			xBounds.x -= offset.x;
			xBounds.y -= offset.x;
			yBounds.x -= offset.y;
			yBounds.y -= offset.y;
		}
		
		#endregion
	}
}
