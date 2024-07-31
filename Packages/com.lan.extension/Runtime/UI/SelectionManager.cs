using LAN.Animation;
using LAN.Extension;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LAN.UI {
    public class SelectionManager : MonoBehaviour {
        #region Static instance from this class
        private static SelectionManager instance;
        public static SelectionManager Instance {
            get {
                instance = instance ?? FindObjectOfType<SelectionManager>();
                if (instance == null) {
                    instance = Instantiate((GameObject)Resources.Load("Prefabs/DontDestroy/CanvasSelection")).GetComponent<SelectionManager>();
                }
                return instance;
            }
        }
        #endregion

        #region Pop-Up Config
        [SerializeField]
        private bool useEditorConfig = false;

        [SerializeField, BoxGroup("Editor Config"), ShowIf("useEditorConfig")]
        private Transform parentList;

        [SerializeField, BoxGroup("Editor Config"), ShowIf("useEditorConfig")]
        private AnimManager animManager;

        [HideInInspector]
        public bool IsShowUp { get; protected set; }
        private bool isAnimating = false;

        private GameObject panelTitle;
        private TMP_Text titleTxt;

        private GameObject panelNotice;
        private Text noticeTxt;
        //private string notice = "";

        private Button outsidePopupBtn;
        private List<GameObject> dontDestroyThis = new List<GameObject>();
        private Dictionary<string, Transform> optionsDict = new Dictionary<string, Transform>();

        private ScrollRect scrollRect;
        private GameObject popup;
        private TMP_InputField searchIF;
        #endregion

        private void Awake() {
            if (instance != null) DontDestroyOnLoad(instance.gameObject);
            popup = this.FindObject<Transform>("Popup").gameObject;
            if (animManager == null) animManager = this.FindObject<AnimManager>("Popup/PanelPopup");
            if (scrollRect == null) scrollRect = this.FindObject<ScrollRect>("Popup/PanelPopup/List");
            if (panelTitle == null) panelTitle = this.FindObject<Transform>("Popup/PanelPopup/Title").gameObject;
            if (titleTxt == null) titleTxt = this.FindObject<TMP_Text>("Popup/PanelPopup/Title/Text");
            if (searchIF == null) searchIF = this.FindObject<TMP_InputField>("Popup/PanelPopup/Search/InputField");
            if (parentList == null) parentList = this.FindObject<Transform>("Popup/PanelPopup/List/Content");
            if (panelNotice == null) panelNotice = this.FindObject<Transform>("Popup/PanelPopup/Footer").gameObject;
            if (noticeTxt == null) noticeTxt = this.FindObject<Text>("Popup/PanelPopup/Footer/Text");
            if (outsidePopupBtn == null) outsidePopupBtn = this.FindObject<Button>("Popup/Background");
        }

        private void Start() {
            popup.SetActive(false);
            panelNotice.SetActive(false);

            foreach (Transform obj in animManager.transform) {
                dontDestroyThis.Add(obj.gameObject);
            }

            IsShowUp = false;
            searchIF.onValueChanged.AddListener(OnSearching);
        }

        private void OnSearching(string keyword) {
            if (!string.IsNullOrEmpty(keyword)) {
                foreach (var kvp in optionsDict) {
                    if (kvp.Value != null) kvp.Value.gameObject.SetActive(kvp.Key.ToLower().Contains(keyword.ToLower()));
                }
            } else {
                foreach (var kvp in optionsDict) {
                    if (kvp.Value != null) kvp.Value.gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Show Selection
        /// </summary>
        /// <param name="title"></param>
        /// <param name="options"></param>
        /// <param name="disableOutside"></param>
        /// <param name="disableOnSelect"></param>
        public void Show(string title = "", Dictionary<string, Transform> options = null, bool disableOutside = false, bool disableOnSelect = true, bool childControlHeight = true) {
            parentList.GetComponent<VerticalLayoutGroup>().childControlHeight = childControlHeight;

            //  Reset search bar
            searchIF.text = "";

            //  Title
            if (titleTxt != null) {
                titleTxt.text = title;
            }
            if (panelTitle != null) panelTitle.SetActive(titleTxt != null && !string.IsNullOrEmpty(titleTxt.text));

            //  Options
            optionsDict = options;
            if (optionsDict != null && optionsDict.Count > 0) {
                foreach (var kvp in optionsDict) {
                    kvp.Value.transform.SetParent(parentList);
                    kvp.Value.transform.localScale = Vector3.one;
                    if (disableOnSelect) kvp.Value.GetComponent<Button>().onClick.AddListener(Hide);
                }
                //FixVerticalLayout(parentList);
            }

            //  Showing Popup and outside area
            if (outsidePopupBtn != null) {
                outsidePopupBtn.interactable = !disableOutside;
            }
            StartCoroutine(Showing());
        }

        protected IEnumerator Showing() {
            scrollRect.verticalNormalizedPosition = 1;
            yield return StartCoroutine(this.FixingVerticalLayout(parentList, true, .0001f));
            popup.SetActive(true);

            if (parentList != null) parentList.parent.GetComponent<ScrollRect>().normalizedPosition = Vector2.up;

            if (animManager != null) {
                isAnimating = true;
                animManager.SetScaleSetting(0, .25f, () => {
                    IsShowUp = true;
                    isAnimating = false;
                });
                yield return animManager.PlayScaleUpY();
            }
        }

        /// <summary>
        /// Hide popup window / form window
        /// </summary>
        public virtual void Hide() {
            try {
                if (animManager == null) animManager = this.FindObject<AnimManager>("PanelPopUp");
                if (parentList != null) foreach (Transform obj in parentList) Destroy(obj.gameObject);

                StartCoroutine(Hiding());
            } catch (Exception e) {
                Debug.Log(e.Source + "\n" + e.Message);
            }
        }

        protected IEnumerator Hiding() {
            yield return null;
            if (parentList != null) {
                foreach (Transform obj in parentList) {
                    yield return null;
                    Destroy(obj.gameObject);
                }
            }

            if (animManager != null) {
                isAnimating = true;
                animManager.SetScaleSetting(0, .15f, () => {
                    popup.SetActive(false);
                    IsShowUp = false;
                    isAnimating = false;
                });
                yield return animManager.PlayScaleDownY();
            }
        }
    }
}
