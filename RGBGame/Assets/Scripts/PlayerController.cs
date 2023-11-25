using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpingForce = 40f;
    private bool facingRight;
    private bool jump;
    private bool onLadder;
    private bool isClimbing;
    public bool enterDoor;
    private Rigidbody2D playerRb;
    private LevelGen l;
    private float horizontalDirection;
    private float vertical;
    private Tilemap ground;
    //public BoxCollider2D playercoll;
    public Vector2 boxSize;
    public float castDistance;
    [SerializeField] private LayerMask grLayer; //used for Physics2D Raycasting
    [SerializeField] int maxJumps = 2;
    private int jumpsLeft;
    public static int amountOfGold = 0;
    public TextMeshProUGUI amountText;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        l = FindObjectOfType<LevelGen>();
        ground = l.GroundMap;
        jump = false;
        enterDoor = false;
        onLadder = false;
        facingRight = false;
        jumpsLeft = maxJumps;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalDirection = Input.GetAxisRaw("Horizontal") * speed;
        vertical = Input.GetAxis("Vertical");
        if (onLadder && Mathf.Abs(vertical) > 0f) isClimbing = true;
        else isClimbing = false;
        jump = Input.GetKeyDown(KeyCode.Space);//; //this code will check to see if a player can jump
        //as we are checking if player is on ground
        playerRb.velocity = new Vector2(horizontalDirection, playerRb.velocity.y);
        if(isGrounded() && playerRb.velocity.y <= 0)
        {
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
        amountText.text = "Current Gold " + amountOfGold.ToString();
    }
    private void FixedUpdate()
    {
        if (isClimbing)
        {
            playerRb.gravityScale = 0f;
            playerRb.velocity = new Vector2(playerRb.velocity.x, vertical * speed);
        }
        else playerRb.gravityScale = 1f;
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
            return true;
        }
        else return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            amountOfGold++;
            Debug.Log(amountOfGold);
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder"))
        {
            onLadder = true;
            Vector3Int ladderPos = ground.WorldToCell(transform.position);
            var test = ground.GetCellCenterWorld(ladderPos);
        }
        if(collision.tag == "Door")
        {
            if(enterDoor == false)
            {
                Debug.Log("EnterDoor");
                enterDoor = true;
                SceneManager.LoadScene("WinScene");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            onLadder = false;
        }
    }
    public Vector3 lastPosition()
    {
        return playerRb.transform.position;
    }
}
