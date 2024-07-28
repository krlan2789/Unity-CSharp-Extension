using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LAN.Animation {
    public class SlowDownMoveAnimation : MonoBehaviour {
        public bool autoStart = true;
        public float delayToStart = 0;
        public float maxSpeed = 5.0f; // the maximum speed at which to move the object
        public float minSpeed = 1.0f; // the minimum speed at which to move the object
        [Range(0, 100)] public float decelerationDistancePercentage = 20f; // the distance from the target position at which the object should start decelerating
        public float smoothTime = 0.3f; // the amount of time it should take to reach the target speed
        public Transform targetPosition; // the position to move the object to

        public UnityAction onAnimationDone;

        private bool isMoving = false;
        private float currentSpeed;
        private float velocity = 0.0f;
        private float startDistance = 0;
        private Vector3 startPosition;

        public bool IsStartingPosition { get; private set; }

        private IEnumerator Start() {
            if (!autoStart) yield break;
            startPosition = transform.position;
            startDistance = Vector3.Distance(transform.position, targetPosition.position);
            yield return new WaitForSeconds(delayToStart);
            isMoving = true;
        }

        private void Update() {
            if (isMoving) {
                float distance = Vector3.Distance(transform.position, targetPosition.position); // calculate distance to target position
                if (distance > startDistance * decelerationDistancePercentage / 100) // if the object is far from the target position, move at max speed
                {
                    currentSpeed = maxSpeed;
                } else // if the object is close to the target position, gradually slow down
                  {
                    float targetSpeed = Mathf.Lerp(minSpeed, maxSpeed, Mathf.Pow(distance / (startDistance * decelerationDistancePercentage / 100), 2));
                    currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref velocity, smoothTime);
                }

                float step = currentSpeed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, step); // move the object towards the target position

                if (transform.position == targetPosition.position) {
                    onAnimationDone?.Invoke();
                    isMoving = false; // stop moving when the object has reached the target position
                }
            }
        }

        public void SetSetting(float delay = -1, float _minSpeed = -1, float _maxSpeed = -1) {
            delayToStart = delay;
            minSpeed = _minSpeed;
            maxSpeed = _maxSpeed;
        }

        public void Play(Transform targetPos = null) {
            if (targetPos != null) targetPosition = targetPos;
            startDistance = Vector3.Distance(transform.position, targetPosition.position);
            isMoving = true;
            IsStartingPosition = false;
        }

        public void PlayRevert() {
            targetPosition.position = startPosition;
            startDistance = Vector3.Distance(transform.position, targetPosition.position);
            isMoving = true;
            IsStartingPosition = true;
        }

        public void PlayToProvious() {
            if (IsStartingPosition) Play();
            else PlayRevert();
        }
    }
}
