using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    public class SceneDesignToolsWindow : EditorWindow
    {
        private static SceneDesignToolsWindow Instance
            => GetWindow<SceneDesignToolsWindow>(Strings.SceneDesignTools);

        [MenuItem("Window/" + Strings.SceneDesignTools)]
        public new static void Show()
        {
            Instance.Show(true);
        }

        private readonly List<BaseSceneDesignTool> _sceneDesignTools = new List<BaseSceneDesignTool>();

        private void OnEnable()
        {
            _sceneDesignTools.Clear();
            _sceneDesignTools.Add(new StickToGround(this));
            _sceneDesignTools.Add(new PlaceObjectByClick(this));
            _sceneDesignTools.Add(new IgnoreLayers(this));
            _sceneDesignTools.Add(new ShowPointTool(this));
            _sceneDesignTools.Add(new ColliderTool(this));
            _sceneDesignTools.Add(new CustomizedTools(this));

            SceneView.onSceneGUIDelegate += OnSceneGUI;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;

            foreach (var sceneDesignTool in _sceneDesignTools)
                sceneDesignTool.OnEnable();
        }

        private void OnPlayModeChanged(PlayModeStateChange obj)
        {
        }

        private void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;

            foreach (var sceneDesignTool in _sceneDesignTools)
                sceneDesignTool.OnDisable();
        }

        private Vector2 _scrollPos = Vector2.zero;

        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            foreach (var sceneDesignTool in _sceneDesignTools)
                sceneDesignTool.OnGUI();

            EditorGUILayout.EndScrollView();
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            foreach (var sceneDesignTool in _sceneDesignTools)
                sceneDesignTool.OnSceneGUI(sceneView);
        }

        public bool HasSelection { get; private set; }

        private void OnSelectionChange()
        {
            HasSelection = Selection.activeGameObject != null;

            foreach (var sceneDesignTool in _sceneDesignTools)
                sceneDesignTool.OnSelectionChange();

            Repaint();
        }

        private void OnInspectorUpdate()
        {
            foreach (var sceneDesignTool in _sceneDesignTools)
                sceneDesignTool.OnInspectorUpdate();
        }
    }
}
