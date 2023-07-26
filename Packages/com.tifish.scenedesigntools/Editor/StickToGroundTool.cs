using System;
using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    public class StickToGroundTool : BaseSceneDesignTool
    {
        static StickToGroundTool()
        {
            _raisingHeightBeforeSticking = PlayerPrefs.GetFloat(RaisingHeightBeforeStickingKey, 0);
        }

        public StickToGroundTool(SceneDesignToolsWindow ownerWindow) : base(ownerWindow)
        {
        }


        public static event EventHandler BeforeStickTo;
        public static event EventHandler AfterStickTo;
        private static bool _isStickingSelectionTo;

        public static void StickSelectionToGround()
        {
            _isStickingSelectionTo = true;
            BeforeStickTo?.Invoke(null, EventArgs.Empty);
            try
            {
                foreach (var go in Selection.gameObjects)
                    StickGameObjectToGround(go);
            }
            finally
            {
                AfterStickTo?.Invoke(null, EventArgs.Empty);
                _isStickingSelectionTo = false;
            }
        }

        public static void StickGameObjectToGround(GameObject go)
        {
            if (!_isStickingSelectionTo)
                BeforeStickTo?.Invoke(null, EventArgs.Empty);

            try
            {
                var currentCenter = go.transform.position;
                currentCenter.y += _raisingHeightBeforeSticking;

                while (Physics.Raycast(currentCenter, Vector3.down, out var hitInfo,
                           float.MaxValue, IgnoreLayersTool.Mask))
                {
                    if (hitInfo.transform == go.transform)
                    {
                        currentCenter.y -= 0.01f;
                        continue;
                    }

                    Undo.RecordObject(go.transform, "Stick to ground");
                    StickToPoint(go.transform, hitInfo.point.y);
                    break;
                }
            }
            finally
            {
                if (!_isStickingSelectionTo)
                    AfterStickTo?.Invoke(null, EventArgs.Empty);
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
            _isStickingSelectionTo = true;
            BeforeStickTo?.Invoke(null, EventArgs.Empty);
            try
            {
                foreach (var go in Selection.gameObjects)
                    StickGameObjectToTerrain(go);
            }
            finally
            {
                AfterStickTo?.Invoke(null, EventArgs.Empty);
                _isStickingSelectionTo = false;
            }
        }

        public static void StickGameObjectToTerrain(GameObject go)
        {
            if (!_isStickingSelectionTo)
                BeforeStickTo?.Invoke(null, EventArgs.Empty);

            try
            {
                var currentCenter = go.transform.position;
                currentCenter.y = 60000;

                while (Physics.Raycast(currentCenter, Vector3.down, out var hitInfo,
                           float.MaxValue, IgnoreLayersTool.Mask))
                {
                    if (!hitInfo.transform.GetComponentInChildren<TerrainCollider>())
                    {
                        currentCenter.y = hitInfo.point.y - 0.01f;
                        continue;
                    }

                    Undo.RecordObject(go.transform, "Stick to terrain");
                    StickToPoint(go.transform, hitInfo.point.y);
                    break;
                }
            }
            finally
            {
                if (!_isStickingSelectionTo)
                    AfterStickTo?.Invoke(null, EventArgs.Empty);
            }
        }

        private const string RaisingHeightBeforeStickingKey = "SceneDesignTools.RaisingHeightBeforeSticking";
        private static float _raisingHeightBeforeSticking;

        private static bool _expanded = true;

        public override void OnGUI()
        {
            _expanded = EditorGUILayout.Foldout(_expanded, Strings.StickToGround, EditorGUIHelper.FoldoutStyle);
            if (_expanded)
            {
                GUI.enabled = OwnerWindow.HasSelection;

                EditorGUIHelper.BeginIndent();

                if (GUILayout.Button(Strings.StickSelectedObjectsToGround))
                    StickSelectionToGround();

                EditorGUI.BeginChangeCheck();
                _raisingHeightBeforeSticking = EditorGUILayout.FloatField(
                    Strings.RaisingHeightBeforeSticking, _raisingHeightBeforeSticking);
                if (EditorGUI.EndChangeCheck())
                    PlayerPrefs.SetFloat(RaisingHeightBeforeStickingKey, _raisingHeightBeforeSticking);

                if (GUILayout.Button(Strings.StickSelectedObjectsToTerrain))
                    StickSelectionToTerrain();

                EditorGUIHelper.EndIndent();
            }

            EditorGUIHelper.SeparatorLine();

            GUI.enabled = true;
        }
    }
}
