using System.Collections.Generic;
using UnityEngine;

namespace LAN.Scene {
    public class SceneStack {
        private static Stack<string> scenesStack = new Stack<string>();

        public static void Clear() {
            Debug.Log("scenesStack.Clear()");
            scenesStack.Clear();
        }

        public static string[] GetList() {
            return scenesStack.ToArray();
        }

        public static string PeekLatest() {
            if (scenesStack.Count > 0) return scenesStack.Peek();
            else return "";
        }

        public static string GetLatest() {
            if (scenesStack.Count > 0) return scenesStack.Pop();
            else return "";
        }

        public static void SetLatest(string sceneName) {
            scenesStack.Push(sceneName);
        }
    }
}