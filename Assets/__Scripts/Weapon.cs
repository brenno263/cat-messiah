using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region variables
    [Header("Set in Inspector")]
    public GameObject projectileGO;
	//[header("Set Dynamically")]
	//[header("Fetched on Init")]
	#endregion

	#region monobehavior methods
    void Start()
    {

    }

    #endregion

    #region private methods
    public void fire()
    {
        GameObject go = Instantiate<GameObject>(projectileGO);
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;

    }
    #endregion
}
