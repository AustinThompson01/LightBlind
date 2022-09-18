using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObj : MonoBehaviour
{
    public ThrowableObj to;
    public Collider2D triggerCol;
    public Collider2D physCol;

    private Rigidbody2D rb;
    private Animator anim;

    private bool th = false;
    private bool gr = false;

    private AudioSource Aud;

    // Start is called before the first frame update
    void Start()
    {
        triggerCol.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Aud = GetComponent<AudioSource>();
        Aud.clip = to.audio;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monster")
        {
            collision.GetComponent<AIPathing>().investigate(this.gameObject, to.attracts);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            physCol.enabled = false;
            triggerCol.enabled = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (th)
        {
            triggerCol.enabled = true;
            anim.SetTrigger("Ground");
            gr = true;
        }

        if (gr)
        {
            Debug.Log("Play audio");
            Aud.Play();
            if(to.attracts)
            {
                Destroy(this.gameObject, 2f);
            }
            else
            {
                Destroy(this.gameObject, 1f);
            }
        }
    }

    public void thrown()
    {
        anim.SetTrigger("Threw");
        th = true;
    }
}
