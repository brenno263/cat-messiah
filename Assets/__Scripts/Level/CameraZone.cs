using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace __Scripts.Level
{
	public class CameraZone : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")] 
		public Vector2 xBounds;
		public Vector2 yBounds;
		public float cameraSize;
		public float aspect;

		#endregion

		#region monobehaviors

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
			
			//top left
			corners.Add(new Vector2(adjustedXBounds.x, adjustedYBounds.y));
			//top right
			corners.Add(new Vector2(adjustedXBounds.y, adjustedYBounds.y));
			//bottom right
			corners.Add(new Vector2(adjustedXBounds.y, adjustedYBounds.x));
			//bottom left
			corners.Add(new Vector2(adjustedXBounds.x, adjustedYBounds.x));

			for (int i = 1; i <= 4; i++)
			{
				Gizmos.DrawLine(corners[i - 1], corners[i % 4]);
			}
		}

		#endregion
	}
}
