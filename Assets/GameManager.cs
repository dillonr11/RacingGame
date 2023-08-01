using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool startGame = false;

    public Text countdownText;

    private bool localStart = false;

    private int countdownValue = 0;
    
    private bool showCountdownText = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    [PunRPC]
    private void SetSomeVariable(bool value)
    {
        startGame = value;
    }

    // Call this method to change the variable and synchronize it across all players
    public void ChangeVariableForAllPlayers(bool newValue)
    {
        photonView.RPC("SetSomeVariable", RpcTarget.All, newValue);
    }

    // Call this method to synchronize the countdown value across all players
    [PunRPC]
    private void UpdateCountdownText(int value)
    {
        countdownText.text = value > 0 ? value.ToString() : "GO!";
    }

    // Start the countdown coroutine
    private IEnumerator StartCountdownCoroutine()
    {
        countdownValue = 5;
        showCountdownText = true; // Show the countdown text at the beginning
        while (countdownValue > 0)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            countdownValue--;
        }

        showCountdownText = false; // Hide the countdown text after the countdown is finished

        // Call the method to change the variable after the countdown
        ChangeVariableForAllPlayers(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the "Escape" key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Call the method to quit the game
            Application.Quit();

        }
        if (!localStart)
        {
            if (PhotonNetwork.InRoom)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient && startGame == false)
                {
                    localStart = true;
                    StartCoroutine(StartCountdownCoroutine());
                }
            }
        }

        // Update the countdown text based on the synchronized countdownValue
        countdownText.text = showCountdownText ? countdownValue.ToString() : "";
    }

    // This method is called when photonView is instantiated on a remote client
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Sending data to the network (only the master client will do this)
            stream.SendNext(countdownValue);
            stream.SendNext(showCountdownText);
        }
        else
        {
            // Receiving data from the network (other clients)
            countdownValue = (int)stream.ReceiveNext();
            showCountdownText = (bool)stream.ReceiveNext();
        }
    }
}
