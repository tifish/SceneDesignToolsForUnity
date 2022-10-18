using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    public class StickToGround : BaseSceneDesignTool
    {
        static StickToGround()
        {
            _raisingHeightBeforeSticking = PlayerPrefs.GetFloat(RaisingHeightBeforeStickingKey, 0);
        }

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
            currentCenter.y += _raisingHeightBeforeSticking;

            while (Physics.Raycast(currentCenter, Vector3.down, out var hitInfo,
                       float.MaxValue, IgnoreLayers.Mask))
            {
                if (hitInfo.transform == go.transform)
                {
                    currentCenter.y -= 0.01f;
                    continue;
                }

                StickToPoint(go.transform, hitInfo.point.y);
                break;
            }
        }

        private static void StickToPoint(Transform movingTransform, float targetY)
        {
            var currentCenter = movingTransform.position;
            var bottomY = currentCenter.y - movingTransform.localScale.y * 0.5f;
            currentCenter.y -= bottomY - targetY;
            movingTransform.position = currentCenter;
        }

        public static void StickSelectionToTerrain()
        {
            foreach (var go in Selection.gameObjects)
                StickGameObjectToTerrain(go);
        }

        public static void StickGameObjectToTerrain(GameObject go)
        {
            var currentCenter = go.transform.position;
            currentCenter.y = 60000;

            while (Physics.Raycast(currentCenter, Vector3.down, out var hitInfo,
                       float.MaxValue, IgnoreLayers.Mask))
            {
                if (!hitInfo.transform.GetComponentInChildren<TerrainCollider>())
                {
                    currentCenter.y = hitInfo.point.y - 0.01f;
                    continue;
                }

                StickToPoint(go.transform, hitInfo.point.y);
                break;
            }
        }

        private const string RaisingHeightBeforeStickingKey = "SceneDesignTools.RaisingHeightBeforeSticking";
        private static float _raisingHeightBeforeSticking;

        public override void OnGUI()
        {
            GUI.enabled = OwnerWindow.HasSelection;

            if (GUILayout.Button(Strings.StickSelectedObjectsToGround))
                StickSelectionToGround();

            EditorGUI.BeginChangeCheck();
            _raisingHeightBeforeSticking = EditorGUILayout.FloatField(
                Strings.RaisingHeightBeforeSticking, _raisingHeightBeforeSticking);
            if (EditorGUI.EndChangeCheck())
                PlayerPrefs.SetFloat(RaisingHeightBeforeStickingKey, _raisingHeightBeforeSticking);

            if (GUILayout.Button(Strings.StickSelectedObjectsToTerrain))
                StickSelectionToTerrain();

            GUI.enabled = true;
        }
    }
}
