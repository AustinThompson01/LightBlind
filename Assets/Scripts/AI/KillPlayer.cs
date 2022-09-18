using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public GameObject parent;
    private bool attackOnce = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && attackOnce)
        {
            attackOnce = false;
            parent.GetComponent<AIPathing>().attack();
        }
    }
}
