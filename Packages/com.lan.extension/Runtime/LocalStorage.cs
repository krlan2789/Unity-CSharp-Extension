using LAN.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;

namespace LAN {
    public class LocalStorage : CustomBehaviour {
        #region Static attribute from this class
        private static LocalStorage instance;
        public static LocalStorage Instance {
            get {
                instance = instance == null ? FindObjectOfType<LocalStorage>() : instance;
                return instance;
            }
        }
        #endregion

        private void Start() {
            bool hasPermission = Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite);
            if (hasPermission) {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }
        }

        #region Basic CRUD to persistend data path
        /// <summary>
        /// Get local path of fileName from persistent data path
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetLocalPath(string fileName) {
            string path = Application.persistentDataPath + "/cookies/";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            //return path + DataSecure.Encode(fileName) + ".r";
            return path + (string.IsNullOrEmpty(fileName) ? "" : DataSecure.Encode(fileName) + ".r");
        }

        /// <summary>
        /// Get local path of fileName from persistent data path with custom directory
        /// </summary>
        /// <param name="directory">Additional directory</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetLocalPath(string directory, string fileName) {
            string path = Application.persistentDataPath + "/cookies/" + directory.Trim('/') + "/";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var _path = path + (string.IsNullOrEmpty(fileName) ? "" : DataSecure.Encode(fileName) + ".r");
            //Debug.Log("GetLocalPath: " + _path);
            return _path;
        }

        /// <summary>
        /// Delete one or all files in Application.persistentDataPath + /cookies/
        /// </summary>
        /// <param name="filename">Fill this param for deleting spesific file</param>
        public static void Clear(string filename = null, bool clearFile = true) {
            try {
                if (!string.IsNullOrEmpty(filename) && clearFile) {
                    if (!File.Exists(GetLocalPath(filename))) {
                        Debug.LogWarning("File " + GetLocalPath(filename) + " doesn't exist");
                        return;
                    }
                    File.Delete(GetLocalPath(filename));
                } else {
                    string dir = Application.persistentDataPath + "/cookies/";
                    if (!string.IsNullOrEmpty(filename)) dir += filename + "/";
                    if (!Directory.Exists(Application.persistentDataPath + "/cookies/")) {
                        Debug.LogWarning("Directory " + dir + " doesn't exist");
                        return;
                    }
                    Directory.Delete(dir, true);
                }
            } catch (Exception e) {
                Debug.LogWarning(e.Message);
            }
        }

        /// <summary>
        /// Write data into a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public static void Write(string filePath, string data) {
            try {
                if (!string.IsNullOrEmpty(filePath)) {
                    if (File.Exists(filePath)) File.Delete(filePath);
                    //string dir = string.Join("/", new List<string>(filePath.Replace("\\", "/").Split('/')).GetRange(0, -1).ToArray());
                    //Debug.Log(dir);
                    //if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    File.WriteAllText(filePath, data);
                }
            } catch (Exception e) {
                Debug.LogWarning(e.Message);
            }
        }

        /// <summary>
        /// Read data from a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string Read(string filePath) {
            if (!File.Exists(filePath)) return null;
            return File.ReadAllText(filePath);
        }
        #endregion

        #region Save in secure mode
        /// <summary>
        /// Save data string to local in secure mode
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Save(string key, string value) {
            Write(GetLocalPath(key), DataSecure.Encode(value));
        }

        /// <summary>
        /// Save data int to local in secure mode
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Save(string key, int value) {
            Write(GetLocalPath(key), DataSecure.Encode(value.ToString()));
        }

        /// <summary>
        /// Save data long to local in secure mode
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Save(string key, long value) {
            Write(GetLocalPath(key), DataSecure.Encode(value.ToString()));
        }

        /// <summary>
        /// Save data float to local in secure mode
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Save(string key, float value) {
            Write(GetLocalPath(key), DataSecure.Encode(value.ToString()));
        }
        #endregion

        #region Save in secure mode with custom directory
        /// <summary>
        /// Save data string to local with custom directory in secure mode
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Save(string directory, string key, string value) {
            Write(GetLocalPath(directory, key), DataSecure.Encode(value));
        }

        /// <summary>
        /// Save data long to local with custom directory in secure mode
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Save(string directory, string key, long value) {
            Write(GetLocalPath(directory, key), DataSecure.Encode(value.ToString()));
        }

        /// <summary>
        /// Save data int to local with custom directory in secure mode
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Save(string directory, string key, int value) {
            Write(GetLocalPath(directory, key), DataSecure.Encode(value.ToString()));
        }

        /// <summary>
        /// Save data float to local with custom directory in secure mode
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Save(string directory, string key, float value) {
            Write(GetLocalPath(directory, key), DataSecure.Encode(value.ToString()));
        }
        #endregion

        #region Load in secure mode
        /// <summary>
        /// Load data string from local in secure mode
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue">return this value if key not found</param>
        /// <returns></returns>
        public static string Load(string key, string defaultValue) {
            if (File.Exists(GetLocalPath(key))) return DataSecure.Decode(Read(GetLocalPath(key)));
            else return defaultValue;
        }

        /// <summary>
        /// Load data int from local in secure mode
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue">return this value if key not found</param>
        /// <returns></returns>
        public static int Load(string key, int defaultValue) {
            if (File.Exists(GetLocalPath(key))) return Convert.ToInt32(DataSecure.Decode(Read(GetLocalPath(key))));
            else return defaultValue;
        }

        /// <summary>
        /// Load data long from local in secure mode
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue">return this value if key not found</param>
        /// <returns></returns>
        public static long Load(string key, long defaultValue) {
            if (File.Exists(GetLocalPath(key))) return Convert.ToInt64(DataSecure.Decode(Read(GetLocalPath(key))));
            else return defaultValue;
        }

        /// <summary>
        /// Load data float from local in secure mode
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue">return this value if key not found</param>
        /// <returns></returns>
        public static float Load(string key, float defaultValue) {
            if (File.Exists(GetLocalPath(key))) return (float)Convert.ToDouble(DataSecure.Decode(Read(GetLocalPath(key))));
            else return defaultValue;
        }
        #endregion

        #region Load in secure mode with custom directory
        /// <summary>
        /// Load data string from local in secure mode with custom directory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue">return this value if key not found</param>
        /// <returns></returns>
        public static string Load(string directory, string key, string defaultValue) {
            if (File.Exists(GetLocalPath(directory, key))) return DataSecure.Decode(Read(GetLocalPath(directory, key)));
            else return defaultValue;
        }

        /// <summary>
        /// Load data int from local in secure mode with custom directory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue">return this value if key not found</param>
        /// <returns></returns>
        public static int Load(string directory, string key, int defaultValue) {
            if (File.Exists(GetLocalPath(directory, key))) return Convert.ToInt32(DataSecure.Decode(Read(GetLocalPath(directory, key))));
            else return defaultValue;
        }

        /// <summary>
        /// Load data long from local in secure mode with custom directory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue">return this value if key not found</param>
        /// <returns></returns>
        public static long Load(string directory, string key, long defaultValue) {
            if (File.Exists(GetLocalPath(directory, key))) return Convert.ToInt64(DataSecure.Decode(Read(GetLocalPath(directory, key))));
            else return defaultValue;
        }

        /// <summary>
        /// Load data float from local in secure mode with custom directory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue">return this value if key not found</param>
        /// <returns></returns>
        public static float Load(string directory, string key, float defaultValue) {
            if (File.Exists(GetLocalPath(directory, key))) return (float)Convert.ToDouble(DataSecure.Decode(Read(GetLocalPath(directory, key))));
            else return defaultValue;
        }
        #endregion

        #region Load all file on secure mode inside custom directory
        /// <summary>
        /// Load all data string from local in secure mode with custom directory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="getValue"></param>
        /// <returns></returns>
        public static string[] LoadAll(string directory, bool getValue = true) {
            string path = GetLocalPath(directory, "");
            Debug.Log(path);
            List<string> files = new();
            if (Directory.Exists(path)) {
                foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)) {
                    Debug.Log(file);
                    files.Add(getValue ? DataSecure.Decode(Read(file)) : file);
                    //Debug.Log(file);
                    //Debug.Log(Read(file));
                    //Debug.Log(DataSecure.Decode(Read(file)));
                }
            }
            return files.ToArray(); ;
        }

        /// <summary>
        /// Load all directory from local in secure mode with custom directory
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string[] LoadAllDir(string directory) {
            string path = GetLocalPath(directory, "");
            //Debug.Log(path);
            List<string> files = new List<string>();
            if (Directory.Exists(path)) {
                foreach (var dir in Directory.GetDirectories(path)) {
                    //Debug.Log(dir);
                    files.Add(dir);
                }
            }
            return files.ToArray(); ;
        }
        #endregion
    }
}
