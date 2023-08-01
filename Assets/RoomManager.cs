using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject player;

    [Space]
    public Transform spawnPoint;

    [Space]
    public Transform spawnPoint2;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("connected to server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("we're in the lobby");

        PhotonNetwork.JoinOrCreateRoom("test", null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("we're in a room");
        if (PhotonNetwork.IsMasterClient) {
            // Load the prefab (assuming 'player' is your prefab GameObject)
            GameObject playerPrefab = player;

            // Create a new Quaternion representing a 90-degree rotation on the Y-axis
            Quaternion rotation = Quaternion.Euler(0f, -90f, 0f);

            // Apply the rotation to the prefab's transform
            playerPrefab.transform.rotation = rotation;
           
            GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, rotation);
            _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        }
        else
        {
            // Load the prefab (assuming 'player' is your prefab GameObject)
            GameObject playerPrefab = player;

            // Create a new Quaternion representing a 90-degree rotation on the Y-axis
            Quaternion rotation = Quaternion.Euler(0f, -90f, 0f);

            // Apply the rotation to the prefab's transform
            playerPrefab.transform.rotation = rotation;
            GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint2.position, rotation);
            _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        }
        
    }
}
