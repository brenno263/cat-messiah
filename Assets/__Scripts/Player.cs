using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Set in Inspector")]
    public float acceleration;
    public float topSpeed;
    public GameObject WeapAnchor;


    [Header("Set Dynamically")]
    private Rigidbody2D rigid;
    private HashSet<Weapon> weapons;
    public Vector3 MoveInput;
    public Vector3 FireInput;

    // Start is called before the first frame update
    void Start()
    {
        MoveInput = new Vector3();

        rigid = GetComponent<Rigidbody2D>();
        weapons = new HashSet<Weapon>(GetComponentsInChildren<Weapon>());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveInput.x = Input.GetAxis("MoveX");
        MoveInput.y = Input.GetAxis("MoveY");
        FireInput.x = Input.GetAxis("FireX");
        FireInput.y = Input.GetAxis("FireY");
        Move();
        Aim();
        foreach(Weapon weap in weapons)
        {
            if(FireInput.magnitude > 0)
            {
                weap.fire();
            }
        }
    }

    void Move()
    {
        //string[] joyNames = Input.GetJoystickNames();
        //foreach(string name in joyNames)
        //{
        //    print(name);
        //}
        Vector2 vel = rigid.velocity;
        Vector2 input = new Vector2(Input.GetAxis("MoveX"), Input.GetAxis("MoveY"));
        vel = Vector2.Lerp(vel, input * topSpeed, acceleration);
        rigid.velocity = vel;
    }

    void Aim()
    {
        Vector3 YDir = new Vector3(-Input.GetAxis("FireY"), Input.GetAxis("FireX"), 0);
        WeapAnchor.transform.localRotation = Quaternion.LookRotation(Vector3.forward, YDir);
    }
}
