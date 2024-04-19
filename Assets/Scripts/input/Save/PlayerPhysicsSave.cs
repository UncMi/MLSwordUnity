using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class ImmediatePlayerMovementAgent : Agent
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

    private KeyCode upKey = KeyCode.W;
    private KeyCode downKey = KeyCode.S;
    private KeyCode leftShiftKey = KeyCode.LeftShift;
    private KeyCode rightShiftKey = KeyCode.RightShift;
    private KeyCode aKey = KeyCode.A;
    private KeyCode dKey = KeyCode.D;
    private KeyCode attackKey = KeyCode.E;

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
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        //UpdatePlayerSprite();

        if (Input.GetKeyDown(attackKey))
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

    Vector3 SwordLocation = new Vector3(9999f, -9999f, 9999f);
void HandleInput()
    {
        // Handle sword direction switching and SpriteManagerInteger
        bool isAttacking = animator.GetBool("BoolAttacking");

        if (SwordLocation == new Vector3(9999f, -9999f, 9999f))
        {
            SwordObject.transform.position = SwordLocation;
        }

        if (SwordObject != null)
        {
            SwordLocation = SwordObject.transform.position;
        }


        if (!isAttacking)
        {
            if (Input.GetKeyDown(upKey))
            {
                if (playerSwordDirection == SwordDirection.Down)
                {
                    playerSwordDirection = SwordDirection.Neutral;

                    SwordLocation += new Vector3(0f, 0.27f, 0f);
                    if(SwordObject != null)
                    {
                        SwordObject.transform.position = SwordLocation;
                    }

                    SpriteManagerInteger += 1;
                    animator.SetTrigger("+up");
                }
                else if (playerSwordDirection == SwordDirection.Neutral)
                {
                    playerSwordDirection = SwordDirection.Up;
                    SwordLocation += new Vector3(0f, 0.21f, 0f);
                    if (SwordObject != null)
                    {
                        SwordObject.transform.position = SwordLocation;
                    }
                    SpriteManagerInteger += 1;
                    animator.SetTrigger("+up");
                }
            }
            else if (Input.GetKeyDown(downKey))
            {
                if (playerSwordDirection == SwordDirection.Up)
                {
                    playerSwordDirection = SwordDirection.Neutral;
                    SwordLocation += new Vector3(0f, -0.21f, 0f);
                    if (SwordObject != null)
                    {
                        SwordObject.transform.position = SwordLocation;
                    }
                    SpriteManagerInteger -= 1;
                    animator.SetTrigger("-down");
                }
                else if (playerSwordDirection == SwordDirection.Neutral)
                {
                    playerSwordDirection = SwordDirection.Down;
                    SwordLocation += new Vector3(0f, -0.27f, 0f);
                    if (SwordObject != null)
                    {
                        SwordObject.transform.position = SwordLocation;
                    }
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
        horizontal = Input.GetKey(aKey) ? -1f : Input.GetKey(dKey) ? 1f : 0f;
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
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool EntityAlive = true;
    public void EntityDisable()
    {

        EntityAlive = false;
        SwordObject.SetActive(false);
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaa " + gameObject.name);
    }


    void UpdatePlayerSprite()
    {
        SpriteManagerInteger = Mathf.Clamp(SpriteManagerInteger, 0, NarmSpriteList.Count - 1);

        if (NarmSpriteList.Count > 0)
        {
            NarmSpriteRenderer.sprite = NarmSpriteList[SpriteManagerInteger];
        }

        SpriteManagerInteger = Mathf.Clamp(SpriteManagerInteger, 0, SwordSpriteList.Count - 1);

    }


    
}
