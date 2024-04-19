using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collidedCollider = collision.collider;

        if(!collidedCollider.gameObject.CompareTag("sword"))
        {
            if (transform.parent.GetComponent<TestPlayerMovement>() != null)
            {
                TestPlayerMovement playerMovement = transform.parent.GetComponent<TestPlayerMovement>();
                playerMovement.AddReward(1f);
                Debug.Log("EnemyWaifu Reward: " + playerMovement.GetCumulativeReward());
                /////////////////////////////////////////
            }

            else if (transform.parent.GetComponent<ImmediatePlayerMovement>() != null)
            {
                ImmediatePlayerMovement playerMovement = transform.parent.GetComponent<ImmediatePlayerMovement>();
                playerMovement.AddReward(1f);
                Debug.Log("PlayerWaifu Reward: " + playerMovement.GetCumulativeReward());

            }
            else
            {
                // Log a warning if the component is not found
                Debug.LogWarning("PlayerMovement component not found on the parent object.");
            }
        }
            


    }
}
