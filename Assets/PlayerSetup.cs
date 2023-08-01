using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public PrometeoCarController movement;

    public GameObject camera;

    public void IsLocalPlayer()
    {
        movement.enabled = true;
        camera.SetActive(true);
    }
}

