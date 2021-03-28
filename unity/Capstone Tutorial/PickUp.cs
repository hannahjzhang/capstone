using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUp : MonoBehaviour
{
    public GameObject activateOnTouch;
    public UnityEvent onTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        Debug.Log(other.gameObject);
        onTrigger.Invoke();

        if (activateOnTouch)
        {
            activateOnTouch.SetActive(true);
        }
    }
    public void hello()
    {
        Debug.Log("Hello World");
    }
}
