using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LapDetector : MonoBehaviour
{
    public int totalLaps = 3; // The total number of laps required to end the game
    public int currentLap = 0; // The current lap count

    private bool canTrigger = true;

    // This method is called when a collider enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (canTrigger && other.tag == "Player")
        {
            currentLap++;
            if (currentLap >= totalLaps)
            {
                Application.Quit();
            }
        }
        // Start the cooldown coroutine
        StartCoroutine(CooldownCoroutine());
    }


    private IEnumerator CooldownCoroutine()
    {
        // Set the flag to prevent further triggering during the cooldown
        canTrigger = false;

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Reset the flag to allow triggering again
        canTrigger = true;
    }
}
