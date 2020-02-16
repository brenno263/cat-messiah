using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace __Scripts
{
	public class Enemy : MonoBehaviour
	{
	    #region variables
	    [Header("Set in Inspector")]
	    public float maxHealth;
	    public Action onDeath;

	    //[Header("Set Dynamically")]
	    private float _health;
	    public float health
	    {
	        get { return _health; }
	        set
	        {
	            _health = Mathf.Min(maxHealth, value);
	            if (_health <= 0)
	            {
	                if (onDeath != null) onDeath();
	                else Destroy(gameObject);
	            }
	        }
	    }
		//[header("Fetched on Init")]
		#endregion

		#region monobehavior methods
	    void Start()
	    {
	        health = maxHealth;
	    }
	    #endregion

	    #region private methods
	    public void Damage(float amt)
	    {
	        health -= amt;
	    }
	    #endregion
	}
}
