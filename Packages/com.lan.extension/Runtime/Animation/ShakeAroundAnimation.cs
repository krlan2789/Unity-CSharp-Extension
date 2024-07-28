using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAN.Animation {
    public class ShakeAroundAnimation : MonoBehaviour {
        // Variables for controlling the shake intensity and speed
        public float shakeIntensity = 0.05f;
        public float shakeSpeed = 1.0f;
        public float moveDelay = 0.1f;

        // The starting position of the object
        private Vector3 initialPosition;

        // The current position offset of the object
        private Vector3 currentPositionOffset;

        // The target position offset of the object
        private Vector3 targetPositionOffset;

        // The current rotation offset of the object
        private Quaternion currentRotationOffset;

        // The target rotation offset of the object
        private Quaternion targetRotationOffset;

        // The timer for the delay between moves
        private float moveTimer = 0f;

        private void Start() {
            // Store the initial position of the object
            initialPosition = transform.position;

            // Set the initial position and rotation offsets to zero
            currentPositionOffset = Vector3.zero;
            currentRotationOffset = Quaternion.identity;

            // Set the target position and rotation offsets to zero
            targetPositionOffset = Vector3.zero;
            targetRotationOffset = Quaternion.identity;
        }

        private void Update() {
            // Increment the move timer
            moveTimer += Time.deltaTime;

            // If the move timer has elapsed, calculate a new target offset and reset the move timer
            if (moveTimer >= moveDelay) {
                // Calculate a random position offset for the object
                Vector3 randomOffset = new Vector3(
                    Random.Range(-1f, 1f) * shakeIntensity,
                    Random.Range(-1f, 1f) * shakeIntensity,
                    Random.Range(-1f, 1f) * shakeIntensity
                );

                // Calculate a rotation offset for the object
                Quaternion randomRotation = Quaternion.Euler(
                    0f,
                    0f,
                    Random.Range(-1f, 1f) * shakeIntensity * 50f
                );

                // Set the target position and rotation offsets based on the random offsets
                targetPositionOffset = randomOffset;
                targetRotationOffset = randomRotation;

                // Reset the move timer
                moveTimer = 0f;
            }

            // Smoothly interpolate the current position and rotation offsets towards the target offsets
            currentPositionOffset = Vector3.Lerp(currentPositionOffset, targetPositionOffset, Time.deltaTime * shakeSpeed);
            currentRotationOffset = Quaternion.Lerp(currentRotationOffset, targetRotationOffset, Time.deltaTime * shakeSpeed);

            // Apply the position and rotation offset to the object
            transform.position = initialPosition + currentPositionOffset;
            transform.rotation = currentRotationOffset;
        }
    }
}
