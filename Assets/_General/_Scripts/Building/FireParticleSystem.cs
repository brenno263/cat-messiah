using UnityEngine;

namespace _General._Scripts.Building
{
	public class FireParticleSystem : MonoBehaviour
	{
		#region variables
		//[Header("Set in Inspector")]
		[Header("Set Dynamically")]

		private int fireLevel = 0;
	
		#endregion

		#region monobehavior methods

		private void Start()
		{
			fireLevel = 0;
		}
	
		public int FireLevel
		{
			get => fireLevel;
			set
			{
				fireLevel = value;
				ParticleSystem ps = this.gameObject.GetComponent<ParticleSystem>();
				ParticleSystem.MainModule main = ps.main;
				ParticleSystem.EmissionModule em = ps.emission;

				switch (fireLevel)
				{
					case 1:
						ps.Play();
						em.rateOverTime = 3;
						main.startSize = 0.5f;
						break;
					case 2:
						ps.Play();
						em.rateOverTime = 3;
						main.startSize = 1.5f;
						break;
					case 3 :
						ps.Play();
						em.rateOverTime = 5;
						main.startSize = 3;
						break;
					default:
						em.rateOverTime = 0; 
						main.startSize = 1;
						ps.Stop();
						break;
				}
			} 
		}

		#endregion

		#region private methods
		#endregion
	}
}
