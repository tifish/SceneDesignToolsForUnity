using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

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

                GUI.enabled = true;

                GUILayout.Label(Strings.VisibleColors);

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(Strings.ShowAll))
                {
                    ShowPoint.VisibleColors.SetAll(true);
                    SaveVisibleColors();
                }

                if (GUILayout.Button(Strings.HideAll))
                {
                    ShowPoint.VisibleColors.SetAll(false);
                    SaveVisibleColors();
                }

                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginChangeCheck();
                for (var i = 0; i < ShowPoint.ColorNames.Length; i++)
                    ShowPoint.VisibleColors[i] = EditorGUILayout.Toggle(ShowPoint.ColorNames[i], ShowPoint.VisibleColors[i]);
                if (EditorGUI.EndChangeCheck())
                    SaveVisibleColors();

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            void SaveVisibleColors()
            {
                var intArray = new int[1];
                ShowPoint.VisibleColors.CopyTo(intArray, 0);
                PlayerPrefs.SetInt(ShowPoint.VisibleColorsKey, intArray[0]);

                SceneView.RepaintAll();
            }
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
