using System.Collections.Generic;
using System.Linq;
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
            currentCenter.y += _raisingHeightBeforeSticking;

            while (Physics.Raycast(currentCenter, Vector3.down, out var hitInfo))
            {
                if (hitInfo.transform == go.transform)
                {
                    currentCenter.y -= 0.01f;
                    continue;
                }

                if (_ignoreLayers.Contains(hitInfo.transform.gameObject.layer))
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

            while (Physics.Raycast(currentCenter, Vector3.down, out var hitInfo))
            {
                if (_ignoreLayers.Contains(hitInfo.transform.gameObject.layer)
                    || !hitInfo.transform.GetComponentInChildren<TerrainCollider>())
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

            IgnoreLayersOnGUI();

            GUI.enabled = true;
        }

        private static readonly List<string> LayerNames =
            Enumerable.Range(0, 32)
                .Select(LayerMask.LayerToName)
                .Where(l => !string.IsNullOrEmpty(l))
                .ToList();

        private const string IgnoreLayersKey = "SceneDesignTools.IgnoreLayers";

        private static List<int> _ignoreLayers;

        static StickToGround()
        {
            _raisingHeightBeforeSticking = PlayerPrefs.GetFloat(RaisingHeightBeforeStickingKey, 0);

            var pref = PlayerPrefs.GetString(IgnoreLayersKey, "");

            if (string.IsNullOrEmpty(pref))
                _ignoreLayers = new List<int>();
            else
                _ignoreLayers = pref.Split(',')
                    .Select(int.Parse)
                    .ToList();
        }

        private void IgnoreLayersOnGUI()
        {
            GUILayout.Label(Strings.IgnoreLayers);

            EditorGUILayout.BeginVertical(EditorStyles.foldout);

            EditorGUI.BeginChangeCheck();
            for (var i = 0; i < _ignoreLayers.Count; i++)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

                _ignoreLayers[i] = EditorGUILayout.LayerField(_ignoreLayers[i], EditorStyles.toolbarDropDown);
                if (GUILayout.Button("-", EditorStyles.toolbarButton))
                    _ignoreLayers.RemoveAt(i);

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+", EditorStyles.toolbarButton))
                for (var i = 0; i < 32; i++)
                {
                    if (_ignoreLayers.Contains(i) || LayerMask.LayerToName(i) == null)
                        continue;

                    _ignoreLayers.Add(i);
                    break;
                }

            if (EditorGUI.EndChangeCheck())
            {
                _ignoreLayers = _ignoreLayers.Distinct().ToList();
                var pref = string.Join(",", _ignoreLayers.Select(i => i.ToString()));
                PlayerPrefs.SetString(IgnoreLayersKey, pref);
            }

            EditorGUILayout.EndVertical();
        }
    }
}
