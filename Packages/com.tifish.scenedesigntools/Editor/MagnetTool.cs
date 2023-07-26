using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    public class MagnetTool : BaseSceneDesignTool
    {
        public MagnetTool(SceneDesignToolsWindow ownerWindow) : base(ownerWindow)
        {
        }

        private static bool _expanded = true;

        public override void OnGUI()
        {
            _expanded = EditorGUILayout.Foldout(_expanded, Strings.ColliderRenderer, EditorGUIHelper.FoldoutStyle);
            if (_expanded)
            {
                GUI.enabled = ColliderMagnet.CanMagnent;

                EditorGUIHelper.BeginIndent();

                if (GUILayout.Button(Strings.MagnetCollider))
                    ColliderMagnet.DoMagnet();

                EditorGUILayout.LabelField(Strings.MagnetColliderHint);

                GUI.enabled = true;

                EditorGUIHelper.EndIndent();
            }

            EditorGUIHelper.SeparatorLine();
        }

    }
}
