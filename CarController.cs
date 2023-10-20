using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private GestureManager gestureManager;
    private string recognizedGesture = "";

    [Header("Wheel collider")]
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider backLeftWheelCollider;
    public WheelCollider backRightWheelCollider;

    [Header("Wheel Transform")]
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform backLeftWheelTransform;
    public Transform backRightWheelTransform;

    [Header("Car Engine")]
    public float accelerationForce = 2000f;
    public float reverseForce = 1000f;
    public float wheelsTorque = 35f;
    public float turnSpeed = 100f;
    public float breakingForce = 3000f;
    private const float accelerationSpeed = 10f;


    [Header("Car Sounds")]
    public AudioSource audioSource;
    public AudioClip accelerationSound;
    public AudioClip slowAccelerationSound;
    public AudioClip stopSound;

    [Header("Gesture recognition parameters")]
    public float noGestureTimeout = 2.0f;
    private float noGestureTimer = 0f;

    private float targetAcceleration = 0f;
    private float targetTurnAngle = 0f;
    private float presentAcceleration = 0f;
    private float presentTurnAngle = 0f;


    private bool isMovingVertical = false;
    private bool isMovingHorizontal = false;

    private void Start()
    {
        gestureManager = GestureManager.Instance;

        if (gestureManager == null)
        {
            Debug.LogError("GestureManager is not properly initialized.");
        }
       
    }

        private void Update()
        {
            recognizedGesture = gestureManager.GetRecognizedGesture();

            MoveCar();


           UpdateCarBasedOnGesture(recognizedGesture);

            
            ApplyBreaks();
        }

        private void MoveCar()
        {

            isMovingVertical = recognizedGesture.Contains("Thumb Up") || recognizedGesture.Contains("Pinky Finger Up");
            isMovingHorizontal = recognizedGesture.Contains("Index Finger Up") || recognizedGesture.Contains("Middle Finger Up");
            // Use the recognizedGesture variable to control the car based on gestures
               if (isMovingVertical)
               {
                   if (recognizedGesture.Contains("Thumb Up"))
                   {
                       targetAcceleration = accelerationForce;
                       audioSource.PlayOneShot(accelerationSound, 0.2f);
                   }
                   else if (recognizedGesture.Contains("Pinky Finger Up"))
                   {
                        targetAcceleration = -reverseForce;

                       audioSource.PlayOneShot(accelerationSound, 0.2f);
                       Debug.Log("Recognized Gesture: " + recognizedGesture);
                   }
                   else
                   {
                       // No gesture detected for vertical movement, release acceleration
                       targetAcceleration = 0f;
                   }

               }
            

            if (isMovingHorizontal)
            {
                if (recognizedGesture.Contains("Index Finger Up"))
                {
                    // Turn the car left
                    targetTurnAngle = -wheelsTorque;
                    audioSource.PlayOneShot(accelerationSound, 0.2f);
                }
                else if (recognizedGesture.Contains("Middle Finger Up"))
                {
                    // Turn the car right
                    targetTurnAngle = wheelsTorque;
                    audioSource.PlayOneShot(accelerationSound, 0.2f);
                }
            }
            else
            {
                // No gesture for horizontal movement, reset turn angle
                targetTurnAngle = 0f;
            }


             // Fallback mechanism: If no gesture is detected for a certain period, reset the car
            if (!string.IsNullOrEmpty(recognizedGesture))
            {
              noGestureTimer = 0f; // Reset the timer
            }
            else
            {
              noGestureTimer += Time.deltaTime;

              if (noGestureTimer >= noGestureTimeout)
              {
                ResetCarToDefaultState();
              }
            }

           // Smoothly adjust the presentAcceleration
            presentAcceleration = Mathf.Lerp(presentAcceleration, targetAcceleration, Time.deltaTime * accelerationSpeed);

             // Smoothly adjust the presentTurnAngle
             presentTurnAngle = Mathf.Lerp(presentTurnAngle, targetTurnAngle, Time.deltaTime * turnSpeed);

              // Apply the calculated acceleration and steering
              frontLeftWheelCollider.motorTorque = presentAcceleration;
              frontRightWheelCollider.motorTorque = presentAcceleration;

               // Set the steering angle for turning
              frontLeftWheelCollider.steerAngle = presentTurnAngle;
              frontRightWheelCollider.steerAngle = presentTurnAngle;

            SteeringWheels(frontLeftWheelCollider, frontLeftWheelTransform);
            SteeringWheels(frontRightWheelCollider, frontRightWheelTransform);
            SteeringWheels(backLeftWheelCollider, backLeftWheelTransform);
            SteeringWheels(backRightWheelCollider, backRightWheelTransform);

        }

        private void SteeringWheels(WheelCollider WC, Transform WT)
        {
            Vector3 position;
            Quaternion rotation;

            WC.GetWorldPose(out position, out rotation);

            WT.position = position;
            WT.rotation = rotation;
        }

        private void ApplyBreaks()
        {
            // Use the recognizedGesture variable to control brakes
            if (recognizedGesture.Contains("Ring Finger Up"))
            {
                // Apply brakes
                frontLeftWheelCollider.brakeTorque = breakingForce;
                frontRightWheelCollider.brakeTorque = breakingForce;
                backLeftWheelCollider.brakeTorque = breakingForce;
                backRightWheelCollider.brakeTorque = breakingForce;
            }
            else
            {
                // No gesture detected or other gestures, release brakes
                frontLeftWheelCollider.brakeTorque = 0f;
                frontRightWheelCollider.brakeTorque = 0f;
                backLeftWheelCollider.brakeTorque = 0f;
                backRightWheelCollider.brakeTorque = 0f;
            }
        }

        private void UpdateCarBasedOnGesture(string gesture)
        {
            // Store the recognized gesture
            recognizedGesture = gesture;
        }


        private void ResetCarToDefaultState()
        {
            targetAcceleration = 0f;
            targetTurnAngle = 0f;
            presentAcceleration = 0f;
            presentTurnAngle = 0f;
            
        }

    }