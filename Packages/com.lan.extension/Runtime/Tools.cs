using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace LAN.Helper {
    public static class Tools {
        /// <summary>
        /// Encrypt a text(string) with own created method.
        /// </summary>
        /// <param name="text">Text to be encrypt.</param>
        /// <returns>Encrypted text</returns>
        public static string EncryptText(string text, byte key = 0) {
            string d = ER1.Encrypt(text, key);
            // Debug.Log(d);
            return d;
        }

        /// <summary>
        /// Decrypt encrypted text with own created method.
        /// </summary>
        /// <param name="code">Code to be decrypt.</param>
        /// <returns>Decrypted text</returns>
        public static string DecryptText(string code, byte key = 0) {
            return ER1.Decrypt(code, key);
        }

        public static Sprite[] GetSpritesAtPath(string path) {
            List<Sprite> result = new List<Sprite>();
            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);

            foreach (string fileName in fileEntries) {
                if (fileName.Contains(".meta")) {
                    continue;
                }

                int index = fileName.LastIndexOf("/");
                string localPath = "Assets/" + path;

                if (index > 0) localPath += fileName.Substring(index);

                Debug.Log(localPath);

                Sprite t = Resources.Load<Sprite>(localPath);

                if (t != null) result.Add(t);
            }

            return result.ToArray();
        }

        public static int GetRelativeKeyboardHeight(this RectTransform rectTransform, bool includeInput)
        {
            int keyboardHeight = GetKeyboardHeight(includeInput);
            float screenToRectRatio = Screen.height / rectTransform.rect.height;
            float keyboardHeightRelativeToRect = keyboardHeight / screenToRectRatio;

            return (int)keyboardHeightRelativeToRect;
        }

        private static int GetKeyboardHeight(bool includeInput)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject unityPlayer = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
                    AndroidJavaObject view = unityPlayer.Call<AndroidJavaObject>("getView");
                    AndroidJavaObject dialog = unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");
                    if (view == null || dialog == null)
                        return 0;
                    var decorHeight = 0;
                    if (includeInput)
                    {
                        AndroidJavaObject decorView = dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");
                        if (decorView != null)
                            decorHeight = decorView.Call<int>("getHeight");
                    }
                    using (AndroidJavaObject rect = new AndroidJavaObject("android.graphics.Rect"))
                    {
                        view.Call("getWindowVisibleDisplayFrame", rect);
                        return Screen.height - rect.Call<int>("height") + decorHeight;
                    }
                }
            } else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return (int)TouchScreenKeyboard.area.height;
            } else return 0;
        }

        public static Color32 PaletteMaroon { get { return new Color32(95, 14, 34, 255); } }
        public static Color32 PaletteRed { get { return new Color32(215, 17, 6, 255); } }
        public static Color32 PaletteDark { get { return new Color32(21, 21, 21, 255); } }
    }

    #region IComparer inherited class
    //public class ComparerDateCreateAtEn : IComparer {
    //    //  Sorting Object array
    //    public int Compare(object x, object y) {
    //        return new CaseInsensitiveComparer().Compare(((DateCreateAtEn)x).create_at, ((DateCreateAtEn)y).create_at);
    //    }
    //}

    public static class GenericsTool {
        public static object GetPropValue(object src, string propName) {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
    #endregion
}