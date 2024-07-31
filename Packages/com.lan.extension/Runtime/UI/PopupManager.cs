using LAN.Animation;
using LAN.Extension;
using LAN.Helper;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LAN.UI {
    public enum PopupListType {
        None = 0, BottomHorizontal, BottomVertical, CenterHorizontal, CenterVertical
    }

    public enum InputFieldValidation {
        Password = 0, Standard, Email, NumberOnly
    }

    [Serializable]
    public class PopupButton {
        public string buttonText;
        public Action buttonAction;
        public Color buttonColor;
        public Color textColor;
    }

    [Serializable]
    public class PopupInputField {
        public string placeholder;
        public string defaultValue = "";
        public InputFieldValidation characterValidation;
    }

    public class PopupManager : MonoBehaviour {
        #region Pop-Up Config
        [SerializeField]
        protected bool useEditorConfig = false;
        
        //[BoxGroup("Editor Config"), ShowIf("useEditorConfig")]
        //public string title;

        //[BoxGroup("Editor Config"), ShowIf("useEditorConfig"), ResizableTextArea]
        //public string message;

        [SerializeField, BoxGroup("Editor Config"), ShowIf("useEditorConfig")]
        private PopupListType listType;

        [SerializeField, BoxGroup("Editor Config"), ShowIf("useEditorConfig")]
        private List<PopupButton> buttons;

        [SerializeField, BoxGroup("Editor Config"), ShowIf("useEditorConfig")]
        private Dictionary<string, PopupInputField> inputFields;

        [SerializeField, BoxGroup("Editor Config"), ShowIf("useEditorConfig")]
        private GameObject prefabButton;
        [SerializeField, BoxGroup("Editor Config"), ShowIf("useEditorConfig")]
        private GameObject prefabInputField;

        [SerializeField, BoxGroup("Editor Config"), ShowIf("useEditorConfig")]
        protected Transform parentList;

        [SerializeField, BoxGroup("Editor Config"), ShowIf("useEditorConfig")]
        protected AnimManager animManager;

        [HideInInspector]
        public bool IsShowUp { get; protected set; }
        protected bool isAnimating = false;

        protected GameObject panelTitle;
        protected Text titleTxt;

        private GameObject panelMessage;
        private Text messageTxt;

        protected GameObject panelNotice;
        protected Text noticeTxt;
        protected string notice = "";

        private Dictionary<string, InputField> createdIF;
        protected Button outsidePopupBtn;
        protected List<GameObject> dontDestroyThis = new List<GameObject>();
        #endregion

        protected void Awake() {
            string path = "Prefabs/PanelList/LIst" + listType.ToString();
            if (listType != PopupListType.None && parentList == null) parentList = (Transform)Resources.Load(path);
            if (prefabButton == null) prefabButton = ((GameObject)Resources.Load("Prefabs/ListItem/ListItemButton"));
            if (prefabInputField == null) prefabInputField = ((GameObject)Resources.Load("Prefabs/ListItem/InputDataObject"));
            if (animManager == null) animManager = GetComponentInChildren<AnimManager>();

            foreach (Transform obj in animManager.transform) {
                dontDestroyThis.Add(obj.gameObject);
            }

            buttons ??= new List<PopupButton>();
            createdIF ??= new Dictionary<string, InputField>();
            inputFields ??= new Dictionary<string, PopupInputField>();

            if (panelTitle == null) panelTitle = this.FindObject<Transform>("PanelPopup/PanelTitle").gameObject;
            if (titleTxt == null) titleTxt = this.FindObject<Text>("PanelPopup/PanelTitle/Title");
            if (panelMessage == null) panelMessage = this.FindObject<Transform>("PanelPopup/PanelMessage").gameObject;
            if (messageTxt == null) messageTxt = this.FindObject<Text>("PanelPopup/PanelMessage/Message");
            if (panelNotice == null) panelNotice = this.FindObject<Transform>("PanelPopup/PanelNotice").gameObject;
            if (noticeTxt == null) noticeTxt = this.FindObject<Text>("PanelPopup/PanelNotice/Notice");
            if (outsidePopupBtn == null) outsidePopupBtn = this.FindObject<Button>("Background");

            if (outsidePopupBtn != null) outsidePopupBtn.onClick.AddListener(Hide);
            if (outsidePopupBtn != null) outsidePopupBtn.gameObject.SetActive(false);

            Hide();
        }

        public virtual void SetNotice(string value) {
            notice = value;
            if (noticeTxt != null) noticeTxt.text = notice;
            if(!string.IsNullOrEmpty(value) && panelNotice != null) panelNotice.SetActive(true);
        }

        /// <summary>
        /// Show popup form window
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="buttonLabel"></param>
        /// <param name="fields"></param>
        /// <param name="callback"></param>
        public virtual void ShowForm(string title, string message, string buttonLabel, Dictionary<string, PopupInputField> fields, Action<Dictionary<string, string>> callback) {
            PopupButton[] buttons = new PopupButton[1] {
                new PopupButton {
                    buttonText = buttonLabel,
                    buttonColor = new Color32(21, 21, 21, 255),
                    textColor = Color.white,
                    buttonAction = () => {
                        Hide(() => {
                            Dictionary<string, string> listValue = new Dictionary<string, string>();
                            foreach (KeyValuePair<string, InputField> kvp in createdIF) {
                                listValue.Add(kvp.Key, kvp.Value.text);
                            }
                            callback?.Invoke(listValue);
                        });
                    },
                }
            };

            inputFields.Clear();
            foreach (KeyValuePair<string, PopupInputField> kvp in fields) {
                if (inputFields.ContainsKey(kvp.Key)) inputFields[kvp.Key] = kvp.Value;
                else inputFields.Add(kvp.Key, kvp.Value);
            }
            Show(title, message, (GameObject)Resources.Load("Prefabs/ListItem/ListItemButton"), buttons, PopupListType.BottomVertical);
        }

        /// <summary>
        /// Show popup window
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="prefabButton"></param>
        /// <param name="listButtons"></param>
        /// <param name="listType"></param>
        /// <param name="disableOutside"></param>
        public virtual void Show(string title = "", string message = "", GameObject prefabButton = null, PopupButton[] listButtons = null, PopupListType listType = PopupListType.None, bool disableOutside = false) {
            if (IsShowUp) return;
            if (animManager == null) animManager = this.FindObject<AnimManager>("PanelPopUp");
            if (animManager != null) {
                foreach (Transform obj in animManager.transform) {
                    if (!dontDestroyThis.Contains(obj.gameObject)) {
                        Destroy(obj.gameObject);
                    }
                }
            }

            if (prefabButton == null) prefabButton = this.prefabButton;
            if (listButtons != null && listButtons.Length > 0) {
                buttons.Clear();
                buttons.AddRange(listButtons);
            }

            //  Get reference for parent for inputfields and buttons
            if (listType != PopupListType.None) this.listType = listType;
            if (this.listType != PopupListType.None) {
                string path = "Prefabs/List" + this.listType.ToString();
                parentList = ((GameObject)Resources.Load(path)).transform;
            }
            if (parentList != null && this.listType == PopupListType.None) {
                Destroy(parentList.gameObject);
                parentList = null;
            }

            //  Show or hide popup title
            if (!string.IsNullOrEmpty(title)) {
                if (titleTxt != null) titleTxt.text = title;
                if (parentList != null) panelTitle.gameObject.SetActive(true);
            } else {
                if (parentList != null) panelTitle.gameObject.SetActive(false);
            }

            //  Show or hide popup message
            if (!string.IsNullOrEmpty(message)) {
                if (messageTxt != null) messageTxt.text = message;
                if (panelMessage != null) panelMessage.gameObject.SetActive(true);
            } else {
                if (panelMessage != null) panelMessage.gameObject.SetActive(false);
            }

            //  Show or hide popup notice
            if (!string.IsNullOrEmpty(notice)) {
                if (noticeTxt != null) noticeTxt.text = notice;
                if (panelNotice != null) panelNotice.gameObject.SetActive(true);
            } else {
                if (panelNotice != null) panelNotice.gameObject.SetActive(false);
            }
            notice = "";

            if (parentList != null && animManager != null) {
                //  Create parent for inputfields and buttons
                parentList = parentList.gameObject.CreateObject(animManager.transform).transform;
                //  Creating inputfields
                createdIF.Clear();
                if (inputFields.Count > 0) {
                    //Debug.Log("inputFields.Count: " + inputFields.Count);
                    foreach (KeyValuePair<string, PopupInputField> kvp in inputFields) {
                        switch (kvp.Value.characterValidation) {
                            case InputFieldValidation.Standard:
                                prefabInputField = ((GameObject)Resources.Load("Prefabs/ListItem/InputDataObject"));
                                break;
                            case InputFieldValidation.Email:
                                prefabInputField = ((GameObject)Resources.Load("Prefabs/ListItem/InputEmailDataObject"));
                                break;
                            case InputFieldValidation.NumberOnly:
                                prefabInputField = ((GameObject)Resources.Load("Prefabs/ListItem/InputNumberOnlyDataObject"));
                                break;
                            case InputFieldValidation.Password:
                                prefabInputField = ((GameObject)Resources.Load("Prefabs/ListItem/InputPasswordDataObject"));
                                break;
                        }

                        Transform inputfield = prefabInputField.CreateObject(parentList).transform;
                        inputfield.name = kvp.Key;
                        VerticalLayoutGroup vlg = inputfield.GetComponent<VerticalLayoutGroup>();
                        vlg.padding = new RectOffset(32, 32, vlg.padding.top, 32);
                        //inputfield.Find("InputField/Text Area/Text").GetComponent<TMP_Text>().text = kvp.Value.defaultValue;
                        inputfield.Find("InputField/Text Area/Placeholder").GetComponent<Text>().text = kvp.Value.placeholder;
                        inputfield.Find("Label").gameObject.SetActive(false);

                        InputField tmpIF = inputfield.Find("InputField").GetComponent<InputField>();
                        tmpIF.text = kvp.Value.defaultValue;
                        createdIF.Add(kvp.Key, tmpIF);
                        this.FixVerticalLayout(inputfield);
                    }
                }

                //  Creating buttons
                //Debug.Log($"prefabButton: {prefabButton != null} | buttons.Count: {buttons.Count}");
                if (prefabButton != null && buttons.Count > 0) {
                    foreach (PopupButton pb in buttons) {
                        //Debug.Log("Create popup button " + pb.buttonText);
                        Button btn = prefabButton.CreateObject(parentList).GetComponent<Button>();
                        btn.transform.Find("Text").GetComponent<Text>().text = pb.buttonText;
                        btn.transform.Find("Text").GetComponent<Text>().color = pb.textColor;
                        btn.image.color = pb.buttonColor;
                        btn.onClick.AddListener(() => {
                            Hide(pb.buttonAction);
                        });
                    }
                }
            }

            //  Reset list of button and inputfield that need to be create
            buttons.Clear();
            inputFields.Clear();

            //  Showing Popup and outside area
            if (outsidePopupBtn != null) {
                outsidePopupBtn.gameObject.SetActive(true);
                outsidePopupBtn.interactable = !disableOutside;
            }
            SetPopUpHeight(panelMessage, parentList.GetComponent<RectTransform>().sizeDelta.y);
            if (!messageTxt.text.IsEmpty() || !titleTxt.text.IsEmpty() || parentList.childCount > 0) StartCoroutine(Showing());
        }

        public virtual void ShowDetail(string title = "", string message = "", Transform[] contents = null, bool disableOutside = false) {
            if (IsShowUp) return;
            if (animManager == null) animManager = this.FindObject<AnimManager>("PanelPopUp");
            if (animManager != null) {
                foreach (Transform obj in animManager.transform) {
                    if (!dontDestroyThis.Contains(obj.gameObject)) {
                        Destroy(obj.gameObject);
                    }
                }
            }

            //  Get reference for parent for inputfields and buttons
            listType = PopupListType.BottomVertical;
            if (listType != PopupListType.None) {
                string path = "Prefabs/List" + this.listType.ToString();
                parentList = ((GameObject)Resources.Load(path)).transform;
            }

            //  Show or hide popup title
            if (!string.IsNullOrEmpty(title)) {
                if (titleTxt != null) titleTxt.text = title;
                if (parentList != null) panelTitle.gameObject.SetActive(true);
            } else {
                if (parentList != null) panelTitle.gameObject.SetActive(false);
            }

            //  Show or hide popup message
            if (!string.IsNullOrEmpty(message)) {
                if (messageTxt != null) messageTxt.text = message;
                if (panelMessage != null) panelMessage.gameObject.SetActive(true);
            } else {
                if (panelMessage != null) panelMessage.gameObject.SetActive(false);
            }

            //  Show or hide popup notice
            if (!string.IsNullOrEmpty(notice)) {
                if (noticeTxt != null) noticeTxt.text = notice;
                if (panelNotice != null) panelNotice.gameObject.SetActive(true);
            } else {
                if (panelNotice != null) panelNotice.gameObject.SetActive(false);
            }
            notice = "";

            //  Create detail items
            if (parentList != null && animManager != null) {
                //  Create parent for inputfields and buttons
                parentList = parentList.gameObject.CreateObject(animManager.transform).transform;

                if (contents.Length > 0) {
                    foreach (var content in contents) {
                        content.SetParent(parentList);
                        content.localScale = Vector3.one;
                    }
                }

                if (prefabButton != null) {
                    Button btn = prefabButton.CreateObject(parentList).GetComponent<Button>();
                    btn.transform.Find("Text").GetComponent<Text>().text = "Tutup";
                    //btn.transform.Find("Text").GetComponent<TMP_Text>().color = Color.white;
                    //btn.image.color = SalesTracker.Main.Tools.PaletteMaroon;
                    btn.onClick.AddListener(Hide);
                }
            }

            //  Reset list of button and inputfield that need to be create
            buttons.Clear();
            inputFields.Clear();

            //  Showing Popup and outside area
            if (outsidePopupBtn != null) {
                outsidePopupBtn.gameObject.SetActive(true);
                outsidePopupBtn.interactable = !disableOutside;
            }
            SetPopUpHeight(parentList.gameObject);
            if (!messageTxt.text.IsEmpty() || !titleTxt.text.IsEmpty() || parentList.childCount > 0) StartCoroutine(Showing());
        }

        /// <summary>
        /// Hide popup window / form window
        /// </summary>
        public virtual void Hide() {
            try {
                StartCoroutine(Hiding(null));
            } catch (Exception e) {
                Debug.Log(e.Source + "\n" + e.Message);
            }
        }

        /// <summary>
        /// Hide popup window / form window
        /// </summary>
        /// <param name="callback"></param>
        public virtual void Hide(Action callback) {
            try {
                StartCoroutine(Hiding(callback));
            } catch (Exception e) {
                Debug.Log(e.Source + "\n" + e.Message);
            }
        }

        protected virtual void SetPopUpHeight(GameObject limitThisPanel, float additonalPaddingTop = 0) {
            Canvas canvas = FindObjectOfType<Canvas>();
            float maxPopupHeight = canvas.GetComponent<RectTransform>().sizeDelta.y * .72f;
            float popupHeight = animManager != null ? animManager.GetComponent<RectTransform>().sizeDelta.y : 0;
            float panelHeight = limitThisPanel != null ? limitThisPanel.GetComponent<RectTransform>().sizeDelta.y : 0;

            //Debug.Log($"pMessageHeight: {pMessageHeight} | popupHeight: {popupHeight} | maxPopupHeight: {maxPopupHeight}");
            if (popupHeight > maxPopupHeight && limitThisPanel != null) {
                LayoutElement mle = limitThisPanel.GetComponent<LayoutElement>();
                mle.preferredHeight = maxPopupHeight - additonalPaddingTop;
                limitThisPanel.GetComponent<LayoutElement>().enabled = true;
            }
        }

        protected virtual IEnumerator Showing() {
            yield return null;
            //SetPopUpHeight();
            if (panelMessage != null) panelMessage.GetComponent<ScrollRect>().normalizedPosition = Vector2.up;
            isAnimating = true;
            if (animManager != null) {
                animManager.SetSwingSetting(swingTargetByPercent: new Vector3(.5f, 0f, 0f), duration: .25f);
                animManager.StartMove(AnimationSwingType.BottomUp);
            }
            IsShowUp = true;
            isAnimating = false;
        }

        protected virtual IEnumerator Hiding(Action callback) {
            if (panelMessage != null) {
                LayoutElement mle = panelMessage.GetComponent<LayoutElement>();
                mle.enabled = false;
                yield return null;
                mle.preferredHeight = -1;
                mle.enabled = true;
            }

            if (messageTxt != null) messageTxt.text = "";
            isAnimating = true;
            if (animManager != null) {
                float duration = .15f;
                animManager.SetSwingSetting(swingTargetByPercent: new Vector3(.5f, -.8f, 0f), duration: duration);
                animManager.StartMove(AnimationSwingType.TopDown);
                yield return new WaitForSeconds(duration);
            }
            if (outsidePopupBtn != null) outsidePopupBtn.gameObject.SetActive(false);


            //Debug.Log("ParentList: " + "Prefabs/PanelList/LIst" + listType.ToString());
            if (parentList == null) parentList = (Transform)Resources.Load("Prefabs/PanelList/LIst" + listType.ToString());
            if (animManager == null) animManager = this.FindObject<AnimManager>("PanelPopUp");
            if (parentList != null) foreach (Transform obj in parentList) Destroy(obj.gameObject);

            IsShowUp = false;
            isAnimating = false;
            yield return null;

            callback?.Invoke();
        }
    }
}
