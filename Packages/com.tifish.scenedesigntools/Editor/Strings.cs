using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    [InitializeOnLoad]
    public static class Strings
    {
#if SCENE_DESIGN_TOOLS_CHINESE_SIMPLIFIED
        public const string SceneDesignTools = "场景设计工具";
        public const string StickSelectedObjectsToGround = "选中物体贴地";
        public const string RaisingHeightBeforeSticking = "贴地之前抬升高度";
        public const string StickSelectedObjectsToTerrain = "选中物体贴地形（Terrain）";
        public const string PlaceSelectedObjectsByClick = "鼠标点击放置选中物体";
        public const string IgnoreLayers = "忽略以下层级：";
        public const string ShowPoint = "显示点";
        public const string ShowMe = "显示我自己";
        public const string Multiple = "放大倍数";
        public const string AddShowPointTool = "添加显示点工具";
        public const string RemoveShowPointTool = "删除显示点工具";
        public const string AddShowPointToolForDirectChildren = "为子节点添加显示点工具";
        public const string RemoveShowPointToolForDirectChildren = "为子节点删除显示点工具";
#else
        public const string SceneDesignTools = "Scene Design Tools";
        public const string StickSelectedObjectsToGround = "Stick selected objects to ground";
        public const string RaisingHeightBeforeSticking = "Raising before sticking";
        public const string StickSelectedObjectsToTerrain = "Stick selected objects to terrain";
        public const string PlaceSelectedObjectsByClick = "Place selected objects by click";
        public const string IgnoreLayers = "Ignore layers:";
        public const string ShowPoint = "Show Point";
        public const string ShowMe = "Show Me";
        public const string Multiple = "Multiple";
        public const string AddShowPointTool = "Add show point tool";
        public const string RemoveShowPointTool = "Remove show point tool";
        public const string AddShowPointToolForDirectChildren = "Add show point tool for direct children";
        public const string RemoveShowPointToolForDirectChildren = "Remove show point tool for direct children";
#endif

        private const string ChineseMacroName = "SCENE_DESIGN_TOOLS_CHINESE_SIMPLIFIED";

        static Strings()
        {
            var macros = PlayerSettings.GetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup).Split(';').ToList();

            if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
            {
                if (macros.Contains(ChineseMacroName))
                    return;

                macros.Add(ChineseMacroName);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", macros));
            }
            else
            {
                if (!macros.Contains(ChineseMacroName))
                    return;

                macros.Remove(ChineseMacroName);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", macros));
            }
        }
    }
}
