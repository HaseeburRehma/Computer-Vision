using UnityEngine;
using System;

public class GestureManager : MonoBehaviour
{
    public static GestureManager Instance { get; private set; }
    private string recognizedGesture = ""; // Store the recognized gesture

    public event Action<string> OnGestureReceived;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetRecognizedGesture(string gesture)
    {
        recognizedGesture = gesture;

        // Trigger the event to notify other scripts of the new gesture
        OnGestureReceived?.Invoke(gesture);
    }

    public string GetRecognizedGesture()
    {
        return recognizedGesture;
    }
}