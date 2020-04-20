using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			ParticleSystem.EmissionModule em = ps.emission;
			ParticleSystem.SizeOverLifetimeModule solm = ps.sizeOverLifetime;
			ParticleSystem.MinMaxCurve mmc = solm.size;

			switch (fireLevel)
			{
				case 1:
					print("case 1");
					ps.Play();
					em.rateOverTime = 2;
					mmc.constant = 0.5f;
					break;
				case 2:
					print("case 2");

					ps.Play();
					em.rateOverTime = 5;
					mmc.constantMax = 1;
					break;
				case 3 :
					print("case 3");

					ps.Play();
					em.rateOverTime = 5;
					mmc.constantMax = 3;
					break;
				default:
					print("default");

					em.rateOverTime = 0; 
					ps.Stop();
					break;
			}
		} 
	}

	#endregion

    #region private methods
    #endregion
}
