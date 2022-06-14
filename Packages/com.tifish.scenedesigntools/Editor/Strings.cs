using UnityEngine;

namespace SceneDesignTools
{
    public static class Strings
    {
        public static readonly string SceneDesignTools = "Scene Design Tools";
        public static readonly string StickSelectedObjectsToGround = "Stick selected objects to ground";
        public static readonly string PlaceSelectedObjectsByClick = "Place selected objects by click";
        public static readonly string ShowPoint = "Show Point";
        public static readonly string ShowMe = "Show Me";
        public static readonly string AddShowPointTool = "Add show point tool";
        public static readonly string RemoveShowPointTool = "Remove show point tool";
        public static readonly string AddShowPointToolForDirectChildren = "Add show point tool for direct children";
        public static readonly string RemoveShowPointToolForDirectChildren = "Remove show point tool for direct children";

        static Strings()
        {
            if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
            {
                SceneDesignTools = "场景设计工具";
                StickSelectedObjectsToGround = "选中物体贴地";
                PlaceSelectedObjectsByClick = "鼠标点击放置选中物体";
                ShowMe = "显示我自己";
                AddShowPointTool = "添加显示点工具";
                RemoveShowPointTool = "删除显示点工具";
                AddShowPointToolForDirectChildren = "为子节点添加显示点工具";
                RemoveShowPointToolForDirectChildren = "为子节点删除显示点工具";
            }
        }
    }
}
