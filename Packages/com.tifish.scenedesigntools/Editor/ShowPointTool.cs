﻿using UnityEditor;
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

                EditorGUIHelper.EndIndent();
            }

            EditorGUIHelper.SeparatorLine();

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
