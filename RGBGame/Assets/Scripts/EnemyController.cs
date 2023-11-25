using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyController : MonoBehaviour
{
    public float speed = 0.5f;
    private Transform playerTarget;
    private PlayerController play;
    private Vector3 currentTarget;
    private float flipDirection;
    private bool facingRight = false;
    private Rigidbody2D enemyRb;
    private void Awake()
    {
        play = GameObject.Find("Player").GetComponent<PlayerController>();
        enemyRb = GetComponent<Rigidbody2D>();
        currentTarget = play.lastPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector3.Magnitude(currentTarget - transform.position) <= 0))
        {
            currentTarget = play.lastPosition();
        }
        Vector3 direction = currentTarget - transform.position;
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
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
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
            SceneManager.LoadScene("GameOver");
        }
    }
}
