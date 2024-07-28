using LAN;
using LAN.Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LAN.UI {
    public class CanvasPopup2 : CustomBehaviour {
        #region Static instance of this class
        private static CanvasPopup2 instance;
        public static CanvasPopup2 Instance {
            get {
                instance = instance != null ? instance : FindObjectOfType<CanvasPopup2>();
                if (instance == null) {
                    instance = Instantiate((GameObject)LoadResStatic("Prefabs/DontDestroy/CanvasAttendence")).GetComponent<CanvasPopup2>();
                }
                return instance;
            }
        }
        #endregion

        [SerializeField] private AnimManager animManager;
        [SerializeField] private Transform popup;
        [SerializeField] private Button hideBtn;

        private List<Transform> panels = new List<Transform>();
        private bool isAnimating = false;
        private bool preventToHide = false;

        public bool IsShow { get { return popup.gameObject.activeInHierarchy; } }

        private void Awake() {
            if (animManager == null) animManager = GetComponentInChildren<AnimManager>();
            if (popup == null) popup = transform.Find("Popup");

            foreach (Transform obj in popup) {
                panels.Add(obj);
            }

            Hide();
        }

        public void Show(int index, bool preventToHide = false) {
            this.preventToHide = preventToHide;
            for (int a = 0; a < panels.Count; a++) {
                panels[a].gameObject.SetActive(a == index);
            }
            StartCoroutine(Showing());
        }

        private IEnumerator Showing() {
            while (isAnimating) yield return null;
            isAnimating = true;
            if (popup != null) popup.gameObject.SetActive(true);
            if (hideBtn != null) {
                hideBtn.gameObject.SetActive(true);
                hideBtn.interactable = !preventToHide;
            }
            animManager.SetScaleSetting(0, .15f, () => {
                isAnimating = false;
            });
            animManager.StartScale(AnimationScaleType.ScaleUpY);
            yield return null;
        }

        public void Hide() {
            for (int a = 0; a < panels.Count; a++) {
                panels[a].gameObject.SetActive(false);
            }
            preventToHide = false;
            StartCoroutine(Hiding());
        }

        private IEnumerator Hiding() {
            while (isAnimating) yield return null;
            isAnimating = true;
            animManager.SetScaleSetting(0, .15f, () => {
                isAnimating = false;
                if (popup != null) popup.gameObject.SetActive(false);
                if (hideBtn != null) {
                    hideBtn.gameObject.SetActive(false);
                    hideBtn.interactable = !preventToHide;
                }
                //Debug.Log($"CanvaPopup2.Hiding: {isAnimating} | {popup != null} | {hideBtn != null}");
            });
            animManager.StartScale(AnimationScaleType.ScaleDownY);
            yield return null;
        }
    }
}
