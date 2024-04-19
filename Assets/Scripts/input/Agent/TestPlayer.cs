using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Analytics;

public class TestPlayerMovement : Agent
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

    GameObject gameControllerObject;
    GameController gameController;
    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        moveSpeed = normalMoveSpeed;


        NarmSpriteRenderer = NarmObject.GetComponentInChildren<SpriteRenderer>();
        SwordSpriteRenderer = SwordObject.GetComponentInChildren<SpriteRenderer>();

        Flip();
        gameControllerObject = GameObject.Find("GameController");

        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
            if (gameController != null){}
            else{Debug.LogError("GameController component not found on GameController object.");}
        }
        else{Debug.LogError("GameController object not found in the scene.");}
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveAction = actions.ContinuousActions[0];

        // Clamp the moveAction value to ensure it's within -1 to 1 range
        horizontal = Mathf.Clamp(moveAction, -1f, 1f);

        float swordAction_Up = actions.DiscreteActions[0];
        float swordAction_Down = actions.DiscreteActions[1];
        float moveAction_Attack = actions.DiscreteActions[2];

        bool isAttacking = animator.GetBool("BoolAttacking");

        if (!isAttacking)
        {
            // Handle movement
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }

        if (!isAttacking)
        {
            // Handle sword actions
            if (swordAction_Up == 1)
            {
                // Handle sword up action
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

            if (swordAction_Down == 1)
            {
                // Handle sword down action
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

            // Handle attack action
            if (moveAction_Attack == 1)
            {
                animator.SetTrigger("Attack");
                animator.SetBool("BoolAttacking", true);
                StartCoroutine(WaitForAnimationToFinish());
            }
        }

    }

    public override void OnEpisodeBegin()
    {
    }


    private bool prevSwordUpPressed = false;
    private bool prevSwordDownPressed = false;
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        var discreteActionsOut = actionsOut.DiscreteActions;

        bool isAttacking = animator.GetBool("BoolAttacking");

        if (!isAttacking)
        {
            // Handle movement
            horizontal = Input.GetKey(LeftKey) ? -1f : Input.GetKey(RightKey) ? 1f : 0f;
        }

        // Assign horizontal movement to the continuous action
        continuousActionsOut[0] = horizontal;

        // Handle sword actions
        bool swordUpPressed = Input.GetKey(SwordUpKey);
        bool swordDownPressed = Input.GetKey(SwordDownKey);

        if (swordUpPressed && !prevSwordUpPressed)
        {
            discreteActionsOut[0] = 1;
            prevSwordUpPressed = true;
        }
        else if (!swordUpPressed)
        {
            prevSwordUpPressed = false;
        }

        if (swordDownPressed && !prevSwordDownPressed)
        {
            discreteActionsOut[1] = 1;
            prevSwordDownPressed = true;
        }
        else if (!swordDownPressed)
        {
            prevSwordDownPressed = false;
        }

        // Handle attack action
        if (Input.GetKey(AttackKey))
        {
            discreteActionsOut[2] = 1;
        }


        // Update player sprite
        UpdatePlayerSprite();
    }

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

    private void FixedUpdate()
    {
        RequestDecision();
    }
    


    void HandleInput()
    {
        // Handle sword direction switching and SpriteManagerInteger
        bool isAttacking = animator.GetBool("BoolAttacking");

        if (!isAttacking)
        {
            if (Input.GetKey(SwordUpKey))
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
            else if (Input.GetKey(SwordDownKey))
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
        
    }

    private bool EntityAlive = true;
    public void EntityDisable()
    {
        
        EntityAlive = false;
        SwordObject.SetActive(false);
    }

    public void EntityEnable()
    {

        EntityAlive = true;
        SwordObject.SetActive(true);
    }
}
