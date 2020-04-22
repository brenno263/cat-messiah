using System;
using _General._Scripts.Building;
using UnityEngine;

namespace _General._Scripts.Player
{
	public class RoomTracker : MonoBehaviour
	{
		#region variables
		//[Header("Set in Inspector")]
		//[Header("Set Dynamically")]
		//[Header("Fetched on Init")]

		public bool inRoom;
		
		public Room currentRoom;

		public Room lastRoom;
	
		#endregion

		#region monobehavior methods

		private void OnTriggerEnter2D(Collider2D other)
		{
			Room room = other.GetComponent<Room>();
			if (room == null) return;
			
			lastRoom = currentRoom;
			currentRoom = room;
			inRoom = true;
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			Room room = other.GetComponent<Room>();
			if (room == null || currentRoom == null) return;

			if (room == currentRoom)
			{
				lastRoom = currentRoom;
				currentRoom = null;
				inRoom = false;
			}
		}

		#endregion

		#region private methods
		#endregion
	}
}
