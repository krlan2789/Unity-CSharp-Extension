using LAN.Helper;
using LAN.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAN {
    public abstract class CustomBehaviour : ExtendedBehaviour {
        protected PopupManager popupManager;
        private string epochKey = "LAN.OnLoadProgress.Epoch";
        private int progressCount = 0;
        private bool alwaysShowLoading = false;
        private bool dontShowLoading = false;

        protected GameObject prefabPopupBtn;

        private void Awake() {
            if (popupManager == null) popupManager = FindObjectOfType<PopupManager>();
        }

        protected float EpochTime {
            get {
                float e = PlayerPrefs.GetFloat(epochKey, 0f);
                //print($"ProgressCount:{progressCount} => Epoch:{e}");
                return e;
            }
            set {
                //print($"ProgressCount:{progressCount} => Epoch:{value}");
                PlayerPrefs.SetFloat(epochKey, value);
            }
        }

        protected void SetEpochTime(string subKey, float value) {
            //print($"ProgressCount:{progressCount} => Epoch:{value}");
            PlayerPrefs.SetFloat(epochKey, value);
        }

        protected float GetEpochTime(string subKey) {
            float e = PlayerPrefs.GetFloat(epochKey, 0f);
            //print($"ProgressCount:{progressCount} => Epoch:{e}");
            return e;
        }

        /// <summary>
        /// Get count of current on progress
        /// </summary>
        protected int OnProgressCount {
            get {
                //return PlayerPrefs.GetInt(onLoadKey, 0);
                return progressCount;
            }
        }

        /// <summary>
        /// Prefab for Canvas Loading
        /// </summary>
        protected GameObject PrefabCanvasLoading {
            get {
                return Resources.Load<GameObject>("LAN/Prefabs/CanvasLoading");
            }
        }

        /// <summary>
        /// Prefab for Canvas Loading
        /// </summary>
        protected CanvasLoading CanvasLoading {
            get {
                CanvasLoading canvasLoading = FindObjectOfType<CanvasLoading>();
                if (canvasLoading == null)
                {
                    canvasLoading = Instantiate(PrefabCanvasLoading).GetComponent<CanvasLoading>();
                    canvasLoading.name = "CanvasLoading";
                    DontDestroyOnLoad(canvasLoading.gameObject);
                }
                return canvasLoading;
            }
        }

        protected void OnStart() {
            //  Reset for CanvasLoading value
            //PlayerPrefs.SetInt(onLoadKey, 0);
            progressCount = 0;
            EpochTime = 0;
        }

        protected void SetLoadingVisibility(bool status) {
            dontShowLoading = !status;
        }

        protected void SetLoadingTimeOut(float seconds) {
            CanvasLoading.SetTimeOutTemporary(seconds);
        }

        /// <summary>
        /// Add count of current on load progress
        /// </summary>
        /// <param name="_alwaysShowLoading">Always show loading until pass true on method LoadIsDone</param>
        protected void LoadOnProgress(bool _alwaysShowLoading = false) {
            if (_alwaysShowLoading) alwaysShowLoading = _alwaysShowLoading;
            progressCount += 1;
            //int count = PlayerPrefs.GetInt(onLoadKey, 0) + 1;
            //PlayerPrefs.SetInt(onLoadKey, count);
            
            if (!dontShowLoading) {
                //GameObject obj = Instantiate(PrefabCanvasLoading);
                //obj.name = "CanvasLoading";
                //DontDestroyOnLoad(obj.gameObject);
                CanvasLoading.Show();
            }
            //Debug.Log("Progress - Count: " + progressCount);
        }

        /// <summary>
        /// Get count of current on load progress
        /// </summary>
        /// <param name="_forceHideLoading">Force hide loading and reset progress</param>
        /// <returns>Count of current on progress</returns>
        protected int LoadIsDone(bool _forceHideLoading = false) {
            if (!alwaysShowLoading) progressCount -= 1;
            if (_forceHideLoading) {
                progressCount = 0;
                alwaysShowLoading = false;
            }
            //int count = PlayerPrefs.GetInt(onLoadKey, 0) - 1;
            if (progressCount < 1 && !alwaysShowLoading) {
                //PlayerPrefs.SetInt(onLoadKey, 0);
                CanvasLoading.Hide();
                SetLoadingVisibility(true);
            }
            if (progressCount < 0) progressCount = 0;
            //Debug.Log("IsDone - Count: " + progressCount);
            return progressCount;
        }

        /// <summary>
        /// Showing a popup window
        /// </summary>
        /// <param name="title">Popup title, null or empty for hidden.</param>
        /// <param name="message">Popup message, null or empty for hidden.</param>
        /// <param name="buttons">Setup for popup buttons, null for use default button (depend on isQuit value).</param>
        /// <param name="isQuit">If buttons is null, and True: quit from apps, false: close popup window.</param>
        protected virtual void PopupWindow(string title, string message, PopupButton[] buttons = null, bool isQuit = true, bool disableOutside = false) {
            if (prefabPopupBtn == null) {
                prefabPopupBtn = (GameObject)Resources.Load("Prefabs/ListItem/ListItemButton");
            }

            if (buttons == null) {
                buttons = new PopupButton[1];
                buttons[0] = new PopupButton {
                    buttonText = isQuit ? "Keluar" : "Tutup",
                    buttonColor = isQuit ? Tools.PaletteRed : new Color32(255, 255, 255, 255),
                    textColor = isQuit ? Color.white : Color.gray,
                    buttonAction = () => {
                        if (isQuit) {
                            Debug.Log("Application: Quit!");
                            Application.Quit();
                        } else {
                            popupManager.Hide();
                        }
                    },
                };
            }

            if (popupManager == null) popupManager = FindObjectOfType<PopupManager>();
            popupManager.Show(title, message, prefabPopupBtn, buttons, PopupListType.BottomVertical, disableOutside);
        }

        /// <summary>
        /// Showing a popup form
        /// </summary>
        /// <param name="title">Popup title, null or empty for hidden.</param>
        /// <param name="message">Popup message, null or empty for hidden.</param>
        /// <param name="buttons">Setup for popup buttons, null for use default button (depend on isQuit value).</param>
        /// <param name="isQuit">If buttons is null, and True: quit from apps, false: close popup window.</param>
        protected virtual void PopupForm(string title, string message, string buttonLabel, Dictionary<string, PopupInputField> fields, System.Action<Dictionary<string, string>> callback, bool disableOutside = false) {
            if (callback == null) {
                callback = (Dictionary<string, string> values) => {
                    foreach (KeyValuePair<string, string> value in values) {
                        Debug.Log(value.Key + " : " + value.Value);
                    }
                };
            }

            if (popupManager == null) popupManager = FindObjectOfType<PopupManager>();
            popupManager.ShowForm(title, message, buttonLabel, fields, callback);
        }

        /// <summary>
        /// Open URL
        /// </summary>
        /// <param name="url"></param>
        public void OpenURL(string url) {
            Application.OpenURL(url);
        }
    }
}
