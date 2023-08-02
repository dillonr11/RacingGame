using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LapDetector : MonoBehaviourPunCallbacks
{
    public int totalLaps = 3; // The total number of laps required to end the game

    
    public int currentLap = 0; // The current lap count

    private bool canTrigger = true;

    public Text lapCountText;

    // This method is called when a collider enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        // Get the player GameObject from the collider
        GameObject playerObject = other.gameObject;

        // Find the PhotonView component in the player GameObject's hierarchy
        PhotonView photonView = playerObject.GetComponentInParent<PhotonView>();
        
        // Check if the local player has entered the trigger area
        if (canTrigger && playerObject.CompareTag("Player") && photonView != null && photonView.IsMine)
        {
            currentLap++;
            if (currentLap >= totalLaps)
            {
                // Call a custom method to handle the game end and declare a winner
                GameEnd();
            }
            else
            {
                
                lapCountText.text = "Lap: " + currentLap + "/" + totalLaps;
                // Start the cooldown coroutine
                StartCoroutine(CooldownCoroutine());
            }
        }
    }

    // Custom method to handle game end when a player finishes all laps
    private void GameEnd()
    {
        // Add your code here to handle the end of the game
        // For example, you can show a winner UI, restart the race, etc.
        Debug.Log("game over");
    }

    private IEnumerator CooldownCoroutine()
    {
        // Set the flag to prevent further triggering during the cooldown
        canTrigger = false;

        // Wait for the cooldown period (e.g., 5 seconds)
        float cooldownDuration = 5f;
        yield return new WaitForSeconds(cooldownDuration);

        // Reset the flag to allow triggering again
        canTrigger = true;
    }
}
