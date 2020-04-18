using UnityEngine;

namespace _General._Scripts
{
	public class RoomTracker : MonoBehaviour
	{
		#region variables
		//[Header("Set in Inspector")]
		//[Header("Set Dynamically")]
		//[Header("Fetched on Init")]

		private string _currentRoom = null; //update to type room
	
		#endregion

		public string GetRoom()
		{
			return _currentRoom;
		}
	
		#region monobehavior methods

		private void OnTriggerEnter(Collider other)
		{
			//update the current room. This object only collides with room triggers
		}

		#endregion

		#region private methods
		#endregion
	}
}
