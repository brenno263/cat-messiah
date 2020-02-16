using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Set in Inspector")]
    public float acceleration;
    public float topSpeed;


    [Header("Set Dynamically")]
    public static Player singleton;
    private Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< Updated upstream
=======
        singleton = this;
        MoveInput = new Vector3();

>>>>>>> Stashed changes
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector2 vel = rigid.velocity;
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        vel = Vector2.Lerp(vel, input * topSpeed, acceleration);
        rigid.velocity = vel;
        string[] names = Input.GetJoystickNames();
        foreach(string name in names)
        {
            print(name);
        }
    }
}
