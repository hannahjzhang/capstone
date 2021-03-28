using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float speed = 5;
    public GameObject thing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float h = Input.GetAxisRaw("Horizontal"); // checks if you are pressing left or right
        float v = Input.GetAxisRaw("Vertical"); // checks if you are pressing forward or backward
        bool jumping = Input.GetButton("Jump"); // check for jump
        bool fire = Input.GetButtonDown("Fire1");

        if (fire)
        {
            GameObject go = Instantiate(thing);
            go.transform.position = transform.position + transform.forward;
        }
        float j = jumping ? 1 : rb.velocity.y / speed;
        Vector3 move = new Vector3(h, j, v); // h is x, j is y, v is z
        // transform.position += move * Time.deltaTime; // keep track of time to make moves slower

        // creating a velocity vector you can see
        NonStandard.Lines.Make("velocity").Arrow(transform.position, transform.position + rb.velocity, Color.red);

        rb.velocity = move * speed;
    }
}