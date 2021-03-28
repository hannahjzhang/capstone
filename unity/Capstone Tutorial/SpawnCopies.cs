using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCopies : MonoBehaviour
{
    public float when = 1;
    // Start is called before the first frame update
    void Start()
    {
        NonStandard.Clock.setTimeout(() =>
        {
            Instantiate(this.gameObject);
            Debug.Log("hello world" + this);
        }, (long)(when * 1000));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
