using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAfterFall : MonoBehaviour
{
    Vector3 startPosition;
    public float minimumY = -100;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // preventing the character from going out of certain bounds
        if (transform.position.y < minimumY)
        {
            transform.position = startPosition;
        }
    }
}
