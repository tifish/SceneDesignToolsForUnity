using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    [CustomEditor(typeof(ShowPoint))]
    [CanEditMultipleObjects]
    public class ShowPointInspector : Editor
    {
        private SerializedProperty _colorProp;
        private SerializedProperty _showMeProp;

        private void OnEnable()
        {
            _colorProp = serializedObject.FindProperty(nameof(ShowPoint.color));
            _showMeProp = serializedObject.FindProperty(nameof(ShowPoint.showMe));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_colorProp, new GUIContent(Strings.Color));

            EditorGUILayout.PropertyField(_showMeProp, new GUIContent(Strings.ShowMe));

            EditorGUI.BeginChangeCheck();
            ShowPoint.Multiple = EditorGUILayout.Slider(
                Strings.Multiple, ShowPoint.Multiple, 0, 50);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetFloat(ShowPoint.ShowPointMultipleKey, ShowPoint.Multiple);
                SceneView.RepaintAll();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
