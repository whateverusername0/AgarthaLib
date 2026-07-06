using System.Linq;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using AgarthaLib.Data;
using UnityEngine.Tilemaps;


#if UNITY_EDITOR
using UnityEditor;
#endif

#if USING_TILEMAP_EXTRAS
using UnityEngine.Tilemaps;
#endif

namespace AgarthaLib.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ScriptableObjectIconAttribute : PropertyAttribute
    {
        public ConstColor BackgroundColor { get; set; } = ConstColor.white;

        public ScriptableObjectIconAttribute() { }
        public ScriptableObjectIconAttribute(ConstColor backgroundColor)
            => BackgroundColor = backgroundColor;
    }

    #if UNITY_EDITOR
    [ExecuteInEditMode, InitializeOnLoad]
    public class ScriptableObjectIconDrawer : Editor
    {
        private static bool _unityDeafaultOnNull;
        public static bool UnityDeafaultOnNull
        {
            get => _unityDeafaultOnNull;
            set
            {
                Menu.SetChecked(MN_DEFAULT_NULL, value);
                EditorPrefs.SetBool(MN_DEFAULT_NULL, value);
                _unityDeafaultOnNull = value;
            }
        }

        private static bool _disableIcons;
        public static bool DisableIcons
        {
            get => _disableIcons;
            set
            {
                _disableIcons = value;
                SetStateDisabled(value);
            }
        }

        public static Texture2D _background = new(1, 1);

        private const string MN_TOGGLE_ICONS = "Assets / Icons / Enable | disable icons";
        private const string MN_DEFAULT_NULL = "Assets / Icons / Toggle unity default for no icon";


        [MenuItem(MN_TOGGLE_ICONS)]
        private static void ToggleShowIconsAction()
        {
            DisableIcons = !DisableIcons;
        }

        [MenuItem(MN_DEFAULT_NULL)]
        private static void ToggleDefOnNullAction()
        {
            UnityDeafaultOnNull = !UnityDeafaultOnNull;
        }

        static ScriptableObjectIconDrawer()
        {
            _disableIcons = EditorPrefs.GetBool(MN_TOGGLE_ICONS, false);
            _unityDeafaultOnNull = EditorPrefs.GetBool(MN_DEFAULT_NULL, false);

            EditorApplication.delayCall += () =>
            {
                SetStateDisabled(_disableIcons);
                UnityDeafaultOnNull = _unityDeafaultOnNull;
            };
        }

        static void SetStateDisabled(bool value)
        {
            Menu.SetChecked(MN_TOGGLE_ICONS, value);
            EditorPrefs.SetBool(MN_TOGGLE_ICONS, value);
            if (value)
            {
                EditorApplication.projectWindowItemOnGUI -= Callback();
            }
            else
            {
                EditorApplication.projectWindowItemOnGUI -= Callback();
                EditorApplication.projectWindowItemOnGUI += Callback();
            }
        }

        static EditorApplication.ProjectWindowItemCallback Callback()
            => new(DrawIcon);

        static void DrawIcon(string guidString, Rect rect)
        {
            var guidReal = AssetDatabase.GUIDToAssetPath(guidString);

            if (AssetDatabase.LoadAssetAtPath(guidReal, typeof(object)) is not object asset
            || asset.GetType() == null)
                return;

            var dict = new Dictionary<PropertyInfo, ScriptableObjectIconAttribute>();
            foreach (var propInfo in asset.GetType().GetProperties())
            {
                if (propInfo == null) continue;

                var att = propInfo.GetCustomAttribute(typeof(ScriptableObjectIconAttribute), false);
                if (att == null) continue;

                dict.Add(propInfo, att as ScriptableObjectIconAttribute);
            }

            if (dict.Count <= 0)
                return;

            var kvp = dict.FirstOrDefault();
            var @object = kvp.Key.GetValue(asset);
            if (@object == null) return;

            Texture2D texture = null;
            switch (@object)
            {
                case Sprite sprite:
                    if (sprite != null)
                        texture = sprite.texture;
                    break;

                case Texture2D tex2d:
                    texture = tex2d;
                    break;

                case Tile tile:
                    if (tile != null && tile.sprite != null)
                        texture = tile.sprite.texture;
                    break;

                #if USING_TILEMAP_EXTRAS
                case RuleTile ruleTile:
                    if (ruleTile != null && ruleTile.m_DefaultSprite != null)
                        texture = ruleTile.m_DefaultSprite.texture;
                    break;
                #endif

                default: return;
            }

            if ((texture == null && _unityDeafaultOnNull)
            || rect.width >= 640) // some absurd number to prevent it from going insane
                return;

            var color = kvp.Value.BackgroundColor.Resolve();
            rect.height = rect.width;

            if (_background != null)
                GUI.DrawTexture(rect, _background, ScaleMode.StretchToFill, true, 0, color, 0, 0);

            if (texture != null)
                GUI.DrawTexture(rect, texture, ScaleMode.StretchToFill);
        }
    }
    #endif
}