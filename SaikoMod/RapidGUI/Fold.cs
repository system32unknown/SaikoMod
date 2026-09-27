using UnityEngine;

namespace RapidGUI {
    public class Fold {
        public bool active = false;
        public string text;

        public Fold(string text) => this.text = text;

        public bool DoFold() {
            if (GUILayout.Button("<b><size=14>" + (!active ? "▶" : "▼") + text + "</size></b>", "Label")) active = !active;
            return active;
        }
    }
}