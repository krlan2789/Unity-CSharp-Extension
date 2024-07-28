using System.Collections;
using UnityEngine;

namespace LAN.Animation {

    public class AutoRotate : MonoBehaviour {
        [SerializeField] private float speed;

        //// Update is called once per frame
        //private void FixedUpdate() {
        //    transform.Rotate(0, 0, speed * 100 * Time.fixedDeltaTime);
        //}

        private IEnumerator Start() {
            yield return null;

            while (true) {
                transform.Rotate(0, 0, speed * Application.targetFrameRate * Time.deltaTime);
                yield return new WaitForSeconds(.001f);
            }
        }

        private void OnDestroy() {
            StopCoroutine("Start");
        }

        private void OnDisable() {
            StopCoroutine("Start");
        }
    }
}