using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    [CustomEditor(typeof(ShowPoint))]
    [CanEditMultipleObjects]
    public class ShowPointInspector : Editor
    {
        private SerializedProperty _showMeProp;
        private SerializedProperty _showChildrenProp;

        private void OnEnable()
        {
            _showMeProp = serializedObject.FindProperty(nameof(ShowPoint.showMe));
            _showChildrenProp = serializedObject.FindProperty(nameof(ShowPoint.showChildren));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_showMeProp, new GUIContent(Strings.ShowMe));
            EditorGUILayout.PropertyField(_showChildrenProp, new GUIContent(Strings.ShowChildren));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
