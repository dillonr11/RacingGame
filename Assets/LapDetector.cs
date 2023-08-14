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

    public float[] lapTimes; // Array to store lap times
    public float lapStartTime; // Time when the current lap started


    private void Start()
    {
        lapTimes = new float[totalLaps];
    }

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
                // Record the lap time
                RecordLapTime();
                lapCountText.text = "Lap: " + currentLap + "/" + totalLaps;
                // Start the cooldown coroutine
                StartCoroutine(CooldownCoroutine());
            }
        }
    }

    private void RecordLapTime()
    {
        // end last lap, store it, begin next lap timer

        // Calculate the time taken for the current lap
        float lapTime = Time.time - lapStartTime;

        // Convert the time to integer seconds and store it in the array
        lapTimes[currentLap - 1] = Mathf.FloorToInt(lapTime);

        // Start the next lap
        lapStartTime = Time.time;
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
