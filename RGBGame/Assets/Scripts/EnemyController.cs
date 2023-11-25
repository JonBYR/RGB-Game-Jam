using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 0.5f;
    private Transform playerTarget;
    private float flipDirection;
    private bool facingRight = false;
    private Rigidbody2D enemyRb;
    private void Awake()
    {
        playerTarget = GameObject.Find("Player").GetComponent<Transform>();
        enemyRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = playerTarget.position - transform.position;
        direction.Normalize();
        flipDirection = direction.x;
        if(flipDirection < 0 && !facingRight)
        {
            Flip();
        }
        else if (flipDirection > 0 && facingRight)
        {
            Flip();
        }
        transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, speed * Time.deltaTime);
        enemyRb.MovePosition((Vector2)transform.position + ((Vector2)direction * speed * Time.deltaTime));

    }
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Game Over");
        }
    }
}
