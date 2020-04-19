using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Building : MonoBehaviour
{
	#region variables

	[Header("Set in Inspector")]

	public int updateFireWait;

	public double spreadFireChance;

	public double randomFireChance;
	
	[Header("Set Dynamically")] 
	
	public Room[,] rooms;

	#endregion

	#region monobehavior methods
    void Start()
    {
        initLevelOne();
        StartCoroutine(UpdateFires());
    }

    void FixedUpdate()
    {
        
    }
    
    #endregion

    #region private methods

    void initLevelOne()
    {
	    Room[] roomsList = transform.GetComponentsInChildren<Room>();
	    rooms = new Room[3, 3];
	    foreach (Room room in roomsList)
	    {
		    rooms[room.x, room.y] = room;
	    }

	    // int fireRoomCount = 2; 
	    // Random r = new Random();
	    // Room rm;
	    // while (fireRoomCount > 0)
	    // {
		   //  rm = roomsList[r.Next(roomsList.Length)];
		   //  if (rm != null && !rm.onFire())
		   //  {
			  //   rm.setRoomOnFire();
			  //   fireRoomCount--;
		   //  }
	    // }
    }

    private IEnumerator UpdateFires()
    {
	    Random r = new Random();
	    while (true)
	    {
		    yield return new WaitForSeconds(updateFireWait);
		    SpreadFires();
		    if (r.NextDouble() <= randomFireChance)
		    {
			    GenerateRandomFire();
		    }
	    }
    }

    private void SpreadFires()
    {
	    //Goes through each burning room, and gives the adjacent rooms "points" for a chance
	    //to catch on fire. The more points, the more likely it'll catch
	    int[,] fireGrid = new int[rooms.GetLength(0), rooms.GetLength(1)];
	    for (int i = 0; i < rooms.GetLength(0); i++)
	    {
		    for (int j = 0; j < rooms.GetLength(1); j++)
		    {
			    if (rooms[i, j] != null && rooms[i, j].onFire())
			    {
				    //right
				    if (isSafeRoom(i + 1, j))
				    {
					    if ((rooms[i, j].rightDoor != null && !rooms[i, j].rightDoor.isOpen)
						    || rooms[i, j].roomOrientation == RoomDirection.Left)
					    {
						    fireGrid[i + 1, j]++;
					    }
					    else
					    {
						    fireGrid[i + 1, j] += 2;
					    }
				    }
				    //down
				    if (isSafeRoom(i, j + 1))
				    {
					    fireGrid[i, j + 1]++;
				    }
				    //left
				    if (isSafeRoom(i - 1, j))
				    {
					    if ((rooms[i, j].leftDoor != null && !rooms[i, j].leftDoor.isOpen)
					        || rooms[i, j].roomOrientation == RoomDirection.Right)
					    {
						    fireGrid[i - 1, j]++;
					    }
					    else
					    {
						    fireGrid[i - 1, j] += 2;
					    }
				    }
				    //up
				    if (isSafeRoom(i, j - 1))
				    {
					    fireGrid[i, j - 1]++;
				    }
			    }
		    }
	    }

	    //Goes through each non-buring room, and for every point the room has, it has a chance of catching on fire.
	    Random r = new Random();
	    for (int i = 0; i < fireGrid.GetLength(0); i++)
	    {
		    for (int j = 0; j < fireGrid.GetLength(1); j++)
		    {
			    for (int k = 0; k < fireGrid[i, j]; k++)
			    {
				    if (r.NextDouble() <= spreadFireChance)
				    {
					    rooms[i, j].setRoomOnFire();
					    print("Room caught on fire!");
				    }
			    }
		    }
	    }
    }
    
    private bool isSafeRoom(int x, int y)
    {
	    return -1 < x && x < rooms.GetLength(0)
	                  && -1 < y && y < rooms.GetLength(1)
	                  && rooms[x, y] != null && !rooms[x, y].onFire();
    }

    private void GenerateRandomFire()
    {
	    List<Room> safeRoomsList = new List<Room>();
	    for (int i = 0; i < rooms.GetLength(0); i++)
	    {
		    for (int j = 0; j < rooms.GetLength(1); j++)
		    {
			    if (rooms[i, j] != null && !rooms[i, j].onFire())
			    {
				    safeRoomsList.Add(rooms[i, j]);
			    }
		    }
	    }

	    if (safeRoomsList.Count > 0)
	    {
		    Random r = new Random();
		    safeRoomsList[r.Next(safeRoomsList.Count)].setRoomOnFire();
		    print("Random room caught on fire!");
	    }
    }

    #endregion
}
