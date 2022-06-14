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
            GUI.enabled = OwnerWindow.HasSelection;

            _expanded = EditorGUILayout.Foldout(_expanded, Strings.ShowPoint);
            if (_expanded)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

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

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            GUI.enabled = true;
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
