using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    public static class EditorGUIHelper
    {
        public static void BeginIndent()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15);
            EditorGUILayout.BeginVertical();
        }

        public static void EndIndent()
        {
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        public static void SeparatorLine()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(2f);

            var rect = EditorGUILayout.GetControlRect(false, 1);
            rect.xMin -= 3;
            rect.xMax += 4;
            rect.height = 1;
            var color = EditorGUIUtility.isProSkin
                ? new Color32(89, 89, 89, 255)
                : new Color32(116, 116, 116, 255);
            EditorGUI.DrawRect(rect, color);

            EditorGUILayout.EndVertical();
        }

        public static GUIStyle FoldoutStyle { get; } =
            new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold };
    }
}
