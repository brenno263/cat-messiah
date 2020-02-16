using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{

    //set in inspector
    public float width;
    public float depth = 0.1f;

    //set dynamically
    public bool isGroundedLeft;
    public bool isGroundedRight;
    public bool isGrounded { get { return isGroundedLeft || isGroundedRight; } }

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 posLeft = transform.position;
        posLeft.x -= width / 2;
        Vector2 posRight = transform.position;
        posRight.x += width / 2;

        RaycastHit2D hitLeft = Physics2D.Raycast(posLeft, Vector2.down, depth, LayerMask.GetMask("Ground"));
        RaycastHit2D hitRight = Physics2D.Raycast(posRight, Vector2.down, depth, LayerMask.GetMask("Ground"));

        if(hitLeft.collider == null)
        {
            isGroundedLeft = false;
        } else
        {
            isGroundedLeft = true;
        }
        isGroundedRight = hitRight.collider != null;
    }

    public static implicit operator bool(IsGrounded isGrounded)
    {
        return isGrounded.isGrounded;
    }

    private void OnDrawGizmos()
    {
        Vector2 posLeft = transform.position;
        posLeft.x -= width / 2;
        Vector2 posRight = transform.position;
        posRight.x += width / 2;

        Gizmos.DrawLine(posLeft, posLeft + Vector2.down * depth);
        Gizmos.DrawLine(posRight, posRight + Vector2.down * depth);
    }
}
