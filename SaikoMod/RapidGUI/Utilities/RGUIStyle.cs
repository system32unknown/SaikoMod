using UnityEngine;

namespace RapidGUI {
    public static class RGUIStyle {
        public static GUIStyle centerLabel;

        public static GUIStyle button;
        public static GUIStyle flatButton;
        public static GUIStyle popupFlatButton;
        public static GUIStyle popup;
        public static GUIStyle darkWindow;
        public static GUIStyle alignLeftBox;

        public static GUIStyle warningLabel;
        public static GUIStyle warningLabelNoStyle;

        // GUIStyleState.background will be null 
        // if it set after secound scene load and don't use a few frame
        // to keep textures, set it to other member. at unity2019
        public static Texture2D flatButtonTex;
        public static Texture2D popupTex;
        public static Texture2D darkWindowTexNormal;
        public static Texture2D darkWindowTexOnNormal;

        static RGUIStyle() {
            CreateStyles();
        }

        public static void CreateStyles() {
            CreateButton();
            CreateFlatButton();
            CreatePopupFlatButton();
            CreatePopup();
            CreateDarkWindow();
            CreateAlignLeftBox();
            CreateWarningLabel();
            CreateWarningLabelNoStyle();
            CreateCenterLabel();
        }

        static void CreateCenterLabel() {
            centerLabel = new GUIStyle(GUI.skin.label) {
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = 21f
            };
        }

        static void CreateButton() {
            button = new GUIStyle(GUI.skin.button) {
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = 21f
            };
        }

        static void CreateFlatButton() {
            GUIStyle style = new GUIStyle(GUI.skin.label) {
                wordWrap = false,
                alignment = TextAnchor.MiddleCenter
            };

            GUIStyle toggle = GUI.skin.toggle;
            style.normal.textColor = toggle.normal.textColor;
            style.hover.textColor = toggle.hover.textColor;

            flatButtonTex = new Texture2D(1, 1);
            flatButtonTex.SetPixels(new[] { new Color(0.5f, 0.5f, 0.5f, 0.5f) });
            flatButtonTex.Apply();
            style.hover.background = flatButtonTex;

            style.name = nameof(flatButton);
            flatButton = style;
        }

        static void CreatePopupFlatButton() {
            popupFlatButton = new GUIStyle(flatButton) {
                alignment = GUI.skin.label.alignment,
                padding = new RectOffset(24, 48, 2, 2),
                name = nameof(popupFlatButton)
            };
        }

        static void CreatePopup() {
            GUIStyle style = new GUIStyle(GUI.skin.box) {
                border = new RectOffset()
            };

            popupTex = new Texture2D(1, 1);
            float brightness = 0.2f;
            float alpha = 0.9f;
            popupTex.SetPixels(new[] { new Color(brightness, brightness, brightness, alpha) });
            popupTex.Apply();

            style.normal.background = style.hover.background = popupTex;
            style.name = nameof(popup);
            popup = style;
        }


        public static void CreateDarkWindow() {
            GUIStyle style = new GUIStyle(GUI.skin.window);
            style.normal.background = darkWindowTexNormal = CreateTexDark(style.normal.background, 0.5f, 1.4f);
            style.onNormal.background = darkWindowTexOnNormal = CreateTexDark(style.onNormal.background, 0.6f, 1.5f);
            style.name = nameof(darkWindow);
            darkWindow = style;
        }

        public static void CreateAlignLeftBox() {
            alignLeftBox = new GUIStyle(GUI.skin.box) {
                alignment = TextAnchor.MiddleCenter,
                name = nameof(alignLeftBox),
                fixedHeight = 21f,
                fixedWidth = 260f
            };
        }

        public static Texture2D CreateTexDark(Texture2D src, float colorRate, float alphaRate) {
            // copy texture trick.
            // Graphics.CopyTexture(src, dst) must same format src and dst.
            // but src format can't call GetPixels().
            RenderTexture tmp = RenderTexture.GetTemporary(src.width, src.height);
            Graphics.Blit(src, tmp);

            RenderTexture prev = RenderTexture.active;
            RenderTexture.active = prev;

            Texture2D dst = new Texture2D(src.width, src.height, TextureFormat.RGBA32, false);
            dst.ReadPixels(new Rect(0f, 0f, src.width, src.height), 0, 0);

            RenderTexture.active = prev;
            RenderTexture.ReleaseTemporary(tmp);

            Color[] pixels = dst.GetPixels();
            for (int i = 0; i < pixels.Length; ++i) {
                Color col = pixels[i];
                col.r *= colorRate;
                col.g *= colorRate;
                col.b *= colorRate;
                col.a *= alphaRate;

                pixels[i] = col;
            }

            dst.SetPixels(pixels);
            dst.Apply();

            return dst;
        }

        static void CreateWarningLabel() {
            warningLabel = new GUIStyle(GUI.skin.box) {
                alignment = GUI.skin.label.alignment,
                richText = true,
                name = nameof(warningLabel)
            };
        }

        static void CreateWarningLabelNoStyle() {
            warningLabelNoStyle = new GUIStyle(GUI.skin.label) {
                richText = true,
                name = nameof(warningLabelNoStyle)
            };
        }

        public static readonly Texture2D white = new SaikoMod.Utils.ConverterUtils.Texture(Color.white).tex;
    }
}