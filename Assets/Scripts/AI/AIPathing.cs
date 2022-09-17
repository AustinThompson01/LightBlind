using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathing : MonoBehaviour
{
    [HideInInspector]
    public BoxCollider2D b2d;
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public GameObject invObj;
    [HideInInspector]
    public Rigidbody2D rb2d;

    public float idleTime = 1f;
    public float walkSpeed = 10f;

    public State state;

    public enum State
    {
        patrolling,
        idle,
        chase,
        investigate,
        run
    }

    // Start is called before the first frame update
    void Start()
    {
        b2d = GetComponent<BoxCollider2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case State.patrolling:
                patrol();
                break;

            case State.idle:
                idle(idleTime);
                break;
            case State.chase:
                chase();
                break;
            case State.investigate:
                investigate(invObj);
                break;
            case State.run:

                break;

        }
    }

    /// <summary>
    /// Controls patroling
    /// </summary>
    public void patrol()
    {
        rb2d.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb2d.velocity.y);
    }

    /// <summary>
    /// Turns the dude
    /// </summary>
    public void turn()
    {
        state = State.idle;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
    }

    /// <summary>
    /// Makes the dude wait
    /// </summary>
    public void idle(float i)
    {
        StartCoroutine(wait(i));
    }

    /// <summary>
    /// See above
    /// </summary>
    public IEnumerator wait(float f)
    {
        yield return new WaitForSeconds(f);
        if(state == State.idle)
        {
            state = State.patrolling;
        }
    }
    public void chase()
    {

    }

    public void investigate(GameObject obj)
    {
        invObj = obj;
        state = State.investigate;

    }
}
