using UnityEngine;
using UnityEngine.UI;

namespace LAN.Extension
{
    public static class GameObjectExtension
    {

        public static void SetActive(this Transform go, bool value)
        {
            go.gameObject.SetActive(value);
        }

        /// <summary>
        /// Find child from received path inside current gameobject if parent equals null
        /// </summary>
        /// <typeparam name="T">Type of Companent</typeparam>
        /// <param name="path">Path to target gameobject</param>
        /// <param name="parent">Parent of target gameobject</param>
        /// <returns></returns>
        public static T Find<T>(this Transform transform, string path) where T : UnityEngine.Object
        {
            Transform obj = transform.Find(path);
            if (obj != null) return obj.GetComponent<T>();
            else return null;
        }

        public static void SetLeft(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void SetRight(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void SetTop(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }

        public static Vector2Int GetOriginalSize(this Image img)
        {
            if (img.sprite == null) return new(0, 0);
            int imageWidth = img.sprite.texture.width;
            int imageHeight = img.sprite.texture.height;
            return new(imageWidth, imageHeight);
        }
    }
}
