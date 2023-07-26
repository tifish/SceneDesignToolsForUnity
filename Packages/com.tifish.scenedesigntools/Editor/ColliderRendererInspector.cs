using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    [CustomEditor(typeof(ColliderRenderer))]
    [CanEditMultipleObjects]
    public class ColliderRendererInspector : Editor
    {
        private SerializedProperty _colorProp;
        private SerializedProperty _showMeProp;

        private void OnEnable()
        {
            _colorProp = serializedObject.FindProperty(nameof(ColliderRenderer.color));
            _showMeProp = serializedObject.FindProperty(nameof(ShowPoint.showMe));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_showMeProp, new GUIContent(Strings.ShowMe));

            FixedColors.DrawColorProperty(_colorProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
