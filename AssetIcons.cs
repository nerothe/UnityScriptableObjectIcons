﻿using System.Collections;
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
public class AssetIcons : Editor {

    static bool enabled = false;
    public static Color backgroundColor = new Color(82f / 255f,82f / 255f, 82f / 255f, 1f);
    static Texture2D bg;

    [ExecuteInEditMode]
    private void OnEnable()
    {
        EnableIcons();
    }
    

    [MenuItem("Assets/Icons/Enable")]
    static void EnableIcons()
    {
        enabled = true;
        EditorApplication.projectWindowItemOnGUI -= AssetIcons.MyCallback();
        EditorApplication.projectWindowItemOnGUI += AssetIcons.MyCallback();
    }

    [MenuItem("Assets/Icons/Enable", true)]
    private static bool EnableMenuOptionValidation()
    {
        return !enabled;
    }

    [MenuItem("Assets/Icons/Disable")]
    static void DisableIcons()
    {
        enabled = false;
        EditorApplication.projectWindowItemOnGUI -= AssetIcons.MyCallback();
    }

    [MenuItem("Assets/Icons/Disable", true)]
    private static bool DisableMenuOptionValidation()
    {
        return enabled;
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

            Color[] colors = new Color[32 * 32];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = backgroundColor;
            }
            bg.SetPixels(colors);
        }


        var t = AssetDatabase.LoadAssetAtPath(guid, typeof(object)) as object;
        if (t == null || t.GetType() == null) return;
        var fields = t.GetType().GetFields();

        var atts = fields.Where(fi => ((fi == null) ? 0 : fi.GetCustomAttributes(typeof(ScriptableObjectIcon), false).Count()) > 0);

        if (atts.Count() == 1)
        {
            Rect r2 = new Rect(r);
            r2.height -= 14;
            GUI.DrawTexture(r2, bg, ScaleMode.StretchToFill, false);
            GUI.DrawTexture(r2, bg, ScaleMode.StretchToFill, true, 0, backgroundColor, 2, 3);
            // GUI.DrawTexture(r2, bg);


            Sprite sprite = (Sprite)atts.First().GetValue(t);
            if (sprite == null)
                return;
            r.yMin += 5;
            r.height -= 22;
            GUI.DrawTexture(r, sprite.texture, ScaleMode.ScaleToFit);
        }
    }
}