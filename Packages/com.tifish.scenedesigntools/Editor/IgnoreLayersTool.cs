using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    public class IgnoreLayersTool : BaseSceneDesignTool
    {
        public IgnoreLayersTool(SceneDesignToolsWindow ownerWindow) : base(ownerWindow)
        {
        }

        private const string IgnoreLayersKey = "SceneDesignTools.IgnoreLayers";

        private static List<int> _layerIDs;

        public static int Mask { get; private set; }

        private static void UpdateIgnoreMask()
        {
            Mask = ~_layerIDs.Aggregate(0, (ignoreMask, ignoreLayerID) => ignoreMask | (1 << ignoreLayerID));
        }

        static IgnoreLayersTool()
        {
            var pref = PlayerPrefs.GetString(IgnoreLayersKey, "");

            if (string.IsNullOrEmpty(pref))
                _layerIDs = new List<int>();
            else
                _layerIDs = pref.Split(',')
                    .Select(int.Parse)
                    .ToList();

            UpdateIgnoreMask();
        }

        private static bool _expanded = true;

        public override void OnGUI()
        {
            _expanded = EditorGUILayout.Foldout(_expanded, Strings.IgnoreLayers, EditorGUIHelper.FoldoutStyle);
            if (_expanded)
            {
                EditorGUIHelper.BeginIndent();

                EditorGUI.BeginChangeCheck();
                for (var i = 0; i < _layerIDs.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

                    _layerIDs[i] = EditorGUILayout.LayerField(_layerIDs[i], EditorStyles.toolbarDropDown);
                    if (GUILayout.Button("-", EditorStyles.toolbarButton))
                        _layerIDs.RemoveAt(i);

                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("+", EditorStyles.toolbarButton))
                    for (var i = 0; i < 32; i++)
                    {
                        if (_layerIDs.Contains(i) || LayerMask.LayerToName(i) == null)
                            continue;

                        _layerIDs.Add(i);
                        break;
                    }

                if (EditorGUI.EndChangeCheck())
                {
                    _layerIDs = _layerIDs.Distinct().ToList();
                    UpdateIgnoreMask();

                    var pref = string.Join(",", _layerIDs.Select(i => i.ToString()));
                    PlayerPrefs.SetString(IgnoreLayersKey, pref);
                }

                GUILayout.Space(2f);
                EditorGUIHelper.EndIndent();
            }

            EditorGUIHelper.SeparatorLine();
        }
    }
}
