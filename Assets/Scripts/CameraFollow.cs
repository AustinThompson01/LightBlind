using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject toFollow;
    private Vector3 v3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(toFollow != null)
        {
            v3 = new Vector3(toFollow.transform.position.x, toFollow.transform.position.y, transform.position.z);
            transform.position = v3;
        }
    }
}
