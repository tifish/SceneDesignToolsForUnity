using System;
using System.Collections.Generic;

namespace SceneDesignTools
{
    public class CustomizedTools : BaseSceneDesignTool
    {
        public CustomizedTools(SceneDesignToolsWindow ownerWindow) : base(ownerWindow)
        {
        }

        private static readonly List<Action> CustomizedToolsCallbacks = new List<Action>();

        public static void AddCustomTools(Action callback)
        {
            if (CustomizedToolsCallbacks.Contains(callback))
                return;

            CustomizedToolsCallbacks.Add(callback);
        }

        public override void OnGUI()
        {
            foreach (var customizedToolsCallback in CustomizedToolsCallbacks)
                customizedToolsCallback();
        }
    }
}
