using LAN.Extension;
using LAN.Helper;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LAN.UI {
    public class DatePickerUIManager : CustomBehaviour {
        #region Static instance of this class
        private static DatePickerUIManager instance;
        public static DatePickerUIManager Instance {
            get {
                instance = instance != null ? instance : FindObjectOfType<DatePickerUIManager>();
                return instance;
            }
        }
        #endregion

        [SerializeField] private CalenderManager calender;

        #region Custom Start Date
        [SerializeField] private bool customStart = false;
        [SerializeField, ShowIf("customStart")] private int addYears = 0;
        [SerializeField, ShowIf("customStart")] private int addMonths = 0;
        [SerializeField, ShowIf("customStart")] private int addDays = 0;
        [SerializeField, ShowIf("customStart")] private int addHours = 0;
        [SerializeField, ShowIf("customStart")] private int addMinutes = 0;
        [SerializeField, ShowIf("customStart")] private int addSeconds = 0;
        #endregion Custom Start Date

        private Button bgPanelBtn;
        private Transform popup;
        private string currentDate = "";
        private UnityAction<string> onResults;

        private void Awake() {
            DontDestroyOnLoad(gameObject);
            //if (instance.gameObject.GetInstanceID() != gameObject.GetInstanceID()) Destroy(instance.gameObject);

            bgPanelBtn = FindObject<Button>("Background");
            popup = transform.Find("Popup");

            DateTime startDate = System.DateTime.Now;
            if (customStart) {
                startDate = startDate.AddYears(addYears).AddMonths(addMonths).AddDays(addDays).AddHours(addHours).AddMinutes(addMinutes).AddSeconds(addSeconds);
            }
            currentDate = startDate.ToString("yyyy-MM-dd");

            if (calender != null) {
                calender.onDaySelected = (string date) => {
                    Debug.Log($"DatePickerUIManager.onDayselected is clicked : {date}");
                    onResults?.Invoke(date);
                    Hide();
                };
            }

            bgPanelBtn.onClick.AddListener(Hide);

            Hide();
        }

        public void Show() {
            Show(currentDate);
        }

        public void Show(string defaultDate = "", CalenderManager.VisibilityType visibilityType = CalenderManager.VisibilityType.ALL, UnityAction<string> callback = null) {
            onResults = callback;
            gameObject.SetActive(true);
            if (bgPanelBtn != null) bgPanelBtn.transform.SetActive(true);
            if (popup != null) popup.SetActive(true);
            if (calender != null) {
                calender.visibility = visibilityType;
                calender.onCurrentDayFound = (CalenderDayItem day) => {
                    Color btnColor = day.Btn.image.color;
                    Color labelColor = day.Label.color;
                    day.Btn.image.color = labelColor;
                    day.Label.color = btnColor;
                    day.Label.fontStyle = FontStyle.Bold;
                };
                calender.Show(defaultDate ?? currentDate);
            }
        }

        public void Hide() {
            if (calender != null) calender.gameObject.SetActive(false);
            if (bgPanelBtn != null) bgPanelBtn.transform.SetActive(false);
            if (popup != null) popup.SetActive(false);
        }
    }
}
