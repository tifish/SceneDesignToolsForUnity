using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SceneDesignTools
{
    public class PlaceObjectByClick : BaseSceneDesignTool
    {
        public PlaceObjectByClick(SceneDesignToolsWindow ownerWindow) : base(ownerWindow)
        {
        }

        private bool _isPlacingObject;

        public override void OnGUI()
        {
            GUI.enabled = OwnerWindow.HasSelection;
            
            EditorGUI.BeginChangeCheck();
            _isPlacingObject = GUILayout.Toggle(_isPlacingObject, Strings.PlaceSelectedObjectsByClick, GUI.skin.button);
            if (EditorGUI.EndChangeCheck())
                if (SceneView.lastActiveSceneView)
                    SceneView.lastActiveSceneView.Repaint();
            
            GUI.enabled = true;
        }

        public override void OnSceneGUI(SceneView sceneView)
        {
            if (!_isPlacingObject)
                return;

            // 屏蔽鼠标选中物体
            if (Event.current.type == EventType.Layout)
                HandleUtility.AddDefaultControl(0);

            if (Event.current.type != EventType.MouseDown || Event.current.button != 0)
                return;

            var worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (!Physics.Raycast(worldRay, out var hitInfo, int.MaxValue))
                return;

            foreach (var go in Selection.gameObjects)
            {
                PlaceObjectTo(go, hitInfo.point);
                EditorSceneManager.MarkSceneDirty(go.scene);
            }

            _isPlacingObject = false;

            if (SceneView.lastActiveSceneView != null)
                SceneView.lastActiveSceneView.Repaint();

            if (OwnerWindow)
                OwnerWindow.Repaint();
        }

        private static void PlaceObjectTo(GameObject go, in Vector3 newPosition)
        {
            var position = go.transform.position;
            var bottom = position;
            bottom.y -= go.transform.localScale.y * 0.5f;

            position -= bottom - newPosition;
            go.transform.position = position;
        }
    }
}
