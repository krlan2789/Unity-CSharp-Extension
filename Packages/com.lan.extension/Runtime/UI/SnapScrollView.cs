using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LAN.UI {

    public class SnapScrollView : MonoBehaviour, IDragHandler, IEndDragHandler {
        [SerializeField] private Transform panelSnap, snapPoint;
        private List<Transform> children = new List<Transform>();
        private float[] childrenDistance;
        private bool initialized = false;

        //  For items value
        private float parentHeight = 0, childWidth = 0, childHeight = 0, panelSnapStartPosX = 0, childSpacing = 0, childrenGap = 0;

        private int nearestChild = 0;

        //  Auto slide
        [SerializeField] private bool autoSlide = false;

        [SerializeField] private float countDown = 0f, slideSpeed = 20f;
        private float currentCountDown;

        private void Awake() {
            panelSnap = transform.Find("PanelSnap");
            snapPoint = transform.Find("SnapPoint");
            parentHeight = transform.GetComponent<RectTransform>().sizeDelta.y;
            //Init();
        }

        private void Update() {
            if (autoSlide && initialized) {
                float steps = slideSpeed * Time.unscaledDeltaTime;

                if (currentCountDown <= 0) {
                    children = children.Where(x => x != null).ToList();

                    float newX = panelSnapStartPosX - (childrenGap * nearestChild);
                    Vector2 newPos = Vector2.Lerp(panelSnap.GetComponent<RectTransform>().anchoredPosition, new Vector2(newX, 0), steps);
                    panelSnap.GetComponent<RectTransform>().anchoredPosition = newPos;

                    if (Vector2.Distance(panelSnap.GetComponent<RectTransform>().anchoredPosition, new Vector2(newX, panelSnap.GetComponent<RectTransform>().anchoredPosition.y)) < .02f) {
                        if (nearestChild < children.Count - 1) nearestChild++;
                        else nearestChild = 0;

                        currentCountDown = countDown;
                    }
                } else {
                    currentCountDown -= Time.deltaTime;
                }

                panelSnap.GetComponent<RectTransform>().sizeDelta = new Vector2(panelSnap.GetComponent<RectTransform>().sizeDelta.x, parentHeight);
            }
        }

        private void OnDisable() {
            ResetPanelSnap();
        }

        public void Init() {
            initialized = false;
            currentCountDown = countDown;

            int childCount = panelSnap.childCount;
            //children = new Transform[childCount];
            childrenDistance = new float[childCount];
            childCount = 0;
            panelSnapStartPosX = panelSnap.GetComponent<RectTransform>().anchoredPosition.x;
            childSpacing = panelSnap.GetComponent<HorizontalLayoutGroup>().spacing;
            childWidth = transform.GetComponent<RectTransform>().sizeDelta.x;
            parentHeight = panelSnap.GetComponent<RectTransform>().sizeDelta.y;

            children.Clear();
            foreach (Transform child in panelSnap) {
                childHeight = parentHeight * (childWidth / child.GetComponent<RectTransform>().sizeDelta.x);
                //Debug.Log("sizeDelta.x: " + child.GetComponent<RectTransform>().sizeDelta.x);
                //Debug.Log("sizeDelta.y: " + parentHeight);
                //Debug.Log("childWidth : " + childWidth);
                //Debug.Log("childHeight: " + childHeight);
                child.GetComponent<RectTransform>().sizeDelta = new Vector2(childWidth, childHeight);
                children.Add(child);
                childCount++;
                //print(child.name);
            }
            children = children.Where(x => x != null).ToList();

            panelSnap.GetComponent<RectTransform>().sizeDelta = new Vector2(panelSnap.GetComponent<RectTransform>().sizeDelta.x, childHeight);
            gameObject.GetComponent<LayoutElement>().minHeight = childHeight;
            gameObject.GetComponent<LayoutElement>().preferredHeight = childHeight;
            childrenGap = children[0].GetComponent<RectTransform>().sizeDelta.x + childSpacing;
            panelSnap.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            initialized = true;
        }

        //  Delete all children
        private void ResetPanelSnap() {
            if (panelSnap.childCount > 0) {
                foreach (Transform child in panelSnap) {
                    Destroy(child.gameObject);
                }
            }
        }

        public void OnDrag(PointerEventData eventData) {
            currentCountDown = countDown;
            children = children.Where(x => x != null).ToList();

            for (int a = 0; a < children.Count; a++) {
                childrenDistance[a] = Mathf.Abs(snapPoint.position.x - children[a].position.x);
            }

            float nearestDistance = Mathf.Min(childrenDistance);
            for (int a = 0; a < children.Count; a++) {
                if (nearestDistance == childrenDistance[a]) {
                    nearestChild = a;
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            currentCountDown = countDown;
            float newX = panelSnapStartPosX - (childrenGap * nearestChild);
            Vector2 newPos = new Vector2(newX, 0);
            panelSnap.GetComponent<RectTransform>().anchoredPosition = newPos;
        }
    }
}