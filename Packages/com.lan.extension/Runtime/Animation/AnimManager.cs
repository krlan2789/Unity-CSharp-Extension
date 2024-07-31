using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LAN.Animation {
    public enum AnimationSwingType {
        None = 0, BottomUp, TopDown, LeftToRight, RightToLeft
    }

    public enum AnimationOpacityType {
        None = 0, FadeIn, FadeOut, FadeInout, FadeOutIn
    }

    public enum AnimationScaleType {
        None = 0, ScaleDownY, ScaleUpY, ScaleDownAll, ScaleDownX, ScaleUpX
    }

    public enum AnimationNotifType {
        None = 0, Badge, PopUp
    }

    //[RequireComponent(typeof(CanvasGroup))]
    public class AnimManager : MonoBehaviour {

        #region Animation Settings
        [Tooltip("If empty, will use this object")]
        public GameObject target;

        [SerializeField]
        private bool removeComponentOnEnd = false;

        #region Swing Settings
        [SerializeField, BoxGroup("Animation Swing Settings")]
        public AnimationSwingType swingType;

        [SerializeField, Tooltip("Range values: 0f-1f and 1f-100f depend on given value (Max value is 100, if greater then 100 will reduce to 100)")]
        [BoxGroup("Animation Swing Settings"), HideIf("swingType", AnimationSwingType.None)]
        protected Vector3 swingTargetPosByPercent;

        [SerializeField, BoxGroup("Animation Swing Settings"), HideIf("swingType", AnimationSwingType.None), Min(0)]
        protected float swingDelay = 0f;

        [SerializeField, BoxGroup("Animation Swing Settings"), HideIf("swingType", AnimationSwingType.None), Min(0)]
        protected float swingDuration = 1.5f;

        [SerializeField, BoxGroup("Animation Swing Settings"), HideIf("swingType", AnimationSwingType.None)]
        protected bool swingAutoplay = false;

        [SerializeField, BoxGroup("Animation Swing Settings"), Tooltip("Set value to negative to disable")]
        [HideIf("swingType", AnimationSwingType.None), Range(-.001f, 1f)]
        protected float onWidth = -.001f;

        [SerializeField, BoxGroup("Animation Swing Settings"), Tooltip("Set value to negative to disable")]
        [HideIf("swingType", AnimationSwingType.None), Range(-.001f, 1f)]
        protected float onHeight = -.001f;

        protected Action actionAfterSwing;
        #endregion Swing Settings

        #region Opacity Settings
        [SerializeField, BoxGroup("Animation Opacity Settings"), InfoBox("CanvasGroup Component Required", EInfoBoxType.Warning)]
        protected AnimationOpacityType opacityType;

        [SerializeField, BoxGroup("Animation Opacity Settings"), HideIf("opacityType", AnimationOpacityType.None), Min(0)]
        protected float opacityDelay = 0f;

        [SerializeField, BoxGroup("Animation Opacity Settings"), HideIf("opacityType", AnimationOpacityType.None), Min(0)]
        protected float opacityDuration = 1f;

        [SerializeField, BoxGroup("Animation Opacity Settings"), HideIf("opacityType", AnimationOpacityType.None)]
        protected bool opacityAutoplay = false;

        protected Action actionAfterOpacity;
        #endregion Opacity Settings

        #region Scale Settings
        [SerializeField, BoxGroup("Animation Scale Settings"), InfoBox("CanvasGroup Component Required", EInfoBoxType.Warning)]
        protected AnimationScaleType scaleType;

        [SerializeField, BoxGroup("Animation Scale Settings"), HideIf("scaleType", AnimationScaleType.None), Min(0)]
        protected float scaleDelay = 0f;

        [SerializeField, BoxGroup("Animation Scale Settings"), HideIf("scaleType", AnimationScaleType.None), Min(0)]
        protected float scaleDuration = 1f;

        [SerializeField, BoxGroup("Animation Scale Settings"), HideIf("scaleType", AnimationScaleType.None)]
        protected bool scaleAutoplay = false;

        protected Action actionBeforeScale;
        protected Action actionAfterScale;
        #endregion Scale Settings

        private CanvasGroup targetImage;
        #endregion Animation Settings

        private void Awake() {
            if (target == null) target = gameObject;
            targetImage = target.GetComponent<CanvasGroup>();
            if (targetImage == null) targetImage = target.AddComponent<CanvasGroup>();
        }

        private void OnEnable() {
            Application.targetFrameRate = 60;
            Time.timeScale = 1;

            //  Opacity
            switch (opacityType) {
                case AnimationOpacityType.FadeIn:
                case AnimationOpacityType.FadeInout:
                    targetImage.alpha = 0;
                    break;

                case AnimationOpacityType.FadeOut:
                case AnimationOpacityType.FadeOutIn:
                    targetImage.alpha = 1;
                    break;
            }
            if (opacityAutoplay) StartVisibility();

            //  Swing
            if (swingAutoplay) StartMove();

            //  Scale
            if (scaleAutoplay) StartScale();
        }

        #region Delay
        private IEnumerator Delay(float duration, UnityAction Callback) {
            if (duration > 0) yield return new WaitForSeconds(duration);
            else yield return null;
            Callback();
        }
        #endregion Delay

        #region Animation Swing
        /// <summary>
        /// Set swing setting
        /// </summary>
        /// <param name="delay">Delay in seconds. If less than 0, will use previous value</param>
        /// <param name="duration">Scaling duration in seconds. If less than 0, will use previous value</param>
        /// <param name="swingTargetByPercent">Swing target position (in percent)</param>
        /// <param name="callback">Callback will called when animating is done</param>
        public void SetSwingSetting(float delay = -1, float duration = -1, Vector3 swingTargetByPercent = new Vector3(), Action callback = null) {
            if (delay >= 0) swingDelay = delay;
            if (duration >= 0) swingDuration = duration;
            if (swingTargetByPercent != new Vector3()) swingTargetPosByPercent = swingTargetByPercent;
            if (callback != null) actionAfterSwing = callback;
        }

        /// <summary>
        /// Starting to moving the gameobject
        /// </summary>
        /// <param name="type">Fill None for use last config</param>
        public void StartMove(AnimationSwingType type = AnimationSwingType.None) {
            if (type != AnimationSwingType.None) swingType = type;
            if (swingDelay > 0) StartCoroutine(Delay(swingDelay, () => Move()));
            else Move();
        }

        //  Move target object depend on animation type
        public Coroutine Move() {
            switch (swingType) {
                case AnimationSwingType.BottomUp:
                    //Debug.Log("BottomUp");
                    return StartCoroutine(Moving2());

                case AnimationSwingType.TopDown:
                    //Debug.Log("TopDown");
                    return StartCoroutine(Moving2());

                case AnimationSwingType.LeftToRight:
                    //Debug.Log("LeftToRight");
                    return StartCoroutine(Moving2());

                case AnimationSwingType.RightToLeft:
                    //Debug.Log("RightToLeft");
                    return StartCoroutine(Moving2());

                default:
                    return null;
            }
        }

        //  Moving gameobject to target destination
        private IEnumerator Moving(Vector3 destination) {
            float elapsedTime = 0;

            while (elapsedTime < swingDuration) {
                target.transform.localPosition = Vector3.Lerp(
                    target.transform.localPosition,
                    destination,
                    (elapsedTime / swingDuration)
                );

                if (Vector3.Distance(target.transform.localPosition, destination) < 1f) {
                    actionAfterSwing?.Invoke();
                    yield break;
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            RemoveComponent();
        }

        //  Moving gameobject to target destination version 2
        private IEnumerator Moving2() {
            Vector3 currentPos = target.transform.position;
            sbyte xPlusMinus = (sbyte)(swingTargetPosByPercent.x > 0 ? 1 : -1);
            sbyte yPlusMinus = (sbyte)(swingTargetPosByPercent.y > 0 ? 1 : -1);
            float percentX = GetPercentage(swingTargetPosByPercent.x);
            float percentY = GetPercentage(swingTargetPosByPercent.y);
            float targetX = (onWidth >= 0 ? onWidth : percentX) * Screen.width * xPlusMinus;
            float targetY = (onHeight >= 0 ? onHeight : percentY) * Screen.height * yPlusMinus;
            Vector3 targetPos = new Vector3(targetX, targetY, swingTargetPosByPercent.z);
            float diffPosX = (targetPos.x - currentPos.x) / (swingDuration * Application.targetFrameRate);
            float diffPosY = (targetPos.y - currentPos.y) / (swingDuration * Application.targetFrameRate);
            float diffPosZ = (targetPos.z - currentPos.z) / (swingDuration * Application.targetFrameRate);
            //Debug.Log("diffPosX: " + diffPosX + " | targetPos.x: " + targetPos.x + " | currentPos.x: " + currentPos.x);

            while (true) {
                target.transform.position = new Vector3(diffPosX + currentPos.x, diffPosY + currentPos.y, currentPos.z);
                currentPos = target.transform.position;

                if (Vector3.Distance(target.transform.position, new Vector3(targetX, targetY)) < 0.005f) {
                    actionAfterSwing?.Invoke();
                    break;
                }
                yield return null;
            }

            yield return null;
            RemoveComponent();
        }
        #endregion Animation Swing

        #region Animation Opacity
        public float Alpha {
            get {
                return targetImage.alpha;
            }
            set {
                targetImage.alpha = value;
            }
        }

        /// <summary>
        /// Set opacity setting
        /// </summary>
        /// <param name="delay">Delay in seconds. If less than 0, will use previous value</param>
        /// <param name="duration">Scaling duration in seconds. If less than 0, will use previous value</param>
        /// <param name="callback">Callback will called when animating is done</param>
        public void SetOpacitySetting(float delay = -1, float duration = -1, Action callback = null) {
            if (delay >= 0) opacityDelay = delay;
            if (duration >= 0) opacityDuration = duration;
            if (callback != null) actionAfterOpacity = callback;
        }

        /// <summary>
        /// Start opacity animation depend on opacity setting 
        /// </summary>
        /// <param name="type"></param>
        public void StartVisibility(AnimationOpacityType type = AnimationOpacityType.None) {
            if (type != AnimationOpacityType.None) opacityType = type;
            if (opacityDelay > 0) StartCoroutine(Delay(opacityDelay, Visibility));
            else Visibility();
        }

        //  Changing visibility depend on animation type
        private void Visibility() {
            switch (opacityType) {
                case AnimationOpacityType.FadeIn:
                    PlayAppear();
                    break;

                case AnimationOpacityType.FadeOut:
                    PlayDisappear();
                    break;

                case AnimationOpacityType.FadeInout:
                    break;

                case AnimationOpacityType.FadeOutIn:
                    break;

                default:
                    break;
            }
        }

        #region Animation Appear
        public Coroutine PlayAppear() {
            return StartCoroutine(PlayingAppear());
        }

        private IEnumerator PlayingAppear() {
            float currentDuration = 0;
            while (currentDuration <= opacityDuration) {
                currentDuration += Time.deltaTime;
                if (targetImage.alpha <= 1) {
                    if (currentDuration <= opacityDuration) {
                        targetImage.alpha = (currentDuration / opacityDuration);
                    }
                }
                yield return null;
            }
            actionAfterOpacity?.Invoke();
            yield return null;
            RemoveComponent();
        }
        #endregion Animation Appear

        #region Animation Disappear
        public Coroutine PlayDisappear() {
            return StartCoroutine(PlayingDisappear());
        }

        private IEnumerator PlayingDisappear() {
            float currentDuration = 0;
            while (currentDuration <= opacityDuration) {
                currentDuration += Time.deltaTime;
                if (targetImage.alpha >= 0) {
                    if (currentDuration <= opacityDuration) {
                        targetImage.alpha = 1 - (currentDuration / opacityDuration);
                    }
                }
                yield return null;
            }
            actionAfterOpacity?.Invoke();
            yield return null;
            RemoveComponent();
        }
        #endregion Animation Disappear
        #endregion Animation Opacity

        #region Animation Scale
        public Vector3 Size {
            get {
                return targetImage.transform.localScale;
            }
            set {
                targetImage.transform.localScale = value;
            }
        }

        public Vector2 Pivot {
            get {
                return targetImage.GetComponent<RectTransform>().pivot;
            }
            set {
                targetImage.GetComponent<RectTransform>().pivot = value;
            }
        }

        /// <summary>
        /// Set scale setting
        /// </summary>
        /// <param name="delay">Delay in seconds. If less than 0, will use previous value</param>
        /// <param name="duration">Scaling duration in seconds. If less than 0, will use previous value</param>
        /// <param name="callbackAfter">Callback will called when animating is done</param>
        public void SetScaleSetting(float delay = -1, float duration = -1, Action callbackAfter = null, Action callbackBefore = null) {
            if (delay >= 0) scaleDelay = delay;
            if (duration >= 0) scaleDuration = duration;
            if (callbackAfter != null) actionAfterScale = callbackAfter;
            if (callbackBefore != null) actionBeforeScale = callbackBefore;
        }

        /// <summary>
        /// Start scaling object depend on scale setting
        /// </summary>
        /// <param name="type"></param>
        public void StartScale(AnimationScaleType type = AnimationScaleType.None) {
            if (type != AnimationScaleType.None) scaleType = type;
            if (scaleDelay > 0) StartCoroutine(Delay(scaleDelay, Scale));
            else Scale();
        }

        //  Changing scale depend on animation type
        private void Scale() {
            switch (scaleType) {
                case AnimationScaleType.ScaleUpY:
                    PlayScaleUpY();
                    break;

                case AnimationScaleType.ScaleDownY:
                    PlayScaleDownY();
                    break;

                case AnimationScaleType.ScaleDownAll:
                    PlayScaleDownAll();
                    break;

                case AnimationScaleType.ScaleUpX:
                    PlayingScaleUpX();
                    break;
                    
                default:
                    break;
            }
        }

        #region Animation Scale Up Y
        /// <summary>
        /// Play animation scale up Y axis
        /// </summary>
        public Coroutine PlayScaleUpY() {
            actionBeforeScale?.Invoke();
            return StartCoroutine(PlayingScaleUpY());
        }

        private IEnumerator PlayingScaleUpY() {
            float currentDuration = 0;
            while (currentDuration <= scaleDuration) {
                currentDuration += Time.deltaTime;
                if (targetImage.transform.localScale.y <= 1) {
                    if (currentDuration <= scaleDuration) {
                        targetImage.transform.localScale = new Vector3(1, currentDuration / scaleDuration, 1);
                    }
                }
                yield return null;
            }

            targetImage.transform.localScale = Vector3.one;
            actionAfterScale?.Invoke();
            yield return null;
            RemoveComponent();
        }
        #endregion Animation Scale Up Y

        #region Animation Scale Down Y
        /// <summary>
        /// Play animation scale down Y axis
        /// </summary>
        public Coroutine PlayScaleDownY() {
            actionBeforeScale?.Invoke();
            return StartCoroutine(PlayingScaleDownY());
        }

        private IEnumerator PlayingScaleDownY() {
            try {
                float currentDuration = 0;
                while (currentDuration <= scaleDuration) {
                    currentDuration += Time.deltaTime;
                    if (targetImage.transform.localScale.y >= 0) {
                        if (currentDuration <= scaleDuration) {
                            targetImage.transform.localScale = new Vector3(1, 1 - (currentDuration / scaleDuration), 1);
                        }
                    }
                    //yield return null;
                }

                targetImage.transform.localScale = new Vector3(1, 0, 1);
            } catch (Exception e) {
                Debug.LogWarning(gameObject.name);
                Debug.LogWarning(e.Message);
            }

            actionAfterScale?.Invoke();
            yield return null;
            RemoveComponent();
        }
        #endregion Animation Scale Down Y

        #region Animation Scale Up X
        /// <summary>
        /// Play animation scale up Y axis
        /// </summary>
        public Coroutine PlayScaleUpX() {
            actionBeforeScale?.Invoke();
            return StartCoroutine(PlayingScaleUpX());
        }

        private IEnumerator PlayingScaleUpX() {
            float currentDuration = 0;
            while (currentDuration <= scaleDuration) {
                currentDuration += Time.deltaTime;
                if (targetImage.transform.localScale.x <= 1) {
                    if (currentDuration <= scaleDuration) {
                        targetImage.transform.localScale = new Vector3(currentDuration / scaleDuration, 1, 1);
                    }
                }
                yield return null;
            }

            targetImage.transform.localScale = Vector3.one;
            actionAfterScale?.Invoke();
            yield return null;
            RemoveComponent();
        }
        #endregion Animation Scale Up X

        #region Animation Scale Down All
        /// <summary>
        /// Play animation scale down all axis
        /// </summary>
        public Coroutine PlayScaleDownAll() {
            actionBeforeScale?.Invoke();
            return StartCoroutine(PlayingScaleDownAll());
        }

        private IEnumerator PlayingScaleDownAll() {
            float currentDuration = 0;
            while (currentDuration <= scaleDuration) {
                currentDuration += Time.deltaTime;
                if (targetImage.transform.localScale.y >= 0) {
                    if (currentDuration <= scaleDuration) {
                        float scale = 1 - (currentDuration / scaleDuration);
                        targetImage.transform.localScale = new Vector3(scale, scale, scale);
                    }
                }
                yield return null;
            }

            targetImage.transform.localScale = new Vector3(0, 0, 0);
            actionAfterScale?.Invoke();
            yield return null;
            RemoveComponent();
        }
        #endregion Animation Scale Down All
        #endregion Animation Scale

        private float GetPercentage(float value) {
            float percent = 0f;
            value = value > 0 ? value : value * -1;
            if (value > 100) {
                percent = 1f;
            } else if (value > 1 && value <= 100) {
                percent = value / 100;
            } else {
                percent = value;
            }
            return percent;
        }

        private void RemoveComponent() {
            if (removeComponentOnEnd) {
                Destroy(this);
            }
        }
    }
}