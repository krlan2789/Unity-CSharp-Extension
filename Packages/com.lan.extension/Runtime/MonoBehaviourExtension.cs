using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LAN.Extension
{
    public static class MonoBehaviourExtension
    {
        /// <summary>
        /// Delete all child of parent
        /// </summary>
        /// <param name="parent"></param>
        public static void ClearChildren(this MonoBehaviour thisBehaviour, Transform parent)
        {
            if (parent != null)
            {
                foreach (Transform obj in parent)
                {
                    Object.Destroy(obj.gameObject);
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
        public static T FindObject<T>(this MonoBehaviour thisBehaviour, string path, Transform parent = null) where T : Object
        {
            if (parent != null)
            {
                if (parent.Find(path) == null) return null;
                return parent.Find(path).GetComponent<T>();
            } else
            {
                if (thisBehaviour.transform.Find(path) == null) return null;
                return thisBehaviour.transform.Find(path).GetComponent<T>();
            }
        }

        /// <summary>
        /// Creating gameobject from prefab in Resources folder.
        /// </summary>
        /// <param name="prefabPath">Path to prefab</param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject CreateObject(this GameObject thisObj, string prefabPath, Transform parent)
        {
            return Object.Instantiate((GameObject)Resources.Load(prefabPath), parent.position, Quaternion.identity, parent);
        }

        /// <summary>
        /// Creating gameobject from prefab.
        /// </summary>
        /// <param name="prefab">Prefab object</param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject CreateObject(this GameObject prefab, Transform parent)
        {
            return Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);
        }

        /// <summary>
        /// Creating gameobject from prefab.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T CreateObject<T>(this T prefab, Transform parent) where T : Object
        {
            if (parent != null) return Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);
            else return Object.Instantiate(prefab);
        }

        /// <summary>
        /// Creating gameobject from prefab.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <param name="position"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T CreateObject<T>(this T prefab, Vector3 position, Transform parent) where T : Object
        {
            return Object.Instantiate(prefab, position, Quaternion.identity, parent);
        }

        /// <summary>
        /// Fix vertical layout after medify child content
        /// </summary>
        /// <param name="layoutObject"></param>
        /// <param name="fixHeight"></param>
        /// <param name="duration"></param>
        public static void FixVerticalLayout(this MonoBehaviour thisBehaviour, Transform layoutObject, bool fixHeight = true, float duration = .0005f)
        {
            thisBehaviour.StartCoroutine(FixingVerticalLayout(thisBehaviour, layoutObject, fixHeight, duration));
        }

        public static IEnumerator FixingVerticalLayout(this MonoBehaviour thisBehaviour, Transform layoutObject, bool fixHeight, float duration)
        {
            if (layoutObject == null) yield break;
            VerticalLayoutGroup lg = layoutObject.GetComponent<VerticalLayoutGroup>();
            if (fixHeight)
            {
                bool forceHeight = lg.childForceExpandHeight;
                bool controlHeight = lg.childControlHeight;

                if (forceHeight) lg.childForceExpandHeight = !forceHeight;
                if (controlHeight) lg.childControlHeight = !controlHeight;
                yield return new WaitForSecondsRealtime(duration);
                if (forceHeight) lg.childForceExpandHeight = forceHeight;
                if (controlHeight) lg.childControlHeight = controlHeight;
            } else
            {
                bool forceWidth = lg.childForceExpandWidth;
                bool controlWidth = lg.childControlWidth;

                if (forceWidth) lg.childForceExpandWidth = !forceWidth;
                if (controlWidth) lg.childControlWidth = !controlWidth;
                yield return new WaitForSecondsRealtime(duration);
                if (forceWidth) lg.childForceExpandWidth = forceWidth;
                if (controlWidth) lg.childControlWidth = controlWidth;
            }
        }

        public static Coroutine SetMaxSize(this MonoBehaviour thisBehaviour, Component le)
        {
            Transform parent = le.transform.parent;
            if (parent == null) throw new System.NotSupportedException("This instance is root parent");
            Vector2 size = parent.GetComponent<RectTransform>().sizeDelta;
            return thisBehaviour.SetMaxSize(le, size);
        }

        public static Coroutine SetMaxSize(this MonoBehaviour thisBehaviour, Component com, Vector2 size)
        {
            LayoutElement le = com.GetComponent<LayoutElement>();
            if (le == null) throw new System.NotSupportedException("This instance must have LayoutElement component");
            RectTransform rt = le.GetComponent<RectTransform>();
            if (rt == null) throw new System.NotSupportedException("This instance must have RectTransform component");
            return thisBehaviour.StartCoroutine(thisBehaviour.SettingMaxSize(le, rt, size));
        }

        public static IEnumerator SettingMaxSize(this MonoBehaviour thisBehaviour, LayoutElement le, RectTransform rt, Vector2 size)
        {
            //  Set to max width
            if (size.x != 0 && rt.sizeDelta.x > size.x)
            {
                le.preferredWidth = size.x;
            }
            //  Set to max height
            if (size.y != 0 && rt.sizeDelta.y > size.y)
            {
                le.preferredHeight = size.y;
            }

            le.enabled = false;
            yield return new WaitForSeconds(.01f);
            le.enabled = true;
            yield return null;
        }
    }
}
