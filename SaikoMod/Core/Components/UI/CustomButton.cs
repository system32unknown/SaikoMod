using UnityEngine.UI;
using UnityEngine;

namespace SaikoMod.Core.Components.UI {
    public class CustomButton {
        public Text Label;
        public Button button;
        public RectTransform Rect;

        public Vector3 position {
            get {
                return Rect.anchoredPosition;
            }
            set {
                Rect.anchoredPosition = value;
            }
        }

        public CustomButton(Button button) {
            this.button = button;
            this.button.onClick.RemoveAllListeners();
            Rect = this.button.GetComponent<RectTransform>();
            Label = this.button.transform.GetChild(0).GetComponent<Text>();
            Label.resizeTextForBestFit = false;
        }
    }
}
