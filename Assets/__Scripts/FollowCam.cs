using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    #region variables
    [Header("Set in Inspector")]
    public Transform playerTrans;
    public float acceleration;
    public bool constrainX;
    public Vector2 xBounds;
    public bool constrainY;
    public Vector2 yBounds;
	//[header("Set Dynamically")]
	//[header("Fetched on Init")]
	#endregion

	#region monobehavior methods
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        Vector2 targetPos = playerTrans.position;
        
        if(constrainX)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, xBounds.x, xBounds.y);
        }
        if (constrainY)
        {
            targetPos.y = Mathf.Clamp(targetPos.y, yBounds.x, yBounds.y);
        }

        pos = Vector2.Lerp(pos, targetPos, acceleration * Time.fixedDeltaTime);
        pos.z = transform.position.z;

        transform.position = pos;
    }

    private void OnDrawGizmos()
    {
        if (constrainX)
        {
            Gizmos.DrawLine(new Vector3(xBounds.x, -100, 0), new Vector3(xBounds.x, 100, 0));
            Gizmos.DrawLine(new Vector3(xBounds.y, -100, 0), new Vector3(xBounds.y, 100, 0));


        }
        if (constrainY)
        {
            Gizmos.DrawLine(new Vector3(-100, yBounds.x, 0), new Vector3(100, yBounds.x, 0));
            Gizmos.DrawLine(new Vector3(-100, yBounds.y, 0), new Vector3(100, yBounds.y, 0));

        }
    }
    #endregion

    #region private methods
    #endregion
}
