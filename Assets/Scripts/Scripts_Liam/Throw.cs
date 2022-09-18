using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public float Speed = 4;
    public Vector3 LaunchOffset;
    public bool Thrown;
    // Start is called before the first frame update
    private void Start()
    {
        if (Thrown)
        {
            var direction = -transform.right + Vector3.up;
            GetComponent<Rigidbody2D>().AddForce(direction * Speed, ForceMode2D.Impulse);
        }
        transform.Translate(LaunchOffset);

      
    }
    public void Update()
    {
        if (!Thrown)
        {
            transform.position += -transform.right * Speed * Time.deltaTime;
        }

    }

}
