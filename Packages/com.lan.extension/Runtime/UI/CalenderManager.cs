using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LAN.UI
{
    public class CalenderManager : MonoBehaviour
    {
        public enum VisibilityType
        {
            ALL = 0,
            PREVIOUS = 1,
            NEXT = 2,
            THIS_MONTH = 3,
            THIS_YEAR = 4,
        }

        // References to UI elements
        public Text monthText;
        public Text yearText;
        public Button prevMonthButton;
        public Button nextMonthButton;
        public Transform dayButtonContainer;
        public CalenderDayItem dayButtonPrefab;
        public VisibilityType visibility = VisibilityType.PREVIOUS;

        [SerializeField, Header("Date item size and spacing")] private Vector2 cellSize = new(80, 80);
        [SerializeField] private Vector2 spacing = new(5, 5);

        private List<CalenderDayItem> dayButtons = new List<CalenderDayItem>();
        public UnityAction<string> onDaySelected;
        public UnityAction<int, byte> onMonthChanged;
        public UnityAction<CalenderDayItem> onCurrentDayFound;

        // Variables for keeping track of the current date
        private DateTime currentDate;
        private string selectedDate = "";
        private int currentMonth;
        private int currentYear;

        private readonly byte rowsCount = 6;
        private readonly byte columnsCount = 7;

        public CalenderDayItem[] DayItems
        {
            get
            {
                return dayButtons.ToArray();
            }
        }

        public int[] Days
        {
            get
            {
                List<int> days = new List<int>();
                foreach (var day in dayButtons)
                {
                    days.Add(day.Day);
                }
                return days.ToArray();
            }
        }

        public int[] ActiveDays
        {
            get
            {
                List<int> days = new List<int>();
                foreach (var day in dayButtons)
                {
                    if (day.IsActive) days.Add(day.Day);
                }
                return days.ToArray();
            }
        }

        // Start is called before the first frame update
        private void Awake()
        {
            // Get the current date
            currentDate = DateTime.Today;
        }

        private void OnEnable()
        {
            // Set the initial month and year
            currentMonth = currentDate.Month;
            currentYear = currentDate.Year;
            onMonthChanged?.Invoke(currentYear, (byte)currentMonth);

            // Update the UI elements
            UpdateUI();

            // Create the day buttons
            CreateDayButtons();
        }

        private void Start()
        {
            nextMonthButton.onClick.AddListener(NextMonth);
            prevMonthButton.onClick.AddListener(PrevMonth);
        }

        /// <summary>
        /// Generate and show calender with custom initial date : yyyy-MM-dd
        /// </summary>
        /// <param name="initialDate">yyyy-MM-dd</param>
        public void Show(string initialDate)
        {
            if (!string.IsNullOrEmpty(initialDate)) currentDate = DateTime.ParseExact(initialDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            selectedDate = initialDate;
            gameObject.SetActive(true);
        }

        // Move to the previous month
        public void PrevMonth()
        {
            // Subtract one month from the current date
            currentDate = currentDate.AddMonths(-1);

            // Update the month and year
            currentMonth = currentDate.Month;
            currentYear = currentDate.Year;
            onMonthChanged?.Invoke(currentYear, (byte)currentMonth);

            // Update the UI elements
            UpdateUI();

            // Create the day buttons for the new month
            CreateDayButtons();
        }

        // Move to the next month
        public void NextMonth()
        {
            // Add one month to the current date
            currentDate = currentDate.AddMonths(1);

            // Update the month and year
            currentMonth = currentDate.Month;
            currentYear = currentDate.Year;
            onMonthChanged?.Invoke(currentYear, (byte)currentMonth);

            // Update the UI elements
            UpdateUI();

            // Create the day buttons for the new month
            CreateDayButtons();
        }

        // Update the UI elements with the current month and year
        private void UpdateUI()
        {
            // Update the month and year text
            monthText.text = currentDate.ToString("MMMM");
            yearText.text = currentDate.ToString("yyyy");
        }

        // Create the day buttons for the current month
        private void CreateDayButtons()
        {
            // Clear any existing day buttons
            foreach (CalenderDayItem child in dayButtons)
            {
                Destroy(child.gameObject);
            }

            dayButtons.Clear();

            // Get the number of days in the current month
            int numDays = DateTime.DaysInMonth(currentYear, currentMonth);

            // Get the date of the first day of the month
            DateTime firstDayOfMonth = new DateTime(currentYear, currentMonth, 1);

            // Calculate the day of the week of the first day of the month (0 = Sunday, 1 = Monday, ..., 6 = Saturday)
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            // Add blank buttons for the days before the first of the month
            for (int i = 0; i < firstDayOfWeek; i++)
            {
                CalenderDayItem blankButton = Instantiate(dayButtonPrefab, dayButtonContainer);
                blankButton.Init(0, 0, 0, visibility);
                dayButtons.Add(blankButton);
            }

            // Create a button for each day of the month
            for (int i = 1; i <= numDays; i++)
            {
                // Create a new day button
                CalenderDayItem dayButton = Instantiate(dayButtonPrefab, dayButtonContainer);

                //  Add on selected action
                dayButton.onDaySelected = onDaySelected;
                dayButton.Init(currentYear, currentMonth, i, visibility);

                // Add the day button to the list
                dayButtons.Add(dayButton);

                //  Execute on current day found callback
                if (dayButton.Date.ToString("yyyy-MM-dd") == selectedDate)
                {
                    onCurrentDayFound?.Invoke(dayButton);
                }
            }

            // Sort the day buttons by day of the week (Monday first)
            dayButtons.Sort((a, b) => {
                int dayA = a.Day;
                int dayB = b.Day;

                if (dayA < 1) dayA = 1;
                if (dayB < 1) dayB = 1;

                DateTime dateA = new DateTime(currentYear, currentMonth, dayA);
                DateTime dateB = new DateTime(currentYear, currentMonth, dayB);

                int result = dateA.DayOfWeek.CompareTo(dateB.DayOfWeek);

                if (result == 0)
                {
                    result = dayA.CompareTo(dayB);
                }

                return result;
            });

            //  Fill empty cell with blank button
            int totalMaxCells = rowsCount * columnsCount;
            if (dayButtons.Count < totalMaxCells - 1)
            {
                for (int a = dayButtons.Count; a < totalMaxCells; a++)
                {
                    CalenderDayItem blankButton = Instantiate(dayButtonPrefab, dayButtonContainer);
                    blankButton.Init(0, 0, 0, visibility);
                    dayButtons.Add(blankButton);
                }
            }

            // Set the day button container to grid layout mode
            GridLayoutGroup layout = dayButtonContainer.GetComponent<GridLayoutGroup>();
            layout.cellSize = cellSize;
            layout.spacing = spacing;

            // Set the day buttons as children of the container
            foreach (CalenderDayItem button in dayButtons)
            {
                button.transform.SetParent(dayButtonContainer, false);
            }
        }
    }
}
