using Photon.Pun;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class LapDetector : MonoBehaviourPunCallbacks
{
    public int totalLaps = 3; // The total number of laps required to end the game

    
    public int currentLap = 0; // The current lap count

    private bool canTrigger = true;

    public Text lapCountText;

    public GameManager gameManager;

    public double[] lapTimes = new double[3];

    public double lapStart = 0.0;


    private void Start()
    {
        
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
            if (currentLap > totalLaps)
            {

                // lap just finished
                double lapTime = (PhotonNetwork.Time) - lapStart;
                lapTimes[currentLap - 2] = lapTime;

                // Call a custom method to handle the game end and declare a winner
                GameEnd(other);
            }
            else
            {
                if (currentLap == 1)
                {
                    lapStart = PhotonNetwork.Time;
                }
                else
                {
                    // lap just finished
                    double lapTime = (PhotonNetwork.Time) - lapStart;
                    lapStart = (PhotonNetwork.Time);
                    lapTimes[currentLap-2] = lapTime;
                    
                }
                
                lapCountText.text = "Lap: " + currentLap + "/" + totalLaps;
                // Start the cooldown coroutine
                StartCoroutine(CooldownCoroutine());
            }
        }
    }


    // Custom method to handle game end when a player finishes all laps
    private void GameEnd(Collider other)
    {
        if (gameManager.winner == "")
        {
            // Get the player GameObject from the collider
            GameObject playerObject = other.gameObject;

            // Find the PhotonView component in the player GameObject's hierarchy
            PhotonView photonView = playerObject.GetComponentInParent<PhotonView>();

            this.photonView.RPC("winnerUpdate", RpcTarget.All, photonView.ViewID);
        }
        else
        {
            Debug.Log("You lost");
        }
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

    [PunRPC]
    public void winnerUpdate(int id)
    {
        gameManager.winner = id.ToString();
    }
}
