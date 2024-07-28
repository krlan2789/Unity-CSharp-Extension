using UnityEngine;

namespace LAN {

    public class ErrorHandler : MonoBehaviour {

        #region Static instance of this class

        private static ErrorHandler instance;

        public static ErrorHandler Instance {
            get {
                instance = (instance == null) ? FindObjectOfType<ErrorHandler>() : instance;
                DontDestroyOnLoad(instance.gameObject);
                return instance;
            }
        }

        #endregion Static instance of this class

        private static string errorLogLoc = "";
        private static readonly string baseLogName = "";

        public static void Record(string url, string errorCode, string errorMessage, string sceneName, string callerObject) {
            string operatingSystem = SystemInfo.operatingSystem;
#if UNITY_EDITOR
            string version = Application.unityVersion;
#else
            string version = Application.version;
#endif
            string logFormat = string.Format("{3}/{4} | {1}:{2} | url:\"{0}\"\n", url.Replace(".", "*").Replace(":", "~"), errorMessage, errorCode, operatingSystem, "AppVersion." + version);
            Debug.Log(sceneName + "/" + callerObject + " | " + logFormat);
        }
    }
}