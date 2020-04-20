using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable PossibleMultipleEnumeration

namespace _General._Scripts.Building
{
	[Serializable]
	public class RoomSprites
	{
		public string name;
		public List<Sprite> sprites;
	}
	
	public class Building : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public float fireLevelAdvanceTimerMin;

		public float fireLevelAdvanceTimerMax;

		public int updateFireTick;

		public double spreadFireChance;

		public double randomFireChance;

		[SerializeField]
		public List<RoomSprites> roomBackgroundDictionary;

		[Header("Set Dynamically")]
		public Room[,] rooms;

		public int numRooms;
		public int netFireLevel;

		public int xMax;
		public int yMax;

		public bool[,] roomExists;

		[Header("Fetched on Init")]
		public static Building Singleton;

		#endregion

		#region monobehavior methods

		private void Awake()
		{
			if (Singleton == null) Singleton = this;
		}

		private void Start()
		{
			var roomsArr = transform.GetComponentsInChildren<Room>();

			xMax = roomsArr.OrderBy(t => t.x).Last().x + 1;
			yMax = roomsArr.OrderBy(t => t.y).Last().y + 1;

			roomExists = new bool[xMax, yMax];
			for (int i = 0; i < xMax; i++)
			{
				for (int j = 0; j < yMax; j++) { roomExists[i, j] = false; }
			}

			rooms = new Room[xMax, yMax];
			foreach (Room room in roomsArr)
			{
				rooms[room.x, room.y] = room;
				roomExists[room.x, room.y] = true;
				numRooms++;
			}
			
			StartCoroutine(UpdateFires());
			GenerateRandomFire();
		}

		#endregion

		#region private methods

		/// <summary>
		/// Use StopAllCoroutines to stop.
		/// </summary>
		private IEnumerator UpdateFires()
		{
			while (true)
			{
				SpreadFires();

				yield return null; //split execution over two frames

				if (Random.value <= randomFireChance) { GenerateRandomFire(); }

				yield return new WaitForSeconds(updateFireTick);
			}
		}

		private void SpreadFires()
		{
			//counts total fire level for building
			netFireLevel = 0;
			//Goes through each burning room, and gives the adjacent rooms "points" for a chance
			//to catch on fire. The more points, the more likely it'll catch
			var fireGrid = new int[xMax, yMax];
			for (int i = 0; i < xMax; i++)
			{
				for (int j = 0; j < yMax; j++)
				{
					if (roomExists[i, j] && rooms[i, j].OnFire())
					{
						Room flamingRoom = rooms[i, j];
						int basePoints = flamingRoom.FireLevel - 1;

						netFireLevel++;
						
						//right
						if (IsSafeRoom(i + 1, j))
						{
							int points = basePoints;
							if (! (rooms[i, j].rightDoor != null && !rooms[i, j].rightDoor.IsOpen
							    || rooms[i, j].roomOrientation != RoomDirection.Left)) {points += 2;  }

							fireGrid[i + 1, j] += points;
						}

						//down
						if (IsSafeRoom(i, j + 1)) { fireGrid[i, j + 1] += basePoints + (rooms[i, j + 1].hasStairs ? 2 : 1); }

						//left
						if (IsSafeRoom(i - 1, j))
						{
							int points = basePoints;
							
							if (! (rooms[i, j].leftDoor != null && !rooms[i, j].leftDoor.IsOpen
							    || rooms[i, j].roomOrientation == RoomDirection.Right)) { points += 2;}

							fireGrid[i - 1, j] += points;
						}

						//up
						if (IsSafeRoom(i, j - 1)) { fireGrid[i, j - 1] += basePoints + (rooms[i, j - 1].hasStairs ? 2 : 1); }
					}
				}
			}

			//Goes through each non-burning room, and for every point the room has, add increase chance of catching on fire.
			//four points means 100% of the specified room catch chance
			for (int i = 0; i < xMax; i++)
			{
				for (int j = 0; j < yMax; j++)
				{
					if (Random.value <= spreadFireChance * fireGrid[i, j] / 4)
					{
						rooms[i, j].setRoomOnFire();
						print("Room caught on fire!");
					}
				}
			}
		}

		private bool IsSafeRoom(int x, int y)
		{
			return -1 < x && x < xMax
			              && -1 < y && y < yMax
			              && roomExists[x, y] && !rooms[x, y].OnFire();
		}

		private void GenerateRandomFire()
		{
			List<Room> safeRoomsList = new List<Room>();
			for (int i = 0; i < xMax; i++)
			{
				for (int j = 0; j < yMax; j++)
				{
					if (roomExists[i, j] && !rooms[i, j].OnFire()) { safeRoomsList.Add(rooms[i, j]); }
				}
			}

			if (safeRoomsList.Count > 0)
			{
				safeRoomsList[Random.Range(0, safeRoomsList.Count)].setRoomOnFire();
				print("Random room caught on fire!");
			}
		}

		#endregion
	}
}