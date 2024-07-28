using System;
using System.Collections.Generic;
using UnityEngine;

namespace LAN.Quality {

    [Serializable]
    public struct DisplaySetting {
        public string name;
        public int refreshRate;
        public Vector2Int resolution;
    }

    public class QualityManager : MonoBehaviour {
        public static string displaySettingKey = "LAN.Quality.DisplaySetting.";

        public static IDictionary<string, DisplaySetting> DictDisplaySettings {
            get {
                Dictionary<string, DisplaySetting> setting = new Dictionary<string, DisplaySetting>();
                for (int a = 0; a < LocalStorage.Load(displaySettingKey, 0); a++) {
                    string key = displaySettingKey + (a + 1);
                    string value = LocalStorage.Load(key, null);
                    if (!string.IsNullOrEmpty(value)) {
                        string[] values = value.Split('_');
                        setting.Add(key, new DisplaySetting {
                            name = string.Format("{0}x{1} ({2})", values[0], values[1], values[2]),
                            refreshRate = Convert.ToInt32(values[2]),
                            resolution = new Vector2Int(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]))
                        });
                    }
                }
                return setting;
            }
        }

        private void Awake() {
            Application.targetFrameRate = 60;
            Resolution[] resolutions = Screen.resolutions;

            if (resolutions.Length > 0) {
                int resCount = 0;

                foreach (Resolution res in resolutions) {
                    resCount++;
                    AddDisplaySetting(displaySettingKey + resCount, res);
                }

                LocalStorage.Save(displaySettingKey, resCount);
            }
        }

        /// <summary>
        /// Add new display setting
        /// </summary>
        /// <param name="key"></param>
        /// <param name="res"></param>
        private void AddDisplaySetting(string key, Resolution res) {
            string value = string.Format("{0}_{1}_{2}", res.width, res.height, res.refreshRate);
            LocalStorage.Save(key, value);
        }

        private void SetRefreshRate(int value) {
            Application.targetFrameRate = value;
        }
    }
}