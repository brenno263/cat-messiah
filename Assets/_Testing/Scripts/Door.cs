using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	#region variables

	[Header("Set in Inspector")] 
	public Sprite open;
	public Sprite closed;

	[Header("Set Dynamically")] 
	public bool isOpen;
	
	[Header("Fetched on Init")] 
	public SpriteRenderer spriteRenderer;
	public Collider2D doorCollider;
	#endregion

	#region monobehavior methods
    void Start()
    {
	    spriteRenderer = GetComponent<SpriteRenderer>();
	    doorCollider = GetComponent<Collider2D>();
	    isOpen = false;
	    spriteRenderer.sprite = closed;
	    doorCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
	    if (other.gameObject.name == "Key")
	    {
		    spriteRenderer.sprite = open;
		    isOpen = true;
		    doorCollider.enabled = false;
	    }
    }

    #endregion

    #region private methods
    #endregion
}
