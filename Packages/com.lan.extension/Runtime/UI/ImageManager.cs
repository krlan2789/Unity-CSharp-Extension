using System;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LAN {

    public class ImageManager : CustomBehaviour {

        #region Static atribut dari class ini

        private static ImageManager instance;

        public static ImageManager Instance {
            get {
                instance = (instance == null) ? FindObjectOfType<ImageManager>() : instance;
                if (instance == null) instance = new GameObject("ImageManager").AddComponent<ImageManager>();
                return instance;
            }
        }

        #endregion Static atribut dari class ini

        /// <summary>
        /// Load an Image from received image path
        /// </summary>
        /// <param name="imagePath">Path to image location</param>
        /// <param name="image">GameObject to store collected image</param>
        /// <param name="callerObject">Object that call this method</param>
        /// <param name="Callback">Callback will invoke after image collected</param>
        public void LoadImage(string imagePath, Image image, GameObject callerObject, Action Callback = null) {
            try {
                DownloadFile(imagePath, image, Callback);
            } catch (Exception e) {
                ErrorHandler.Record(imagePath, e.Message, "Error When Loading an Image", SceneManager.GetActiveScene().name, callerObject?.name);
            }
        }

        /// <summary>
        /// Download file from received path
        /// </summary>
        /// <param name="imagePath">Path to image location</param>
        /// <param name="image">GameObject to store collected image</param>
        /// <param name="Callback">Callback will invoke after image collected</param>
        private async void DownloadFile(string imagePath, Image image, Action Callback = null) {
            LoadOnProgress();

            try {
                using (WebClient client = new WebClient()) {
                    byte[] imageByte = await client.DownloadDataTaskAsync(imagePath);
                    if (imageByte.Length > 0) {
                        LoadImageFromByteArray(imageByte, image);
                    }
                }
            } catch (Exception e) {
                Debug.Log(e.Message);
            }

            if (image != null && image.gameObject != null) image.gameObject.SetActive(true);
            Callback?.Invoke();
            LoadIsDone();
        }

        /// <summary>
        /// Load Image from byte[]
        /// </summary>
        /// <param name="obj">Image in byte[]</param>
        /// <param name="image">GameObject to store collected image</param>
        public void LoadImageFromByteArray(byte[] obj, Image image) {
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(obj);
            if (image != null) {
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
            }
        }
    }
}