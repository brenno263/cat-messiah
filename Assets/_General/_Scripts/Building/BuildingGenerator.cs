using System;
using System.Collections.Generic;
using UnityEngine;

namespace _General._Scripts.Building
{
	using static RoomDirection;
	
	[ExecuteInEditMode]
	public class BuildingGenerator : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public GameObject roomPrefab;

		public GameObject wallsBothPrefab;
		public GameObject wallsRightPrefab;
		public GameObject wallsLeftPrefab;

		public GameObject doorPrefab;

		//[Header("Set Dynamically")]
		//[Header("Fetched on Init")]
		#endregion

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
    
		public void Run(int width, int height, Vector2 roomSize, float doorDrop, GameObject buildingGO)
		{
			var roomGOs = new GameObject[width, height];
			var rooms = new Room[width, height];

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					GameObject roomGO = Instantiate(roomPrefab, buildingGO.transform);
					Transform roomTForm = roomGO.transform;
					Room room = roomGO.GetComponent<Room>();
					
					roomGOs[x, y] = roomGO;
					rooms[x, y] = room;
					
					roomTForm.position = new Vector3(x * roomSize.x, y * roomSize.y, 0);
					
					room.x = x;
					room.y = y;

					
					RoomDirection direction = Both;
					if (x == 0) direction = Right;
					if (x == width - 1) direction = Left;
					
					SetWalls(room, roomTForm, direction);
				}
			}
			
			for (int x = 0; x < width - 1; x++)
			{
				for (int y = 0; y < height; y++)
				{
					GameObject roomGO = roomGOs[x, y];
					Room room = rooms[x, y];
					Room nextRoom = rooms[x + 1, y];

					GameObject doorGO = Instantiate(doorPrefab, buildingGO.transform);
					Door door = doorGO.GetComponent<Door>();
					Vector3 doorPos = roomGO.transform.position;
					doorPos.x += roomSize.x;
					doorPos.y -= doorDrop;
					doorGO.transform.position = doorPos;

					room.rightDoor = door;
					nextRoom.leftDoor = door;
				}
			}
		}

		private void SetWalls(Room room, Transform roomTForm, RoomDirection direction)
		{
			//fetch correct wall prefab
			GameObject newWalls;
			switch(direction)
			{
				case Left:
					newWalls = wallsLeftPrefab;
					break;
				case Both:
					newWalls = wallsBothPrefab;
					break;
				case Right:
					newWalls = wallsRightPrefab;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
					
			//set walls correctly
			DestroyImmediate(room.walls);
			room.walls = Instantiate(newWalls, roomTForm);
			room.roomOrientation = direction;
		}

		public void Reset()
		{
			var roomList = new List<Transform>();

			for (int i = 0; i < transform.childCount; i++)
			{
				roomList.Add(transform.GetChild(i));
			}

			foreach (Transform tForm in roomList)
			{
				DestroyImmediate(tForm.gameObject);
			}
		}
	}
}
