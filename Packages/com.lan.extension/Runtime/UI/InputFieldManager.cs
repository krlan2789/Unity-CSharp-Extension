using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LAN.UI {

    public class InputFieldManager : MonoBehaviour {

        [Header("List InputField (TMPro)")]
        public List<TMP_InputField> inputFields = new List<TMP_InputField>();

        private void Awake() {
        }
    }
}