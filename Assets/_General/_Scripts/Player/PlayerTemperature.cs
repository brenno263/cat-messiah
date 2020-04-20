using System.Collections.Generic;
using UnityEngine;

namespace _General._Scripts.Player
{
	public class PlayerTemperature : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public float maxTemperature;

		public Color normalColor;
		public Color hotColor;

		public RoomTracker roomTracker;

		public SpriteRenderer spriteRenderer;

		//contains a heating rate in degrees/second for each level of burning room
		public List<float> heatingRates;

		[Header("Set Dynamically")]
		public int heatLevel = 0;

		public float temperature;

		//[Header("Fetched on Init")]

		#endregion

		#region monobehavior methods

		private void Start()
		{
			spriteRenderer.color = normalColor;
		}

		private void Update()
		{
			heatLevel = !roomTracker.inRoom ? 0 : roomTracker.currentRoom.FireLevel;

			if (temperature <= 0.01f && heatLevel == 0) return;
			spriteRenderer.color = Color.Lerp(normalColor, hotColor, temperature / maxTemperature);

			if (temperature < maxTemperature)
			{
				temperature = Mathf.Max(
					temperature + heatingRates[heatLevel] * Time.deltaTime,
					0);
			}
			else
			{
				GetComponent<Player>().Burn();
				temperature = 0;
			}
		}

		#endregion

		#region private methods

		#endregion
	}
}