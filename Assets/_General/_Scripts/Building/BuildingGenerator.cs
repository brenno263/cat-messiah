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

		public Vector2 roomSize;

		public GameObject wallsBothPrefab;
		public GameObject wallsRightPrefab;
		public GameObject wallsLeftPrefab;
		public GameObject floorPrefab;

		public GameObject doorPrefab;

		public Vector2 doorOffset;

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
    
		public void Run(int width, int height, GameObject buildingGO)
		{
			var roomGOs = new GameObject[width, height];
			var rooms = new Room[width, height];
			Building building = GetComponent<Building>();

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					GameObject roomGO = Instantiate(roomPrefab, buildingGO.transform);
					Transform roomTForm = roomGO.transform;
					Room room = roomGO.GetComponent<Room>();
					
					roomGOs[x, y] = roomGO;
					rooms[x, y] = room;

					roomGO.name = "Room(" + x + "," + y + ")";
					
					roomTForm.localPosition = new Vector3(x * roomSize.x, y * roomSize.y, 0);
					
					room.x = x;
					room.y = y;
					room.building = building;
					
					RoomDirection direction = Both;
					if (x == 0) direction = Right;
					if (x == width - 1) direction = Left;
					if (x == 0 && y == 0) direction = Both;
					
					SetWalls(room, roomTForm, direction);
				}
			}
			print("fetching");
			//GameObject topFloor = Instantiate(new GameObject(), building.transform);
			//topFloor.name = "Top Floor";
			
			for (int x = 0; x < width; x++)
			{
				int y = height;
				GameObject floorGO = Instantiate(floorPrefab, building.transform);
				floorGO.transform.localPosition = new Vector3(x * roomSize.x, y * roomSize.y, 0);
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
					doorPos.x += roomSize.x + doorOffset.x;
					doorPos.y += doorOffset.y;
					doorGO.transform.position = doorPos;

					room.rightDoor = door;
					nextRoom.leftDoor = door;
					door.IsOpen = true;
				}
			}

			Room entry = rooms[0, 0];
			GameObject entryDoorGO = Instantiate(doorPrefab, buildingGO.transform);
			Vector3 entryDoorPos = entry.transform.position;
			entryDoorPos.x += doorOffset.x;
			entryDoorPos.y += doorOffset.y;
			entryDoorGO.transform.position = entryDoorPos;
			
			Door entryDoor = entryDoorGO.GetComponent<Door>();
			entry.leftDoor = entryDoor;
			entryDoor.IsOpen = true;
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
