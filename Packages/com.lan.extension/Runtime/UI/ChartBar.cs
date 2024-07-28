using LAN.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LAN.UI {
    public class ChartBar : Charts {
        [SerializeField, Header("Target Information")] private RectTransform targetLine;
        [SerializeField] private Text targetLabel;
        //[SerializeField] private Color targetColor;

        [SerializeField, Header("Chart Item")] private Transform parentLabels, labelRef, parentBars;
        [SerializeField] private Image barRef;
        //[SerializeField] private Color primaryBarColor, secondaryBarColor;

        [SerializeField, Header("Step marker")] private Transform stepsParent;

        private readonly List<Text> stepsLabel = new();
        private readonly Dictionary<string, KeyValuePair<Transform, Image>> graphLabelPair = new();

        private void Awake() {
            foreach (Transform step in stepsParent) {
                stepsLabel.Add(step.Find("Label").GetComponent<Text>());
            }
            stepsLabel.Reverse();
        }

        public void Create(string _targetLabel, ulong _targetValue, IEnumerable<DataChart> data) {
            ClearChildren(parentBars);
            ClearChildren(parentLabels);
            graphLabelPair.Clear();

            //  To earn highest value
            ulong highestValue = _targetValue;
            foreach (var datum in data) {
                if (highestValue < datum.primaryValue) highestValue = datum.primaryValue;
                if (highestValue < datum.secondaryValue) highestValue = datum.secondaryValue;
            }

            highestValue = (highestValue > 0 ? highestValue : highestValue + 1) * (uint)Random.Range(105, 140) / 100;
            //  To earn highest value

            //  Value per steps
            byte a = 1;
            foreach (var stepLabel in stepsLabel) {
                if (highestValue >= 1_000_000_000) {
                    //stepLabel.text = stepLabel.text.Substring(0, stepLabel.text.Length - 8) + "m";
                    highestValue /= 1_000_000;
                } else
                if (highestValue >= 1_000_000) {
                    highestValue /= 1_000;
                    //stepLabel.text = stepLabel.text.Substring(0, stepLabel.text.Length - 4) + "k";
                }
                stepLabel.text = Tools.ToMoney("" + (a * (highestValue / (uint)stepsLabel.Count)), false, false);
                a++;
            }
            //  Value per steps

            //  Target line
            targetLabel.text = _targetLabel;
            float percentage = (float)_targetValue / highestValue;
            targetLine.anchorMin = new(targetLine.anchorMin.x, percentage);
            targetLine.anchorMax = new(targetLine.anchorMax.x, percentage);
            targetLine.pivot = new(targetLine.pivot.x, percentage);
            //  Target line

            //  Data charts
            foreach (var datum in data) {
                var item = CreateItem(datum.label);
                
                //  Primary value
                item.Value.transform.Find("Fill").GetComponent<Image>().fillAmount = (float)datum.primaryValue / highestValue;
                item.Value.transform.Find("Value1").GetComponent<Text>().text = Tools.ToMoney("" + datum.primaryValue, false, false);

                //  Secondary value
                item.Value.fillAmount = (float)datum.secondaryValue / highestValue;
                var value2 = item.Value.transform.Find("Value2").GetComponent<Text>();
                value2.text = Tools.ToMoney("" + datum.secondaryValue, false, false);
                if (datum.secondaryValue <= 0) Destroy(value2.gameObject);
                else {
                    var value2Rect = value2.GetComponent<RectTransform>();
                    value2Rect.anchorMin = new(value2Rect.anchorMin.x, item.Value.fillAmount);
                    value2Rect.anchorMax = new(value2Rect.anchorMax.x, item.Value.fillAmount);
                    value2Rect.pivot = new(value2Rect.pivot.x, item.Value.fillAmount);
                }

                graphLabelPair.Add(datum.key, item);
            }
            //  Data charts
        }

        private KeyValuePair<Transform, Image> CreateItem(string labelName) {
            var newItemBar = CreateObject(barRef, parentBars);
            newItemBar.name = labelName;

            var newLabelText = CreateObject(labelRef, parentLabels);
            newLabelText.name = labelName;
            newLabelText.GetComponentInChildren<Text>().text = labelName;

            return new KeyValuePair<Transform, Image>(newLabelText, newItemBar);
        }
    }
}
