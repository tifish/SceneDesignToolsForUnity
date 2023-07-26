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
#if UNITY_EDITOR
        public FixedColor color = FixedColor.Yellow;
        public bool showMe = true;

        public static float Multiple = -1f;
        public const string ShowPointMultipleKey = "ShowPointMultiple";

        public const string VisibleColorsKey = "SceneDesignTools.ShowPoint.VisibleColors";

        private static BitArray _visibleColors;

        public static BitArray VisibleColors
        {
            get
            {
                if (_visibleColors == null)
                {
                    _visibleColors = new BitArray(FixedColors.Count, true);
                    var visibleColorsInt = PlayerPrefs.GetInt(VisibleColorsKey, 0xFFFF);
                    for (var i = 0; i < VisibleColors.Count; i++)
                        VisibleColors[i] = ((visibleColorsInt >> i) & 1) == 1;
                }

                return _visibleColors;
            }
        }

        private void OnDrawGizmos()
        {
            if (!showMe)
                return;
            if (!VisibleColors[(int)color])
                return;

            Gizmos.color = FixedColors.GetRealColor(color);
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
