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

    public LayerMask wallLayer;

    public float idleTime = 1f;
    public float walkSpeed = 50f;
    public float runSpeed = 150f;
    public float chaseSpeed = 10f;
    public float runTime = 1f;
    public float seeDist = 50f;

    public float invDist = 3f;

    public BoxCollider2D sightBox;

    private bool mustTurn = false;
    private bool turnCD = false;
    private bool outsideRange = true;
    private bool attracted = false;

    private Vector2 followVector;

    int layerMask = ~(1 << 9);
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
            //patrolls an area until it hits wall, working
            case State.patrolling:
                patrol();
                break;

            //Makes the creature stand still, working
            case State.idle:
                idle(idleTime);
                break;

            //Makes the creature chase the player, working
            case State.chase:
                chase();
                break;

            //Makes the creature investigate object, in progress
            case State.investigate:
                investigate(invObj, attracted);
                break;

            //Move in direction fast, working
            case State.run:
                run();
                break;

        }

        //Debug.Log(outsideRange);
        if(outsideRange && state != State.chase)
        {
            //Debug.Log("Far enough away ");

            if (transform.localScale.x > 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, seeDist, layerMask);
                Debug.Log(hit.transform.gameObject);
                if(hit.transform.gameObject.tag == "Player" && outsideRange)
                {
                    state = State.chase;
                }
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.right, seeDist, layerMask);
                //Debug.Log(hit.transform.gameObject);
                if (hit.transform.gameObject.tag == "Player" && outsideRange)
                {
                    state = State.chase;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        mustTurn = Physics2D.OverlapCircle(transform.position, 0.1f, wallLayer);
        if(turnCD)
        {
            turnCD = mustTurn;
        }
    }

    /// <summary>
    /// Controls patroling
    /// </summary>
    public void patrol()
    {
        if (mustTurn && !turnCD)
        {
            turnCD = true;
            turn();
        }
        else
        {
            rb2d.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb2d.velocity.y);
        }
    }

    /// <summary>
    /// Turns the dude
    /// </summary>
    public void turn()
    {
        state = State.idle;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        runSpeed *= -1;
        //chaseSpeed *= -1;
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

        //Will not be in idle if player is seen
        if (state == State.idle)
        {
            state = State.patrolling;
            //Should prevent infinite turning
            //yield return new WaitForSeconds(f);
            //turnCD = false;
            //Debug.Log("Wait over");
        }
    }

    public void chase()
    {
        if (mustTurn && !turnCD)
        {
            Debug.Log("Chase turn");
            turnCD = true;
            turn();
        }
        else
        {
            //transform.position = Vector2.MoveTowards(transform.position, target.transform.position, chaseSpeed * Time.fixedDeltaTime);
            followVector = Vector2.MoveTowards(transform.position, target.transform.position, 0.06f);
            rb2d.MovePosition(followVector);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            outsideRange = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            outsideRange = true;
        }
    }

    /// <summary>
    /// Goes to investigate object
    /// </summary>
    public void investigate(GameObject obj, bool attrac)
    {
        Debug.Log("investirage called");
        invObj = obj;
        state = State.investigate;
        attracted = attrac;
        if (attracted)
        {
            if (mustTurn && !turnCD)
            {
                Debug.Log("Chase turn");
                turnCD = true;
                turn();
            }
            else
            {
                //transform.position = Vector2.MoveTowards(transform.position, target.transform.position, chaseSpeed * Time.fixedDeltaTime);
                followVector = Vector2.MoveTowards(this.transform.position, invObj.transform.position, 0.06f);
                rb2d.MovePosition(followVector);
            }

            if (Vector2.Distance(this.transform.position, invObj.transform.position) < invDist)
            {
                Debug.Log(Vector2.Distance(this.transform.position, invObj.transform.position));
                Debug.Log("going to idle");
                state = State.idle;
            }
        }
        else
        {
            turn();
            run();
        }
    }

    /// <summary>
    /// runs
    /// </summary>
    public void run()
    {
        state = State.run;
        StartCoroutine(runTiming());
        if (mustTurn && !turnCD)
        {
            turnCD = true;
            turn();
        }
        else
        {
            rb2d.velocity = new Vector2(runSpeed * Time.fixedDeltaTime, rb2d.velocity.y);
        }
    }

    /// <summary>
    /// How long monster can run
    /// </summary>
    public IEnumerator runTiming()
    {
        yield return new WaitForSeconds(runTime);
        //Ensures that if run is over or if it is interrupted this doesn't just set it to idle
        if(state == State.run)
        {
            state = State.idle;
        }
    }
}
