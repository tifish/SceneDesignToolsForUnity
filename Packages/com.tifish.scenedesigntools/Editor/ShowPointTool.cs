using System;
using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    public class ShowPointTool : BaseSceneDesignTool
    {
        public ShowPointTool(SceneDesignToolsWindow ownerWindow) : base(ownerWindow)
        {
        }

        public override void OnGUI()
        {
            GUI.enabled = OwnerWindow.HasSelection;

            if (GUILayout.Button(Strings.AddShowPointTool))
                foreach (var gameObject in Selection.gameObjects)
                    gameObject.AddComponent<ShowPoint>();

            GUI.enabled = true;
        }
    }
}
