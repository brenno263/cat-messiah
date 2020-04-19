using System.Collections.Generic;
using UnityEngine;

namespace _General._Scripts.Player
{
	public class PlayerTemperature : MonoBehaviour
	{
		#region variables

		//placeholder for RoomTracker -> CurrentRoom() -> heatLevel
		public int heatLevel = 1;

		[Header("Set in Inspector")]
		public float maxTemperature;

		public Color normalColor;
		public Color hotColor;

		public SpriteRenderer spriteRenderer;

		//contains a heating rate in degrees/second for each level of burning room
		public List<float> heatingRates;

		[Header("Set Dynamically")]
		public float temperature;

		//[Header("Fetched on Init")]

		#endregion

		#region monobehavior methods

		void Start()
		{
			spriteRenderer.color = hotColor;
		}

		void Update()
		{
			spriteRenderer.color = Color.Lerp(normalColor, hotColor, temperature / maxTemperature);

			if (temperature < maxTemperature)
			{
				temperature = Mathf.Max(
					temperature + heatingRates[heatLevel] * Time.deltaTime,
					0);
			}
			else
			{
				//get hella dead
			}

			heatLevel = Input.GetAxis("Vertical") > 0.1 ? 1 : 0;
		}

		#endregion

		#region private methods

		#endregion
	}
}