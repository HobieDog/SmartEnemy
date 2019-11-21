using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startPos; //Start
    public Transform endPos;   //End
    public Transform desPos;
    public int platType;
    public float speed;

    void Start()
    {
        transform.position = startPos.position;
        desPos = endPos;
    }

    void FixedUpdate()
    {
        if (platType == 0)
        {
            move();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (platType == 1)
            {
                move();
            }
        }
    }

    void move()
    {
        transform.position = Vector2.MoveTowards(transform.position, desPos.position, Time.deltaTime * speed);

        if (Vector2.Distance(transform.position, desPos.position) <= 0.05f)
        {
            if (desPos == endPos)
                desPos = startPos;
            else
                desPos = endPos;
        }
    }
}
