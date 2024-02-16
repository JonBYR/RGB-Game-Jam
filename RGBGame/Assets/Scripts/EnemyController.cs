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
    public float nextWaypointDistance = 3f;
    int currentPoint = 0;
    bool endOfPath = false;
    private void Awake()
    {
        play = GameObject.Find("Player").GetComponent<PlayerController>();
        enemyRb = GetComponent<Rigidbody2D>();
        currentTarget = play.lastPosition(); //gets the last position the player was in before the enemy spawns
        //ai = GetComponent<AIDestinationSetter>();
        //ai.target = GameObject.Find("Player").transform;
        //seek = GetComponent<Seeker>();
        //InvokeRepeating("UpdatePath", 0f, 0.5f);
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if ((Vector3.Magnitude(currentTarget - transform.position) <= 0))
        {
            currentTarget = play.lastPosition(); //if the enemy is far from the player/player has not been reached then set a new target for the enemy
        }
        
        Vector3 direction = currentTarget - transform.position; //gets the direction the enemy needs to go
        direction.Normalize();
        flipDirection = direction.x;
        if(flipDirection < 0 && !facingRight)
        {
            Flip(); //flips sprite should sprite be facing the wrong direction
        }
        else if (flipDirection > 0 && facingRight)
        {
            Flip();
        }
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
        enemyRb.MovePosition((Vector2)transform.position + ((Vector2)direction * speed * Time.deltaTime)); //enemy moves towards player

    }
    /*
    private void FixedUpdate()
    {
        if (p == null) return;
        if (currentPoint >= p.vectorPath.Count)
        {
            endOfPath = true;
            return;
        }
        else endOfPath = false;
        Vector2 direct = ((Vector2)p.vectorPath[currentPoint] - enemyRb.position).normalized;
        Vector2 force = direct * speed * Time.deltaTime;
        enemyRb.AddForce(force);
        float distance = Vector2.Distance(enemyRb.position, p.vectorPath[currentPoint]);
        if (distance < nextWaypointDistance) currentPoint++;
    }
    */
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Calling this collision function");
        enemyRb.AddForce(new Vector2(0, 1f), ForceMode2D.Impulse);
    }
}
