using LAN.Animation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LAN.UI {
    public class CanvasLoading : CustomBehaviour {
        [SerializeField] private Transform panel, offsetPos, onsitePos;
        [SerializeField] private Transform logo;
        [SerializeField] private GameObject panelTooLong;
        [SerializeField] private Button forceStopBtn;

        private AnimManager anim;
        private bool isShow = false;
        private DateTime startTime;
        private float defaultTimeOut = 30;
        private float currentTimeOut = 30;

        private void Awake() {
            anim = panel.GetComponent<AnimManager>();
        }

        private void Start() {
            isShow = true;
            //panel.position = offsetPos.position;
            forceStopBtn.onClick.AddListener(() => {
                //panelTooLong.SetActive(false);
                LoadIsDone(true);
                Hide();
            });
            panelTooLong.SetActive(false);
        }

        private void Update() {
            if (DateTime.Now.Subtract(startTime).TotalSeconds >= currentTimeOut && !panelTooLong.activeInHierarchy) {
                panelTooLong.SetActive(true);
            }
        }

        public void SetTimeOutTemporary(float timeOut) {
            if (currentTimeOut < timeOut) {
                currentTimeOut = timeOut;
                Debug.LogWarning($"Set timeout to {currentTimeOut} seconds");
            }
        }

        public void ResetTimeOut() {
            currentTimeOut = defaultTimeOut;
            Debug.LogWarning($"Reset timeout to {currentTimeOut} seconds");
        }

        public void Show() {
            if (isShow) return;
            panelTooLong.SetActive(false);
            isShow = true;
            startTime = DateTime.Now;
            SetTimeOutTemporary(defaultTimeOut);
            StartCoroutine(Moving(onsitePos));
        }

        public void Hide() {
            if (!isShow) return;
            isShow = false;
            startTime = new DateTime();
            ResetTimeOut();
            StartCoroutine(Moving(offsetPos));
        }

        private IEnumerator Moving(Transform target) {
            //panel.gameObject.SetActive(true);
            yield return null;
            if (anim != null) {
                if (isShow) {
                    anim.SetScaleSetting(0f, .1f, () => OnAnimationDone(isShow), () => OnAnimationDone(false));
                    anim.StartScale(AnimationScaleType.ScaleUpY);
                } else {
                    anim.SetScaleSetting(.5f, .35f, null, () => OnAnimationDone(isShow));
                    anim.StartScale(AnimationScaleType.ScaleDownY);
                }
            }
        }

        private void OnAnimationDone(bool status) {
            //panel.gameObject.SetActive(isShow);
            if (logo.TryGetComponent(out ShakeAroundAnimation anim)) {
                anim.enabled = status;
            }
        }
}
}
