﻿#if UNITY_EDITOR
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
        public const string StickToGround = "贴地";
        public const string StickSelectedObjectsToGround = "选中物体贴地";
        public const string RaisingHeightBeforeSticking = "贴地之前抬升高度";
        public const string StickSelectedObjectsToTerrain = "选中物体贴地形（Terrain）";
        public const string PlaceObject = "放置物体";
        public const string PlaceSelectedObjectsByClick = "鼠标点击放置选中物体";
        public const string IgnoreLayers = "忽略以下层级";
        public const string ShowPoint = "显示点";
        public const string Color = "颜色";
        public const string ShowMe = "显示我自己";
        public const string Multiple = "放大倍数";
        public const string AddShowPointTool = "添加显示点工具";
        public const string RemoveShowPointTool = "删除显示点工具";
        public const string AddShowPointToolForDirectChildren = "为子节点添加显示点工具";
        public const string RemoveShowPointToolForDirectChildren = "为子节点删除显示点工具";
        public const string ShowPointMultipleLimit = "放大倍数限制：";
        public const string VisibleColors = "要显示的颜色：";
        public const string ShowAll = "全部显示";
        public const string HideAll = "全部隐藏";
        public const string ColliderRenderer = "碰撞体显示器";
        public const string AddColliderRenderer = "添加碰撞体显示器";
        public const string RemoveColliderRenderer = "删除碰撞体显示器";
        public const string MagnetTool = "磁吸工具";
        public const string MagnetCollider = "磁吸碰撞体";
        public const string MagnetColliderHint = "先选中固定物体，再选中要移动的物体";
#else
        public const string SceneDesignTools = "Scene Design Tools";
        public const string StickToGround = "Stick to ground";
        public const string StickSelectedObjectsToGround = "Stick selected objects to ground";
        public const string RaisingHeightBeforeSticking = "Raising before sticking";
        public const string StickSelectedObjectsToTerrain = "Stick selected objects to terrain";
        public const string PlaceObject = "Place object";
        public const string PlaceSelectedObjectsByClick = "Place selected objects by click";
        public const string IgnoreLayers = "Ignore layers";
        public const string ShowPoint = "Show Point";
        public const string Color = "Color";
        public const string ShowMe = "Show Me";
        public const string Multiple = "Multiple";
        public const string AddShowPointTool = "Add show point tool";
        public const string RemoveShowPointTool = "Remove show point tool";
        public const string AddShowPointToolForDirectChildren = "Add show point tool for direct children";
        public const string RemoveShowPointToolForDirectChildren = "Remove show point tool for direct children";
        public const string ShowPointMultipleLimit = "Multiple limit:";
        public const string VisibleColors = "Visible colors:";
        public const string ShowAll = "Show all";
        public const string HideAll = "Hide all";
        public const string ColliderRenderer = "Collider Renderer";
        public const string AddColliderRenderer = "Add collider renderer";
        public const string RemoveColliderRenderer = "Remove collider renderer";
        public const string MagnetTool = "Magnet Tool";
        public const string MagnetCollider = "Magnet colliders";
        public const string MagnetColliderHint = "Select fixed object first, then select moving object";
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
#endif
