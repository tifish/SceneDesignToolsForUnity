using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    public class StickToGround : BaseSceneDesignTool
    {
        public StickToGround(SceneDesignToolsWindow ownerWindow) : base(ownerWindow)
        {
        }

        public static void StickSelectionToGround()
        {
            foreach (var go in Selection.gameObjects)
                StickGameObjectToGround(go);
        }

        public static void StickGameObjectToGround(GameObject go)
        {
            var currentCenter = go.transform.position;
            if (!Physics.Raycast(currentCenter, Vector3.down, out var hitInfo))
                return;

            var currentBottom = currentCenter;
            currentBottom.y -= go.transform.localScale.y * 0.5f;

            go.transform.position -= currentBottom - hitInfo.point;
        }

        public override void OnGUI()
        {
            GUI.enabled = OwnerWindow.HasSelection;

            if (GUILayout.Button(Strings.StickSelectedObjectsToGround))
                StickSelectionToGround();

            GUI.enabled = true;
        }
    }
}
