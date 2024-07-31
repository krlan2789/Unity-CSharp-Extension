using LAN.Helper;
using LAN.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAN.Sample
{
    public abstract class CustomBehaviour : MonoBehaviour
    {
        protected PopupManager popupManager;
        protected GameObject prefabPopupBtn;

        private void Awake()
        {
            if (popupManager == null) popupManager = FindObjectOfType<PopupManager>();
        }

        /// <summary>
        /// Prefab for Canvas Loading
        /// </summary>
        protected GameObject PrefabCanvasLoading
        {
            get
            {
                return Resources.Load<GameObject>("LAN/Prefabs/CanvasLoading");
            }
        }

        /// <summary>
        /// Prefab for Canvas Loading
        /// </summary>
        protected CanvasLoading CanvasLoading
        {
            get
            {
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

        /// <summary>
        /// Showing a popup window
        /// </summary>
        /// <param name="title">Popup title, null or empty for hidden.</param>
        /// <param name="message">Popup message, null or empty for hidden.</param>
        /// <param name="buttons">Setup for popup buttons, null for use default button (depend on isQuit value).</param>
        /// <param name="isQuit">If buttons is null, and True: quit from apps, false: close popup window.</param>
        protected virtual void PopupWindow(string title, string message, PopupButton[] buttons = null, bool isQuit = true, bool disableOutside = false)
        {
            if (prefabPopupBtn == null)
            {
                prefabPopupBtn = (GameObject)Resources.Load("Prefabs/ListItem/ListItemButton");
            }

            if (buttons == null)
            {
                buttons = new PopupButton[1];
                buttons[0] = new PopupButton
                {
                    buttonText = isQuit ? "Keluar" : "Tutup",
                    buttonColor = isQuit ? Tools.PaletteRed : new Color32(255, 255, 255, 255),
                    textColor = isQuit ? Color.white : Color.gray,
                    buttonAction = () => {
                        if (isQuit)
                        {
                            Debug.Log("Application: Quit!");
                            Application.Quit();
                        } else
                        {
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
        protected virtual void PopupForm(string title, string message, string buttonLabel, Dictionary<string, PopupInputField> fields, System.Action<Dictionary<string, string>> callback, bool disableOutside = false)
        {
            if (callback == null)
            {
                callback = (Dictionary<string, string> values) => {
                    foreach (KeyValuePair<string, string> value in values)
                    {
                        Debug.Log(value.Key + " : " + value.Value);
                    }
                };
            }

            if (popupManager == null) popupManager = FindObjectOfType<PopupManager>();
            popupManager.ShowForm(title, message, buttonLabel, fields, callback);
        }
    }
}
