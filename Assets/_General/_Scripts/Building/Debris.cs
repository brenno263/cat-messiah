using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
	#region variables

	[Header("Set in Inspector")]
	public float torqueMax;
	//[Header("Set Dynamically")]
	//[Header("Fetched on Init")]
	#endregion

	#region monobehavior methods
    void Start()
    {
	    Rigidbody2D rigid = this.GetComponent<Rigidbody2D>();
	    if (rigid != null)
	    {
		    rigid.AddTorque((Random.value - 0.5f) * torqueMax);
	    }
    }

    void Update()
    {
        
    }
    #endregion

    #region private methods
    #endregion
}
