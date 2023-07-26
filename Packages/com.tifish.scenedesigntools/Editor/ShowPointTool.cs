using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    public class ShowPointTool : BaseSceneDesignTool
    {
        public ShowPointTool(SceneDesignToolsWindow ownerWindow) : base(ownerWindow)
        {
        }

        private static bool _expanded = true;

        public override void OnGUI()
        {
            _expanded = EditorGUILayout.Foldout(_expanded, Strings.ShowPoint, EditorGUIHelper.FoldoutStyle);
            if (_expanded)
            {
                GUI.enabled = OwnerWindow.HasSelection;

                EditorGUIHelper.BeginIndent();

                if (GUILayout.Button(Strings.AddShowPointTool))
                    foreach (var go in Selection.gameObjects)
                        AddShowPoint(go);

                if (GUILayout.Button(Strings.RemoveShowPointTool))
                    foreach (var go in Selection.gameObjects)
                        RemoveShowPoint(go);

                if (GUILayout.Button(Strings.AddShowPointToolForDirectChildren))
                    foreach (var go in Selection.gameObjects)
                    {
                        var trans = go.transform;
                        for (var i = 0; i < trans.childCount; i++)
                            AddShowPoint(trans.GetChild(i).gameObject);
                    }

                if (GUILayout.Button(Strings.RemoveShowPointToolForDirectChildren))
                    foreach (var go in Selection.gameObjects)
                    {
                        var trans = go.transform;
                        for (var i = 0; i < trans.childCount; i++)
                            RemoveShowPoint(trans.GetChild(i).gameObject);
                    }

                GUI.enabled = true;

                MultipleLimitOnGUI();

                FixedColors.DrawColorFilter(ShowPoint.VisibleColors, ShowPoint.VisibleColorsKey);

                EditorGUIHelper.EndIndent();
            }

            EditorGUIHelper.SeparatorLine();
        }

        private const string ShowPointMultipleLimitKey = "ShowPointMultipleLimit";
        public static int MultipleLimit { get; private set; } = EditorPrefs.GetInt(ShowPointMultipleLimitKey, 50);

        private void MultipleLimitOnGUI()
        {
            EditorGUI.BeginChangeCheck();
            MultipleLimit = EditorGUILayout.IntField(Strings.ShowPointMultipleLimit, MultipleLimit);
            if (EditorGUI.EndChangeCheck())
                EditorPrefs.SetInt(ShowPointMultipleLimitKey, MultipleLimit);
        }

        private static void AddShowPoint(GameObject go)
        {
            if (!go.GetComponent<ShowPoint>())
                go.AddComponent<ShowPoint>();
        }

        private static void RemoveShowPoint(GameObject go)
        {
            var showPoint = go.GetComponent<ShowPoint>();
            if (showPoint)
                Object.DestroyImmediate(showPoint);
        }
    }
}
