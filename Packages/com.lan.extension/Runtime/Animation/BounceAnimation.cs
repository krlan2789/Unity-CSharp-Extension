using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAN.Animation {
    public class BounceAnimation : MonoBehaviour {
        public float bounceHeight = 20f; // The height of the bounce
        public float bounceTime = 0.25f; // The time it takes to complete one bounce cycle
        public float delayTime = 0.5f; // The delay between each GameObject's bounce

        private List<GameObject> gameObjects; // A list of the GameObjects to bounce
        private int currentIndex = 0; // The index of the current GameObject being bounced
        private bool isBouncing = false; // Whether or not a GameObject is currently bouncing

        // Start is called before the first frame update
        void Start() {
            gameObjects = new List<GameObject>(gameObject.transform.childCount);
            foreach (Transform child in gameObject.transform) {
                gameObjects.Add(child.gameObject);
            }
            currentIndex = 0;
            isBouncing = false;
            StartCoroutine(Bounce());
        }

        // Update is called once per frame
        void Update() {
            // Nothing to do here
        }

        IEnumerator Bounce() {
            while (true) {
                if (!isBouncing) {
                    StartCoroutine(BounceOne(gameObjects[currentIndex]));
                    currentIndex = (currentIndex + 1) % gameObjects.Count;
                }
                yield return null;
            }
        }

        IEnumerator BounceOne(GameObject obj) {
            isBouncing = true;
            Vector3 startPos = obj.transform.position;
            Vector3 endPos = startPos + new Vector3(0, bounceHeight, 0);
            float startTime = Time.time;
            float endTime = startTime + bounceTime;

            while (Time.time < endTime) {
                float t = (Time.time - startTime) / bounceTime;
                obj.transform.position = Vector3.Lerp(startPos, endPos, Mathf.Sin(t * Mathf.PI));
                yield return null;
            }

            obj.transform.position = startPos;
            isBouncing = false;
            yield return new WaitForSeconds(delayTime);
        }

        //public float delayStart = 0;
        //public float bounceHeight = 1f;   // The height the object will bounce
        //public float bounceSpeed = 1f;    // The speed of the bounce
        //private Vector3 startingPos;      // The starting position of the object
        //private bool playAnim = false;

        //private IEnumerator Start() {
        //    startingPos = transform.position;
        //    yield return new WaitForSeconds(delayStart);
        //    playAnim = true;
        //}

        //private void FixedUpdate() {
        //    if (playAnim) {
        //        // Calculate the new y position based on the sine of the time
        //        float newY = startingPos.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;

        //        // Update the object's position
        //        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        //    }
        //}
    }
}
