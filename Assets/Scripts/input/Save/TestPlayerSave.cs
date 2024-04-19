using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovement2 : MonoBehaviour
{
    public float normalMoveSpeed = 5f;
    public float fastMoveSpeed = 10f;
    private float moveSpeed;
    private float horizontal;
    private bool isFacingRight = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public List<Sprite> SwordSpriteList;
    public List<Sprite> NarmSpriteList;

    private Animator animator;

    public AnimationClip neutralAttack;

    public GameObject NarmObject;
    public GameObject SwordObject;

    private KeyCode SwordUpKey = KeyCode.UpArrow;
    private KeyCode SwordDownKey = KeyCode.DownArrow;
    private KeyCode leftShiftKey = KeyCode.LeftShift;
    private KeyCode rightShiftKey = KeyCode.RightShift;
    private KeyCode LeftKey = KeyCode.LeftArrow;
    private KeyCode RightKey = KeyCode.RightArrow;
    private KeyCode AttackKey = KeyCode.Keypad0;

    //0.155
    //-0.555
    //-1.797

    public enum SwordDirection
    {
        Down,
        Neutral,
        Up
    }

    private SwordDirection playerSwordDirection = SwordDirection.Neutral;

    // Additional variable for managing sprites
    private int SpriteManagerInteger = 1;

    private SpriteRenderer NarmSpriteRenderer;
    private SpriteRenderer SwordSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        moveSpeed = normalMoveSpeed;
        

        NarmSpriteRenderer = NarmObject.GetComponentInChildren<SpriteRenderer>();
        SwordSpriteRenderer = SwordObject.GetComponentInChildren<SpriteRenderer>();

        Flip();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdatePlayerSprite();

        if (Input.GetKeyDown(AttackKey))
        {
            animator.SetTrigger("Attack");
            animator.SetBool("BoolAttacking", true);
            StartCoroutine(WaitForAnimationToFinish());
        }
    }

    private void FixedUpdate()
    {
        // Only move horizontally when "a" or "d" keys are pressed
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }

    void HandleInput()
    {
        // Handle sword direction switching and SpriteManagerInteger
        bool isAttacking = animator.GetBool("BoolAttacking");

        if (!isAttacking)
        {
            if (Input.GetKeyDown(SwordUpKey))
            {
                if (playerSwordDirection == SwordDirection.Down)
                {
                    playerSwordDirection = SwordDirection.Neutral;
                    SwordObject.transform.position += new Vector3(0f, 0.27f, 0f);
                    SpriteManagerInteger += 1;
                    animator.SetTrigger("+up");
                }
                else if (playerSwordDirection == SwordDirection.Neutral)
                {
                    playerSwordDirection = SwordDirection.Up;
                    SwordObject.transform.position += new Vector3(0f, 0.21f, 0f);
                    SpriteManagerInteger += 1;
                    animator.SetTrigger("+up");
                }
            }
            else if (Input.GetKeyDown(SwordDownKey))
            {
                if (playerSwordDirection == SwordDirection.Up)
                {
                    playerSwordDirection = SwordDirection.Neutral;
                    SwordObject.transform.position += new Vector3(0f, -0.21f, 0f);
                    SpriteManagerInteger -= 1;
                    animator.SetTrigger("-down");
                }
                else if (playerSwordDirection == SwordDirection.Neutral)
                {
                    playerSwordDirection = SwordDirection.Down;
                    SwordObject.transform.position += new Vector3(0f, -0.27f, 0f);
                    SpriteManagerInteger -= 1;
                    animator.SetTrigger("-down");
                }
            }
        }

        // Handle movement speed only when "a" or "d" keys are pressed
        if (Input.GetKey(leftShiftKey) || Input.GetKey(rightShiftKey))
        {
            moveSpeed = fastMoveSpeed;
        }
        else
        {
            moveSpeed = normalMoveSpeed;
        }

        // Handle horizontal movement only when "a" or "d" keys are pressed
        horizontal = Input.GetKey(LeftKey) ? -1f : Input.GetKey(RightKey) ? 1f : 0f;
    }

    IEnumerator WaitForAnimationToFinish()
    {
        // Wait for the end of the current frame
        yield return null;

        // Get the length of the animation
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // Wait for the animation to finish
        yield return new WaitForSeconds(animationLength);

        // Set BoolAttacking to false after the animation is done
        animator.SetBool("BoolAttacking", false);
    }

    void Flip()
    {
       
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        
    }

    void UpdatePlayerSprite()
    {
        // Ensure SpriteManagerInteger is within the valid range
        SpriteManagerInteger = Mathf.Clamp(SpriteManagerInteger, 0, NarmSpriteList.Count - 1);

        // Update the player sprite based on SpriteManagerInteger for NarmObject
        if (NarmSpriteList.Count > 0)
        {
            NarmSpriteRenderer.sprite = NarmSpriteList[SpriteManagerInteger];
        }

        // Ensure SpriteManagerInteger is within the valid range for SwordObject
        SpriteManagerInteger = Mathf.Clamp(SpriteManagerInteger, 0, SwordSpriteList.Count - 1);

        // Update the player sprite based on SpriteManagerInteger for SwordObject
        if (SwordSpriteList.Count > 0)
        {
            SwordSpriteRenderer.sprite = SwordSpriteList[SpriteManagerInteger];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collidedCollider = collision.collider;
        if (collision.collider.gameObject.CompareTag("Ground"))
        {

        }

        else 
        {
            Debug.Log("TEST 1: " + collidedCollider.gameObject.tag);
            
        }
        
    }

    private bool EntityAlive = true;
    public void EntityDisable()
    {
        
        EntityAlive = false;
        SwordObject.SetActive(false);
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaa " + gameObject.name);
    }
}
