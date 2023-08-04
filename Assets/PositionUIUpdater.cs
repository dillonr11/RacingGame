using UnityEngine;
using UnityEngine.UI;

public class PositionUIUpdater : MonoBehaviour
{
    public PositionDetector[] positionDetectors; // Array of all PositionDetectors
    public Text positionText;

    private void Start()
    {
        if (positionDetectors == null || positionDetectors.Length == 0)
        {
            Debug.LogError("PositionDetectors not set for PositionUIUpdater!");
            return;
        }

        if (positionText == null)
        {
            Debug.LogError("UI Text reference not set for PositionUIUpdater!");
            return;
        }
    }

    private void Update()
    {
        if (positionDetectors != null && positionText != null)
        {
            UpdatePositionText();
        }
    }

    private void UpdatePositionText()
    {
        string positionTextString = "Racer Positions:\n";

        foreach (PositionDetector detector in positionDetectors)
        {
            
            for (int i = 1; i <= detector.NumberOfRacersToTrack; i++)
            {
                Transform racerAtPosition = detector.GetRacerAtPosition(i);
                if (racerAtPosition != null)
                {
                    positionTextString += "Position " + i + ": " + racerAtPosition.name + "\n";
                }
                else
                {
                    //
                }
            }
        }

        positionText.text = positionTextString;
    }
}
