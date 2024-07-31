#if UNITY_ANDROID && !UNITY_EDITOR
using FantomLib;
#endif
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LAN.UI {
    public class DatePicker : MonoBehaviour {
        public enum PickerType {
            Custom = 0, AndroidNative
        }

        #region Editor Config
        [SerializeField] private PickerType pickerType = PickerType.AndroidNative;
        [SerializeField] private CalenderManager.VisibilityType visibilityType = CalenderManager.VisibilityType.PREVIOUS;
        private bool _useAndroidNativeCalender {
            get {
                bool status = pickerType == PickerType.AndroidNative;
                return status;
            }
        }
        private bool _useCustomCalender {
            get {
                bool status = pickerType == PickerType.Custom;
                return status;
            }
        }

        #region Android Native Calender
        [SerializeField, InfoBox("Required DatePickerController"), ShowIf("_useAndroidNativeCalender")] private Button pickerBtn;
#if UNITY_ANDROID && !UNITY_EDITOR
        private DatePickerController controller;
#endif
        #endregion Android Native Calender
        #endregion Editor Config

        [SerializeField] private bool useTextMeshPro = false;
        [SerializeField, HideIf("useTextMeshPro")] private Text labelTxt;
        [SerializeField, ShowIf("useTextMeshPro")] private TMP_Text labelTmpTxt;

        //#region Custom Start Date
        //[SerializeField] private bool customStart = false;
        //[SerializeField, ShowIf("customStart")] private int addYears = 0;
        //[SerializeField, ShowIf("customStart")] private int addMonths = 0;
        //[SerializeField, ShowIf("customStart")] private int addDays = 0;
        //[SerializeField, ShowIf("customStart")] private int addHours = 0;
        //[SerializeField, ShowIf("customStart")] private int addMinutes = 0;
        //[SerializeField, ShowIf("customStart")] private int addSeconds = 0;
        //#endregion Custom Start Date

        private Button button;
        private string currentDate = "";
        public event UnityAction<string> OnResults;

        private void Awake() {
            button = GetComponent<Button>();

            DateTime startDate = System.DateTime.Now;
            //if (customStart) {
            //    startDate = startDate.AddYears(addYears).AddMonths(addMonths).AddDays(addDays).AddHours(addHours).AddMinutes(addMinutes).AddSeconds(addSeconds);
            //}
            currentDate = startDate.ToString("yyyy-MM-dd");

            switch (pickerType) {
                case PickerType.AndroidNative:
#if UNITY_ANDROID && !UNITY_EDITOR
                    controller = GetComponent<DatePickerController>();
                    pickerBtn.onClick.AddListener(Show);
#else
                    Debug.Log("Date Picker Android Native is called");
#endif
                    break;
            }
        }

        private void OnEnable() {
            UpdateUI(currentDate);
        }

        private void Start() {
            if (button != null) button.onClick.AddListener(Show);
            UpdateUI(currentDate);
        }

        public void Show() {
            Show(currentDate);
        }

        public void Show(string defaultDate = "", UnityAction<string> callback = null, string format = "") {
            switch (pickerType) {
                case PickerType.AndroidNative:
#if UNITY_ANDROID && !UNITY_EDITOR
                    if (controller == null) {
                        Debug.LogWarning("DatePickerController doesn't exist");
                        return;
                    }

                    if (string.IsNullOrEmpty(defaultDate)) controller.defaultDate = DateTime.Now.ToString("yyyy/M/d");
                    if (string.IsNullOrEmpty(format)) controller.resultDateFormat = format;

                    controller.OnResult.AddListener((string str) => {
                        currentDate = str;
                        UpdateUI(str);
                        OnResults?.Invoke(str);
                        callback?.Invoke(str);
                        controller.OnResult.RemoveAllListeners();
                    });

                    if (string.IsNullOrEmpty(defaultDate)) controller.Show();
                    else controller.Show(defaultDate);
#endif
                    break;
                case PickerType.Custom:
                    DatePickerUIManager.Instance.Show(currentDate, visibilityType, (string result) => {
                        currentDate = result;
                        Debug.Log("DatePicker: " + currentDate);
                        UpdateUI(result);
                        OnResults?.Invoke(result);
                        callback?.Invoke(result);
                    });
                    break;
            }
        }

        public void UpdateUI(string str) {
            //Debug.Log("UpdateUI: " + str);
            string[] date = str.Replace("/", "-").Substring(0, 10).Split('-');
            string formatedDate = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2])).ToString("dd MMM yyyy");
            if (!useTextMeshPro && labelTxt != null) labelTxt.text = formatedDate;
            if (useTextMeshPro && labelTmpTxt != null) labelTmpTxt.text = formatedDate;
        }
    }
}
