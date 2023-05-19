using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SceneDesignTools
{
    [Preserve]
    [ExecuteInEditMode]
    public class ShowPoint : MonoBehaviour
    {
        public enum PointColor
        {
            Red,
            Green,
            Blue,
            White,
            Black,
            Yellow,
            Cyan,
            Magenta,
            Gray,
        }

#if UNITY_EDITOR
        public PointColor color = PointColor.Yellow;
        public bool showMe = true;

        public static float Multiple = -1f;
        public const string ShowPointMultipleKey = "ShowPointMultiple";

        public static readonly string[] ColorNames = Enum.GetNames(typeof(PointColor));
        public static readonly int ColorCount = ColorNames.Length;

        public const string VisibleColorsKey = "SceneDesignTools.ShowPoint.VisibleColors";

        private static BitArray _visibleColors;

        public static BitArray VisibleColors
        {
            get
            {
                if (_visibleColors == null)
                {
                    _visibleColors = new BitArray(ColorCount, true);
                    var visibleColorsInt = PlayerPrefs.GetInt(VisibleColorsKey, 0xFFFF);
                    for (var i = 0; i < VisibleColors.Count; i++)
                        VisibleColors[i] = ((visibleColorsInt >> i) & 1) == 1;
                }

                return _visibleColors;
            }
        }


        public static Color GetRealColor(PointColor color)
        {
            switch (color)
            {
                case PointColor.Red:
                    return Color.red;
                case PointColor.Green:
                    return Color.green;
                case PointColor.Blue:
                    return Color.blue;
                case PointColor.White:
                    return Color.white;
                case PointColor.Black:
                    return Color.black;
                case PointColor.Yellow:
                    return Color.yellow;
                case PointColor.Cyan:
                    return Color.cyan;
                case PointColor.Magenta:
                    return Color.magenta;
                case PointColor.Gray:
                    return Color.gray;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDrawGizmos()
        {
            if (!showMe)
                return;
            if (!VisibleColors[(int)color])
                return;

            Gizmos.color = GetRealColor(color);
            const float size = 0.2f;

            if (Multiple <= 0)
                Multiple = EditorPrefs.GetFloat(ShowPointMultipleKey, 1f);

            DrawPoint(transform.position, size * Multiple);
        }

        private static void DrawPoint(in Vector3 position, float size)
        {
            Gizmos.DrawSphere(position + new Vector3(0, size / 2, 0), size);
        }
#endif
    }
}
