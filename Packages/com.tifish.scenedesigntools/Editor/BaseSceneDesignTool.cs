using UnityEditor;

namespace SceneDesignTools
{
    public abstract class BaseSceneDesignTool
    {
        public BaseSceneDesignTool(SceneDesignToolsWindow ownerWindow)
        {
            OwnerWindow = ownerWindow;
        }

        protected readonly SceneDesignToolsWindow OwnerWindow;

        public virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
        {
        }

        public virtual void OnGUI()
        {
        }

        public virtual void OnSceneGUI(SceneView sceneView)
        {
        }

        public virtual void OnSelectionChange()
        {
        }

        public virtual void OnInspectorUpdate()
        {
        }
    }
}
