using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpingForce = 40f;
    private bool facingRight;
    private bool jump;
    private bool doubleJump;
    private bool enterDoor;
    private Rigidbody2D playerRb;
    private LevelGen l;
    private float horizontalDirection;
    private Tilemap ground;
    //public BoxCollider2D playercoll;
    public Vector2 boxSize;
    public float castDistance;
    [SerializeField] private LayerMask grLayer; //used for Physics2D Raycasting
    [SerializeField] int maxJumps = 2;
    private int jumpsLeft;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        l = FindObjectOfType<LevelGen>();
        ground = l.GroundMap;
        jump = false;
        enterDoor = false;
        doubleJump = false;
        facingRight = false;
        jumpsLeft = maxJumps;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalDirection = Input.GetAxisRaw("Horizontal") * speed;
        jump = Input.GetKeyDown(KeyCode.Space);//; //this code will check to see if a player can jump
        //as we are checking if player is on ground
        playerRb.velocity = new Vector2(horizontalDirection, playerRb.velocity.y);
        if(isGrounded() && playerRb.velocity.y <= 0)
        {
            Debug.Log("Ground found");
            jumpsLeft = maxJumps;
        }
        if (jump && jumpsLeft > 0)
        {
            playerRb.velocity = new Vector2(horizontalDirection, jumpingForce);
            jumpsLeft -= 1;
        }
        if (horizontalDirection < 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalDirection > 0 && facingRight)
        {
            Flip();
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    private bool isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, grLayer)) 
        {
            Debug.Log("True return");
            return true;
        }
        else return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}
