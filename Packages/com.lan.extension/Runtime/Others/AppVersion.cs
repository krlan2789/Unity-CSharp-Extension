using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace LAN.UI {
    public class AppVersion : MonoBehaviour {
        [SerializeField]
        private Text text;

        [SerializeField, InfoBox("$VERSION$ will replaced with current app version")]
        private string formatText = "$VERSION$";

        private void Start() {
            if (text == null) text = GetComponent<Text>();
            text.text = formatText.Replace("$VERSION$", Application.version);
        }
    }
}
