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

            serializedObject.ApplyModifiedProperties();
        }
    }
}
