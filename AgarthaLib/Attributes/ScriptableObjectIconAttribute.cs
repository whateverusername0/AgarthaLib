using System.Linq;
using UnityEngine;
using UnityEditor;
using System;

namespace AgarthaLib.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ScriptableObjectIconAttribute : PropertyAttribute {}

#if UNITY_EDITOR
    [ExecuteInEditMode, InitializeOnLoad]
    public class ScriptableObjectIconDrawer : Editor
    {
        private static bool _unityDeafaultOnNull;
        public static bool UnityDeafaultOnNull
        {
            get
            {
                return _unityDeafaultOnNull;
            }
            set
            {
                Menu.SetChecked(MENU_NAME_DEFAULTNULL, value);
                EditorPrefs.SetBool(MENU_NAME_DEFAULTNULL, value);
                _unityDeafaultOnNull = value;
            }
        }

        private static bool _disableIcons;
        public static bool DisableIcons
        {
            get
            {
                return _disableIcons;
            }
            set
            {
                _disableIcons = value;
                SetStateDisabled(value);
            }
        }

        public static Color BackgroundColor = new(82f / 255f, 82f / 255f, 82f / 255f, 1f);
        private static Texture2D _background;

        private const string MENU_NAME_DISABLE_ICONS = "Assets / Icons / Disable icons";
        private const string MENU_NAME_DEFAULTNULL = "Assets / Icons / Unity default for none";


        [MenuItem(MENU_NAME_DISABLE_ICONS)]
        private static void ToggleShowIconsAction()
        {
            DisableIcons = !DisableIcons;
        }

        [MenuItem(MENU_NAME_DEFAULTNULL)]
        private static void ToggleDefOnNullAction()
        {
            UnityDeafaultOnNull = !UnityDeafaultOnNull;
        }

        static ScriptableObjectIconDrawer()
        {
            _disableIcons = EditorPrefs.GetBool(MENU_NAME_DISABLE_ICONS, false);
            _unityDeafaultOnNull = EditorPrefs.GetBool(MENU_NAME_DEFAULTNULL, false);

            EditorApplication.delayCall += () =>
            {
                SetStateDisabled(_disableIcons);
                UnityDeafaultOnNull = _unityDeafaultOnNull;
            };
        }

        static void SetStateDisabled(bool value)
        {
            Menu.SetChecked(MENU_NAME_DISABLE_ICONS, value);
            EditorPrefs.SetBool(MENU_NAME_DISABLE_ICONS, value);
            if (value)
            {
                EditorApplication.projectWindowItemOnGUI -= MyCallback();
            }
            else
            {
                EditorApplication.projectWindowItemOnGUI -= MyCallback();
                EditorApplication.projectWindowItemOnGUI += MyCallback();
            }
        }

        static EditorApplication.ProjectWindowItemCallback MyCallback()
        {
            EditorApplication.ProjectWindowItemCallback myCallback = new EditorApplication.ProjectWindowItemCallback(IconGUI);
            return myCallback;
        }

        static void IconGUI(string s, Rect r)
        {
            var guid = AssetDatabase.GUIDToAssetPath(s);

            _background = _background == null ? new Texture2D(32, 32) : _background;

            var t = AssetDatabase.LoadAssetAtPath(guid, typeof(object)) as object;
            if (t == null || t.GetType() == null) return;

            Texture2D texture = null;

            var atts = t.GetType().GetFields().Where(fi => ((fi == null) ? 0 : fi.GetCustomAttributes(typeof(ScriptableObjectIconAttribute), false).Count()) > 0);

            if (atts != null && atts.Count() == 1)
            {
                var obj = atts.First().GetValue(t);
                if (obj == null) return;

                if (obj.GetType() == typeof(Sprite))
                {
                    var sprite = (Sprite) obj;
                    if (sprite != null) texture = sprite.texture;
                }

                if (obj.GetType() == typeof(Texture2D))
                    texture = (Texture2D) obj;
            }
            else return;

            if (texture == null && _unityDeafaultOnNull)
                return;

            Rect r2 = new(r);
            r2.height -= 14;
            GUI.DrawTexture(r2, _background, ScaleMode.StretchToFill, false);
            GUI.DrawTexture(r2, _background, ScaleMode.StretchToFill, true, 0, BackgroundColor, 2, 3);

            r.yMin += 5;
            r.height -= 22;

            if (texture != null) GUI.DrawTexture(r, texture, ScaleMode.ScaleToFit);
        }
    }
#endif
}