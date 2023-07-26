using System;
using System.Collections.Generic;

namespace SceneDesignTools
{
    public class CustomizedTool : BaseSceneDesignTool
    {
        public CustomizedTool(SceneDesignToolsWindow ownerWindow) : base(ownerWindow)
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
