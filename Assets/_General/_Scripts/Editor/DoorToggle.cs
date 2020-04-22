using _General._Scripts.Building;
using UnityEditor;
using UnityEngine;

namespace _General._Scripts.Editor
{
	[CustomEditor(typeof(Door))]
	public class DoorToggle : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			Door door = (Door) target;
			
			base.OnInspectorGUI();

			if (GUILayout.Button("Toggle")) { door.IsOpen = !door.IsOpen; }
		}
	}
}
