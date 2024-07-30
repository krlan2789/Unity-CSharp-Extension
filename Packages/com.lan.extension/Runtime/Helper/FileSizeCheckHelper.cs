using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace LAN.Helper
{
    public class FileSizeCheckHelper : MonoBehaviour
    {
        private static FileSizeCheckHelper instance;
        public static FileSizeCheckHelper Instance
        {
            get
            {
                instance ??= FindObjectOfType<FileSizeCheckHelper>();
                if (instance == null)
                {
                    instance = new GameObject("FileSizeCheckHelper").AddComponent<FileSizeCheckHelper>();
                }
                return instance;
            }
        }

        private UnityWebRequest req;

        public float Progress { private set; get; }
        public double SizeByte { private set; get; }
        public double SizeKB { get { return SizeByte / 1_000; } }
        public double SizeMB { get { return SizeByte / 1_000_000; } }

        public Coroutine GetSize(string filePath)
        {
            SizeByte = 0;
            Progress = 0;
            return StartCoroutine(GettingSize(new string[] { filePath }));
        }

        public Coroutine GetSize(string[] filesPath)
        {
            SizeByte = 0;
            Progress = 0;
            return StartCoroutine(GettingSize(filesPath));
        }

        private IEnumerator GettingSize(string[] filePath)
        {
            for (int a = 0; a < filePath.Length; a++)
            {
                req = UnityWebRequest.Head(filePath[a]);
                yield return req.SendWebRequest();

                Debug.Log("Size:" + req.GetResponseHeader("Content-Length"));
                string contentLength = req.GetResponseHeader("Content-Length");
                SizeByte += double.Parse(contentLength);
                yield return null;
                Progress = (float)a / filePath.Length;
            }

            if (req != null) req.Dispose();
            yield return null;
            Progress = 1;
        }
    }
}
