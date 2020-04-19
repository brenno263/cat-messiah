using UnityEngine;

namespace _General._Scripts.Building
{
	public class Door : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]

		public GameObject doorObject;
		
		//[Header("Set Dynamically")] 

		public bool IsOpen
		{
			get => isOpen;
			set
			{
				doorObject.SetActive(!value);
				isOpen = value;
			}
		}
		
		[SerializeField]
		private bool isOpen = false;
		
		#endregion

		#region monobehavior methods

		private void Start()
		{
        
		}

		private void Update()
		{
        
		}
		#endregion

		#region private methods
		#endregion
	}
}
