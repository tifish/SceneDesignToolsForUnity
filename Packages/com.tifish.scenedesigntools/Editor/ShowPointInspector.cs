using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    [CustomEditor(typeof(ShowPoint))]
    [CanEditMultipleObjects]
    public class ShowPointInspector : Editor
    {
        private SerializedProperty _showMeProp;

        private void OnEnable()
        {
            _showMeProp = serializedObject.FindProperty(nameof(ShowPoint.showMe));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_showMeProp, new GUIContent(Strings.ShowMe));
            EditorGUI.BeginChangeCheck();
            ShowPoint.Multiple = EditorGUILayout.Slider(Strings.Multiple, ShowPoint.Multiple, 0, 10);
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.lastActiveSceneView.Repaint();
                EditorPrefs.SetFloat(ShowPoint.ShowPointMultipleKey, ShowPoint.Multiple);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
