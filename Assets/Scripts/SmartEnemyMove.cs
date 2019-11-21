using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    CircleCollider2D circleCollider;

    public int nextMove;
    public float movePower = 1f;
    bool isTracing;
    GameObject traceTarget;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        StartCoroutine("Think");
    }

    void FixedUpdate()
    {
        //Move
        Move();

        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 0, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            StopCoroutine("Think");
            StartCoroutine("Turn");
        }
    }

    void Move()
    {
        Vector3 velocity = Vector3.zero;

        if (isTracing)
        {
            Vector3 playerPos = traceTarget.transform.position;

            if (playerPos.x < transform.position.x)
                nextMove = -1;
            else if (playerPos.x > transform.position.x)
                nextMove = 1;
        }

        if (nextMove == -1)
        {
            velocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (nextMove == 1)
        {
            velocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        transform.position += velocity * movePower * Time.deltaTime;
    }

    //recursive function
    IEnumerator Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2);

        //Sprite Animation
        if (nextMove == 0)
            anim.SetBool("isMoving", false);
        else
            anim.SetBool("isMoving", true);

        //Recursive
        yield return new WaitForSeconds(3f);
        StartCoroutine("Think");
    }

    IEnumerator Turn()
    {
        nextMove = nextMove * -1;           //nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        StopCoroutine("Think");
        yield return new WaitForSeconds(3f);
        StartCoroutine("Think");
    }

    //Trace Start
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            traceTarget = other.gameObject;

            StopCoroutine("Think");
        }
    }

    //Trace Maintain
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            traceTarget = other.gameObject;
            isTracing = true;
            anim.SetBool("isMoving", true);
        }
    }

    //Trace Maintain
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = false;

            StartCoroutine("Think");
        }
    }
}