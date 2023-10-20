using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GestureEvents : MonoBehaviour
{
    public static event Action<string> OnGestureRecognized; // Define a gesture recognition event

    // Method to raise the gesture recognition event
    public static void RaiseGestureRecognized(string gesture)
    {
        OnGestureRecognized?.Invoke(gesture); // Raise the gesture recognition event
    }
}

