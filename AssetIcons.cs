using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class ScriptableObjectIcon : Attribute
{
}

[ExecuteInEditMode]
[InitializeOnLoad]
public class AssetIcons : Editor {

    static bool unityDeafaultOnNull;
    static bool UnityDeafaultOnNull
    {
        get
        {
            return unityDeafaultOnNull;
        }
        set
        {
            Menu.SetChecked(MENU_NAME_DefOnNull, value);
            EditorPrefs.SetBool(MENU_NAME_DefOnNull, value);
            unityDeafaultOnNull = value;
        }
    }

    static bool disableIcons;
    static bool DisableIcons {
        get
        {
            return disableIcons;
        }
        set
        {
            disableIcons = value;
            SetStateDisabled(value);
        }
    }

    public static Color backgroundColor = new Color(82f / 255f,82f / 255f, 82f / 255f, 1f);
    static Texture2D bg;

    private const string MENU_NAME_DISABLE_ICONS = "Assets/Icons/Disable";
    private const string MENU_NAME_DefOnNull = "Assets/Icons/Unity default for Null";


    [MenuItem(MENU_NAME_DISABLE_ICONS)]
    private static void ToggleShowIconsAction()
    {
        DisableIcons = !DisableIcons;
    }

    [MenuItem(MENU_NAME_DefOnNull)]
    private static void ToggleDefOnNullAction()
    {
        UnityDeafaultOnNull = !UnityDeafaultOnNull;
    }

    static AssetIcons()
    {
        disableIcons = EditorPrefs.GetBool(MENU_NAME_DISABLE_ICONS, false);
        unityDeafaultOnNull = EditorPrefs.GetBool(MENU_NAME_DefOnNull, false);

        EditorApplication.delayCall += () => {
            SetStateDisabled(disableIcons);
            UnityDeafaultOnNull = unityDeafaultOnNull;
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

        if (bg == null)
        {
            bg = new Texture2D(32, 32);
        }

        var t = AssetDatabase.LoadAssetAtPath(guid, typeof(object)) as object;
        if (t == null || t.GetType() == null) return;
        
        Texture2D texture = null;
        
        var atts = t.GetType().GetFields().Where(fi => ((fi == null) ? 0 : fi.GetCustomAttributes(typeof(ScriptableObjectIcon), false).Count()) > 0);

        if (atts != null && atts.Count() == 1 )
        {
            object obj = atts.First().GetValue(t);
            if (obj == null)
                return;

            if (obj.GetType() == typeof(Sprite))
            {
                Sprite sprite = (Sprite)obj;
                if (sprite != null)
                {
                    texture = sprite.texture;
                }
            }

            if (obj.GetType() == typeof(Texture2D))
                texture = (Texture2D)obj;
        }
        else
        {
            return;
        }

        if (texture == null && unityDeafaultOnNull)
            return;

        Rect r2 = new Rect(r);
        r2.height -= 14;
        GUI.DrawTexture(r2, bg, ScaleMode.StretchToFill, false);
        GUI.DrawTexture(r2, bg, ScaleMode.StretchToFill, true, 0, backgroundColor, 2, 3);
        
        r.yMin += 5;
        r.height -= 22;
        

        if(texture != null)
            GUI.DrawTexture(r, texture, ScaleMode.ScaleToFit);

    }
}
