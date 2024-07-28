using NaughtyAttributes;
using UnityEngine;

namespace LAN.UI {
    //public enum StrectEnum {
    //}

    public class StretchManager : MonoBehaviour {

        #region Editor Config

        [SerializeField]
        private bool useEditorConfig = false;

        [SerializeField, ShowIf("useEditorConfig")]
        private RectTransform thisPanel;

        #endregion Editor Config

        private void Start() {
            if (!useEditorConfig) {
                thisPanel = GetComponent<RectTransform>();
            }
            StrectAround();
        }

        public void StrectAround() {
            thisPanel.anchorMax = new Vector2(1, 1);
            thisPanel.anchorMin = new Vector2(0, 0);
            thisPanel.pivot = new Vector2(.5f, .5f);
            thisPanel.offsetMax = new Vector2(0, 0);
            thisPanel.offsetMin = new Vector2(0, 0);
        }
    }
}