using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegOnHit : MonoBehaviour
{

    private bool isResetInProgress = false;
    private GameController gameController;
    public void Start()
    {
        GameObject gameControllerObject = GameObject.Find("GameController");

        if (gameControllerObject != null)
        {
            // Get the GameController script component from the GameObject
            gameController = gameControllerObject.GetComponent<GameController>();

            // Check if the GameController script component is found
            if (gameController == null)
            {
                Debug.LogError("GameController script not found on the GameController GameObject.");
            }
        }
        else
        {
            Debug.LogError("GameController GameObject not found in the scene.");
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collidedCollider = collision.collider;



        if (transform.parent.GetComponent<TestPlayerMovement>() != null)
        {
            TestPlayerMovement playerMovement = transform.parent.GetComponent<TestPlayerMovement>();
            if (collidedCollider.gameObject.CompareTag("sword"))
            {

                playerMovement.AddReward(-1f);
                Debug.Log("EnemyWaifu Reward: " + playerMovement.GetCumulativeReward());
                playerMovement.EntityDisable();

                if (!isResetInProgress)
                {
                    isResetInProgress = true;
                    Invoke("ResetMatch", 3f);
                    Invoke("EnableEntity", 3f);
                }
            }
        }

        else if (transform.parent.GetComponent<ImmediatePlayerMovement>() != null)
        {
            ImmediatePlayerMovement playerMovement = transform.parent.GetComponent<ImmediatePlayerMovement>();
            if (collidedCollider.gameObject.CompareTag("sword"))
            {
                playerMovement.AddReward(-1f);
                Debug.Log("PlayerWaifu Reward: " + playerMovement.GetCumulativeReward());
                playerMovement.EntityDisable();
                if (!isResetInProgress)
                {
                    isResetInProgress = true;
                    Invoke("ResetMatch", 3f);
                    Invoke("EnableEntity", 3f);
                }
            }
        }


        else
        {
            // Log a warning if the component is not found
            Debug.LogWarning("PlayerMovement component not found on the parent object.");
        }
    }


    private void ResetMatch()
    {
        gameController.ResetMatch();
        isResetInProgress = false;
    }

    private void RestartMatch()
    {
        gameController.RestartMatch();
        isResetInProgress = false;
    }

    private void EnableEntity()
    {

        if (transform.parent.GetComponent<TestPlayerMovement>() != null)
        {
            TestPlayerMovement playerMovement = transform.parent.GetComponent<TestPlayerMovement>();
            playerMovement.EntityEnable();
        }


        else if (transform.parent.GetComponent<ImmediatePlayerMovement>() != null)
        {
            ImmediatePlayerMovement playerMovement = transform.parent.GetComponent<ImmediatePlayerMovement>();
            playerMovement.EntityEnable();
        }

    }
}
