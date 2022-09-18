using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTest : MonoBehaviour
{
	public Throw thrown;
	public bool grabbed;
	RaycastHit2D hit;
	public float distance = 2f;
	public Transform holdpoint;
	public float throwforce;
	public LayerMask notgrabbed;
	

	public GameObject grabbable;


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.V))
        {
			toss();
        }
		if (Input.GetKeyDown(KeyCode.N))
		{
			pit();
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			Physics2D.queriesStartInColliders = false;

			hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);

			if (hit.collider != null && hit.collider.tag == "grabbable" && !grabbed)
			{
				grabbed = true;
				grabbable = hit.collider.gameObject;
			}


			//grab
			/*
			else if (!Physics2D.OverlapPoint(holdpoint.position, notgrabbed))
			{
				grabbed = false;

				if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
				{

					hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * throwforce;
				}


				//throw
			}

			*/
		}

		if (grabbed)
		{
			grabbable.transform.position = holdpoint.position;
		}


	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * distance);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		/*
		if (Input.GetKeyDown(KeyCode.B))
		{

			if (!grabbed)
			{
				Debug.Log("In pickup");
				if (other.tag == "grabbable")
				{
					grabbed = true;
					grabbable = hit.collider.gameObject;
					hit.collider.gameObject.transform.position = holdpoint.position;

				}

				//grab
			}
			else if (!Physics2D.OverlapPoint(holdpoint.position, notgrabbed))
			{
				Debug.Log("In overlap thingy");
				grabbed = false;
				hit.collider.gameObject.GetComponent<Throw>().enabled = true;

				if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
				{

					hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * throwforce;
				}


				//throw
			}

			else if (grabbed)
            {
				Debug.Log("In throw");
				grabbed = false;
				grabbable.GetComponent<Throw>().enabled = true;

				if (grabbable.GetComponent<Rigidbody2D>() != null)
				{

					grabbable.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * throwforce;
				}

			}
		}

		*/
		/*if (!grabbed)
		{
			other.gameObject.transform.position = holdpoint.position;
			grabbed = true;
		}*/
		
	}

	/*
	private void OnTriggerExit2D(Collider2D other)
	{
		grabbed = false;
	}
	*/
	public void pit()
	{
		grabbed = false;
		grabbable.GetComponent<Throw>().enabled = true;
		grabbable = null;

	}
	public void toss()
    {
		/*
		if (grabbed)
		{
			grabbable.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * throwforce;
			grabbable.GetComponent<Throw>().enabled = true;
		}
		*/
		grabbed = false;
		grabbable.GetComponent<SoundObj>().thrown();
		grabbable.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * throwforce;
		grabbable.GetComponent<Throw>().enabled = true;
		grabbable = null;
	}
	
}
