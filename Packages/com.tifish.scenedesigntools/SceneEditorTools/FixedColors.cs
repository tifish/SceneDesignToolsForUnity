using System;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SceneDesignTools
{
    public enum FixedColor
    {
        Red,
        Green,
        Blue,
        White,
        Black,
        Yellow,
        Cyan,
        Magenta,
        Gray,
    }

    public static class FixedColors
    {
        public static readonly string[] Names = Enum.GetNames(typeof(FixedColor));
        public static readonly int Count = Names.Length;

        public static Color GetRealColor(FixedColor color)
        {
            switch (color)
            {
                case FixedColor.Red:
                    return Color.red;
                case FixedColor.Green:
                    return Color.green;
                case FixedColor.Blue:
                    return Color.blue;
                case FixedColor.White:
                    return Color.white;
                case FixedColor.Black:
                    return Color.black;
                case FixedColor.Yellow:
                    return Color.yellow;
                case FixedColor.Cyan:
                    return Color.cyan;
                case FixedColor.Magenta:
                    return Color.magenta;
                case FixedColor.Gray:
                    return Color.gray;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

#if UNITY_EDITOR
        public static void DrawColorProperty(SerializedProperty colorProp)
        {
            GUILayout.Label(Strings.Color);

            var oldColor = GUI.backgroundColor;

            for (var i = 0; i < Names.Length; i++)
            {
                if (i % 3 == 0)
                    EditorGUILayout.BeginHorizontal();

                GUI.backgroundColor = GetRealColor((FixedColor)i);

                EditorGUI.BeginChangeCheck();

                var selected = i == colorProp.enumValueIndex;
                selected = GUILayout.Toggle(selected, selected ? "O" : "x", "Button");

                if (EditorGUI.EndChangeCheck())
                    if (selected)
                        colorProp.enumValueIndex = i;

                GUI.backgroundColor = oldColor;

                if (i % 3 == 2)
                    EditorGUILayout.EndHorizontal();
            }

            if ((Names.Length - 1) % 3 != 2)
                EditorGUILayout.EndHorizontal();
        }

        public static void DrawColorFilter(BitArray filterColors, string saveKey)
        {
            GUILayout.Label(Strings.VisibleColors);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(Strings.ShowAll))
            {
                ShowPoint.VisibleColors.SetAll(true);
                SaveVisibleColors(saveKey);
            }

            if (GUILayout.Button(Strings.HideAll))
            {
                ShowPoint.VisibleColors.SetAll(false);
                SaveVisibleColors(saveKey);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            var oldColor = GUI.backgroundColor;

            EditorGUI.BeginChangeCheck();

            for (var i = 0; i < Names.Length; i++)
            {
                if (i % 3 == 0)
                    EditorGUILayout.BeginHorizontal();

                GUI.backgroundColor = GetRealColor((FixedColor)i);
                filterColors[i] = GUILayout.Toggle(
                    filterColors[i], filterColors[i] ? "O" : "x", "Button");
                GUI.backgroundColor = oldColor;

                if (i % 3 == 2)
                    EditorGUILayout.EndHorizontal();
            }

            if ((Names.Length - 1) % 3 != 2)
                EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
                SaveVisibleColors(saveKey);
        }

        private static void SaveVisibleColors(string saveKey)
        {
            var intArray = new int[1];
            ShowPoint.VisibleColors.CopyTo(intArray, 0);
            PlayerPrefs.SetInt(ShowPoint.VisibleColorsKey, intArray[0]);

            SceneView.RepaintAll();
        }

#endif
    }
}
