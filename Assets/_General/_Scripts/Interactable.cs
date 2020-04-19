using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _General._Scripts
{
	public enum InteractionType
	{
		Item, StairsUp, StairsDown, Door
	}

	public class Interactable : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public InteractionType type;

		public Action<Player.Player> onInteract;
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
	}
}