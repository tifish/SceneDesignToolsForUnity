using SceneDesignTools;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class SceneDesignToolsCustomizeTools
{
    static SceneDesignToolsCustomizeTools()
    {
        CustomizedTool.AddCustomTools(OnGUI);
    }

    private static bool _expanded;

    private static void OnGUI()
    {
        _expanded = EditorGUILayout.Foldout(_expanded, "External Bot", EditorGUIHelper.FoldoutStyle);
        if (_expanded)
        {
            EditorGUIHelper.BeginIndent();

            if (GUILayout.Button("Hello"))
                EditorUtility.DisplayDialog("Hello", "Hello World!", "OK");

            EditorGUIHelper.EndIndent();
        }

        EditorGUIHelper.SeparatorLine();
    }
}
