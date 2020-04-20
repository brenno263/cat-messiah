using System.Runtime.Remoting.Messaging;
using _General._Scripts.Building;
using UnityEditor;
using UnityEngine;

namespace _General._Scripts.Editor
{
	[CustomEditor(typeof(Room))]
	public class RoomSetter : UnityEditor.Editor
	{
		private enum RoomType
		{
			Blue = 0,
			Brick = 1,
			White = 2
		}

		private RoomType _roomType;
		
		public override void OnInspectorGUI()
		{
			Room room = (Room) target;

			base.OnInspectorGUI();
			
			EditorGUILayout.LabelField("Edit Room Type", EditorStyles.boldLabel);
			_roomType = (RoomType) EditorGUILayout.EnumPopup("Room Type", _roomType);
			
			if (GUILayout.Button("Apply"))
			{
				room.roomType = (int) _roomType;
				room.UpdateBackground();
			}
		}
	}
}