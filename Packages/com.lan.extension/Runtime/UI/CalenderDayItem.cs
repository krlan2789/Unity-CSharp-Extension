using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LAN.UI
{
    [RequireComponent(typeof(Button))]
    public class CalenderDayItem : MonoBehaviour
    {
        public UnityAction<string> onDaySelected;
        private Button btn;
        private Text label;

        public Button Btn => btn;
        public Text Label => label;
        public int Day { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
        public DateTime Date
        {
            get
            {
                return Day > 0 && Month > 0 ? new DateTime(Year, Month, Day) : DateTime.Now;
            }
        }
        public bool IsActive
        {
            get
            {
                return btn != null && btn.interactable;
            }
        }

        private void Awake()
        {
            btn = GetComponent<Button>();
            label = GetComponentInChildren<Text>();
            btn.onClick.AddListener(OnClick);
        }

        public void Init(int year, int month, int day, CalenderManager.VisibilityType visibility = CalenderManager.VisibilityType.ALL)
        {
            Day = day;
            Month = month;
            Year = year;

            switch (visibility)
            {
                case CalenderManager.VisibilityType.ALL:
                    btn.interactable = Day > 0;
                    break;
                case CalenderManager.VisibilityType.PREVIOUS:
                    btn.interactable = (Day > 0 && new DateTime(year, month, Day) <= DateTime.Now);
                    break;
                case CalenderManager.VisibilityType.NEXT:
                    btn.interactable = (Day > 0 && new DateTime(year, month, Day) >= DateTime.Now);
                    break;
                case CalenderManager.VisibilityType.THIS_MONTH:
                    btn.interactable = (Day > 0 && year == DateTime.Now.Year && month == DateTime.Now.Month);
                    break;
                case CalenderManager.VisibilityType.THIS_YEAR:
                    btn.interactable = (Day > 0 && year == DateTime.Now.Year);
                    break;
            }

            label.text = Day > 0 ? "" + Day : "";
        }

        private void OnClick()
        {
            string dateString = string.Format("{0:0000}-{1:00}-{2:00}", Year, Month, Day);
            Debug.Log("Calender.Selected: " + dateString);
            onDaySelected?.Invoke(dateString);
        }
    }
}
