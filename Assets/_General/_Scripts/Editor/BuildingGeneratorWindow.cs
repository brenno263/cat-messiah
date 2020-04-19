using System;
using _General._Scripts.Building;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace _General._Scripts.Editor
{
	public class BuildingGeneratorWindow : EditorWindow
	{
		private Vector2 roomSize = Vector2.one;

		private float doorDrop = 0;

		private int width = 1;
		private int height = 1;

		private GameObject buildingGO;

		[MenuItem("Window/Building Generator")]
		public static void OpenWindow()
		{
			BuildingGeneratorWindow window = GetWindow<BuildingGeneratorWindow>("Building Generator");
			window.minSize = new Vector2(250, 250);
			window.maxSize = new Vector2(250, 250);
			window.Show();
		}

		private void OnGUI()
		{
			roomSize.x = EditorGUILayout.FloatField("Room Width", roomSize.x);

			roomSize.y = EditorGUILayout.FloatField("Room Height", roomSize.y);

			doorDrop = EditorGUILayout.FloatField("Door Drop", doorDrop);

			width = EditorGUILayout.IntField("Building Width", width);
			height = EditorGUILayout.IntField("Building Height", height);

			buildingGO = (GameObject) EditorGUI.ObjectField(
				new Rect(3, 140, position.width - 6, 20),
				"Building Gameobject",
				buildingGO,
				typeof(GameObject)
			);

			if (GUILayout.Button("Go!"))
			{
				if (buildingGO != null)
				{
					BuildingGenerator generator = buildingGO.GetComponent<BuildingGenerator>();
					if (generator != null) { generator.Run(width, height, roomSize, doorDrop, buildingGO); }
				}
			}

			if (GUILayout.Button("Reset"))
			{
				if (buildingGO != null)
				{
					BuildingGenerator generator = buildingGO.GetComponent<BuildingGenerator>();
					if (generator != null) { generator.Reset(); }
				}
			}
		}
	}
}