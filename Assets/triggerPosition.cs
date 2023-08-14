using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerPosition : MonoBehaviourPun
{

    public bool localCanTrigger = true;

    public List<int> players = new List<int>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        if (playerObject.CompareTag("Player") && photonView != null)
        {
            if (localCanTrigger)
            {
                // need to add to list
                if (players.Count == 0)
                {
                    Debug.Log("if case getting called");
                    // you are in first
                    this.photonView.RPC("updateList", RpcTarget.All, photonView.ViewID, false);
                    if (photonView.IsMine)
                    {
                        localCanTrigger = false;
                        StartCoroutine(CooldownCoroutine());
                    }
                } else if (players.Count == 1)
                {
                    Debug.Log("else if case getting called");
                    // you are in second
                    this.photonView.RPC("updateList", RpcTarget.All, photonView.ViewID, false);
                    if (photonView.IsMine)
                    {
                        localCanTrigger = false;
                        StartCoroutine(CooldownCoroutine());
                    }
                }
                else
                {
                    Debug.Log(players.Count);
                    Debug.Log("else case getting called");
                    // full, need to empty and you are in first
                    this.photonView.RPC("updateList", RpcTarget.All, photonView.ViewID, true);
                    if (photonView.IsMine)
                    {
                        localCanTrigger = false;
                        StartCoroutine(CooldownCoroutine());
                    }
                }
            }
        }
    }

    private IEnumerator CooldownCoroutine()
    {
       
        // Wait for the cooldown period (e.g., 5 seconds)
        float cooldownDuration = 5f;
        yield return new WaitForSeconds(cooldownDuration);

        // Reset the flag to allow triggering again
        localCanTrigger = true;
    }

    [PunRPC]
    private void AddPlayerToList(int id)
    {
        if (!players.Contains(id))
        {
            players.Add(id);
        }
    }

    [PunRPC]
    private void updateList(int id, bool empty)
    {
        if (empty)
        {
            players.Clear();
        }

        // Now synchronize the changes over the network
        this.photonView.RPC("AddPlayerToList", RpcTarget.All, id);
    }
}
