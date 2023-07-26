using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    public class ColliderTool : BaseSceneDesignTool
    {
        public ColliderTool(SceneDesignToolsWindow ownerWindow) : base(ownerWindow)
        {
        }

        private static bool _expanded = true;

        public override void OnGUI()
        {
            _expanded = EditorGUILayout.Foldout(_expanded, Strings.ColliderRenderer, EditorGUIHelper.FoldoutStyle);
            if (_expanded)
            {
                GUI.enabled = OwnerWindow.HasSelection;

                EditorGUIHelper.BeginIndent();

                if (GUILayout.Button(Strings.AddColliderRenderer))
                    foreach (var go in Selection.gameObjects)
                        AddColliderRenderer(go);

                if (GUILayout.Button(Strings.RemoveColliderRenderer))
                    foreach (var go in Selection.gameObjects)
                        RemoveColliderRenderer(go);

                GUI.enabled = true;

                FixedColors.DrawColorFilter(ColliderRenderer.VisibleColors, ColliderRenderer.VisibleColorsKey);

                EditorGUIHelper.EndIndent();
            }

            EditorGUIHelper.SeparatorLine();
        }

        private static void AddColliderRenderer(GameObject go)
        {
            if (!go.GetComponent<ColliderRenderer>())
                go.AddComponent<ColliderRenderer>();
        }

        private static void RemoveColliderRenderer(GameObject go)
        {
            var colliderRenderer = go.GetComponent<ColliderRenderer>();
            if (colliderRenderer)
                Object.DestroyImmediate(colliderRenderer);
        }
    }
}
