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

            EditorGUILayout.PropertyField(_showMeProp, new GUIContent(Strings.ShowMe));

            DrawMultiple();

            DrawColorProperty();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawMultiple()
        {
            EditorGUI.BeginChangeCheck();

            ShowPoint.Multiple = EditorGUILayout.Slider(
                Strings.Multiple, ShowPoint.Multiple, 0, ShowPointTool.MultipleLimit);

            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetFloat(ShowPoint.ShowPointMultipleKey, ShowPoint.Multiple);
                SceneView.RepaintAll();
            }
        }

        private void DrawColorProperty()
        {
            GUILayout.Label(Strings.Color);

            var oldColor = GUI.backgroundColor;

            for (var i = 0; i < FixedColors.Names.Length; i++)
            {
                if (i % 3 == 0)
                    EditorGUILayout.BeginHorizontal();

                GUI.backgroundColor = FixedColors.GetRealColor((FixedColor)i);

                EditorGUI.BeginChangeCheck();

                var selected = i == _colorProp.enumValueIndex;
                selected = GUILayout.Toggle(selected, selected ? "O" : "x", "Button");

                if (EditorGUI.EndChangeCheck())
                    if (selected)
                        _colorProp.enumValueIndex = i;

                GUI.backgroundColor = oldColor;

                if (i % 3 == 2)
                    EditorGUILayout.EndHorizontal();
            }

            if ((FixedColors.Names.Length - 1) % 3 != 2)
                EditorGUILayout.EndHorizontal();
        }
    }
}
