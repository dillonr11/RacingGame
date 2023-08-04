using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class PositionDetector : MonoBehaviour
{
    private List<Transform> racers = new List<Transform>();
    private SortedDictionary<float, Transform> positionDictionary = new SortedDictionary<float, Transform>();

    public int NumberOfRacersToTrack = 2; // Change this to the number of racers you want to track

    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient && other.gameObject.CompareTag("Player"))
        {
            Transform racer = other.transform;
            if (!racers.Contains(racer))
            {
                racers.Add(racer);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (PhotonNetwork.IsMasterClient && other.gameObject.CompareTag("Player"))
        {
            Transform racer = other.transform;
            if (racers.Contains(racer))
            {
                racers.Remove(racer);
            }
        }
    }

    private void UpdateRacerPositions()
    {
        positionDictionary.Clear();

        foreach (Transform racer in racers)
        {
            float distanceToPosition = Vector3.Distance(transform.position, racer.position);
            positionDictionary.Add(distanceToPosition, racer);
        }
    }

    public Transform GetRacerAtPosition(int position)
    {
        if (position < 1 || position > NumberOfRacersToTrack)
        {
            return null;
        }

        UpdateRacerPositions();

        int index = position - 1;
        if (index >= positionDictionary.Count)
        {
            return null;
        }

        return positionDictionary.Values.ToArray()[index];
    }
}
