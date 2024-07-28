using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LAN {
    public abstract class ExtendedBehaviour : MonoBehaviour {
        /// <summary>
        /// Delete all child of parent
        /// </summary>
        /// <param name="parent"></param>
        protected void ClearChildren(Transform parent) {
            if (parent != null) {
                foreach (Transform obj in parent) {
                    Destroy(obj.gameObject);
                }
            }
        }

        /// <summary>
        /// Find child from received path inside current gameobject if parent equals null
        /// </summary>
        /// <typeparam name="T">Type of Companent</typeparam>
        /// <param name="path">Path to target gameobject</param>
        /// <param name="parent">Parent of target gameobject</param>
        /// <returns></returns>
        protected T FindObject<T>(string path, Transform parent = null) where T : Object {
            if (parent != null) {
                if (parent.Find(path) == null) return null;
                return parent.Find(path).GetComponent<T>();
            } else {
                if (transform.Find(path) == null) return null;
                return transform.Find(path).GetComponent<T>();
            }
        }

        /// <summary>
        /// Load object inside Resources folder
        /// </summary>
        /// <param name="path">Location target prefab object</param>
        /// <returns></returns>
        protected Object LoadRes(string path) {
            return Resources.Load(path);
        }

        /// <summary>
        /// Load object inside Resources folder using static method
        /// </summary>
        /// <param name="path">Location target prefab object</param>
        /// <returns></returns>
        protected static Object LoadResStatic(string path) {
            return Resources.Load(path);
        }

        /// <summary>
        /// Creating gameobject from prefab in Resources folder.
        /// </summary>
        /// <param name="prefabPath">Path to prefab</param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected GameObject CreateObject(string prefabPath, Transform parent) {
            return Instantiate((GameObject)LoadRes(prefabPath), parent.position, Quaternion.identity, parent);
        }

        /// <summary>
        /// Creating gameobject from prefab.
        /// </summary>
        /// <param name="prefab">Prefab object</param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected GameObject CreateObject(GameObject prefab, Transform parent) {
            return Instantiate(prefab, parent.position, Quaternion.identity, parent);
        }

        /// <summary>
        /// Creating gameobject from prefab.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected T CreateObject<T>(T prefab, Transform parent) where T : Object {
            if (parent != null) return Instantiate(prefab, parent.position, Quaternion.identity, parent);
            else return Instantiate(prefab);
        }

        /// <summary>
        /// Creating gameobject from prefab.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <param name="position"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected T CreateObject<T>(T prefab, Vector3 position, Transform parent) where T : Object {
            return Instantiate(prefab, position, Quaternion.identity, parent);
        }

        /// <summary>
        /// Copy text to clipboard
        /// </summary>
        /// <param name="text"></param>
        protected void CopyToClipboard(string text) {
            TextEditor te = new TextEditor { text = text };
            te.SelectAll();
            te.Copy();
            Debug.Log("Text copied!");
        }

        /// <summary>
        /// Capitalize first letter in each word or first word only
        /// </summary>
        /// <param name="sentence">Sentence source</param>
        /// <param name="allWord">All word / First word only</param>
        /// <param name="sparator">Sparator </param>
        /// <returns></returns>
        protected string Capitalize(string sentence, bool allWord = true, char sparator = ' ') {
            if (string.IsNullOrEmpty(sentence)) return sentence;
            string results = "";
            if (allWord && sentence.Contains(sparator)) {
                string[] words = sentence.Split(sparator);
                foreach (string w in words) {
                    results += w[0].ToString().ToUpper() + w[1..].ToLower() + sparator;
                }
            } else {
                results = sentence[0].ToString().ToUpper() + sentence[1..].ToLower();
            }
            return results.Substring(0, results.Length);
        }

        /// <summary>
        /// Fix vertical layout after medify child content
        /// </summary>
        /// <param name="layoutObject"></param>
        /// <param name="fixHeight"></param>
        /// <param name="duration"></param>
        protected void FixVerticalLayout(Transform layoutObject, bool fixHeight = true, float duration = .0005f) {
            StartCoroutine(FixingVerticalLayout(layoutObject, fixHeight, duration));
        }

        protected IEnumerator FixingVerticalLayout(Transform layoutObject, bool fixHeight, float duration) {
            if (layoutObject == null) yield break;
            VerticalLayoutGroup lg = layoutObject.GetComponent<VerticalLayoutGroup>();
            if (fixHeight) {
                bool forceHeight = lg.childForceExpandHeight;
                bool controlHeight = lg.childControlHeight;

                if (forceHeight) lg.childForceExpandHeight = !forceHeight;
                if (controlHeight) lg.childControlHeight = !controlHeight;
                yield return new WaitForSecondsRealtime(duration);
                if (forceHeight) lg.childForceExpandHeight = forceHeight;
                if (controlHeight) lg.childControlHeight = controlHeight;
            } else {
                bool forceWidth = lg.childForceExpandWidth;
                bool controlWidth = lg.childControlWidth;

                if (forceWidth) lg.childForceExpandWidth = !forceWidth;
                if (controlWidth) lg.childControlWidth = !controlWidth;
                yield return new WaitForSecondsRealtime(duration);
                if (forceWidth) lg.childForceExpandWidth = forceWidth;
                if (controlWidth) lg.childControlWidth = controlWidth;
            }
        }

        public Coroutine SetMaxSize(Component le) {
            Transform parent = le.transform.parent;
            if (parent == null) throw new System.NotSupportedException("This instance is root parent");
            Vector2 size = parent.GetComponent<RectTransform>().sizeDelta;
            return SetMaxSize(le, size);
        }

        public Coroutine SetMaxSize(Component com, Vector2 size) {
            LayoutElement le = com.GetComponent<LayoutElement>();
            if (le == null) throw new System.NotSupportedException("This instance must have LayoutElement component");
            RectTransform rt = le.GetComponent<RectTransform>();
            if (rt == null) throw new System.NotSupportedException("This instance must have RectTransform component");
            return StartCoroutine(SettingMaxSize(le, rt, size));
        }

        private IEnumerator SettingMaxSize(LayoutElement le, RectTransform rt, Vector2 size) {
            //  Set to max width
            if (size.x != 0 && rt.sizeDelta.x > size.x) {
                le.preferredWidth = size.x;
            }
            //  Set to max height
            if (size.y != 0 && rt.sizeDelta.y > size.y) {
                le.preferredHeight = size.y;
            }

            le.enabled = false;
            yield return new WaitForSeconds(.01f);
            le.enabled = true;
            yield return null;
        }
    }
}
