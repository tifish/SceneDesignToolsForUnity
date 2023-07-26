using System.Collections;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace SceneDesignTools
{
    [ExecuteInEditMode]
    public class ColliderRenderer : MonoBehaviour
    {
#if UNITY_EDITOR
        private Collider _collider;
        public FixedColor color = FixedColor.Green;
        public bool showMe = true;

        public const string VisibleColorsKey = "SceneDesignTools.ColliderRenderer.VisibleColors";

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

        private void CheckCollider()
        {
            if (!_collider)
                _collider = gameObject.GetComponent<Collider>();
        }

        private void OnDrawGizmos()
        {
            if (!showMe)
                return;

            if (!VisibleColors[(int)color])
                return;

            CheckCollider();

            var oldGizmosMatrix = Gizmos.matrix;
            var realColor = FixedColors.GetRealColor(color);
            realColor.a = 0.4f;
            Gizmos.color = realColor;

            var trans = transform;
            Gizmos.matrix = Matrix4x4.TRS(trans.position, trans.rotation, trans.lossyScale);
            switch (_collider)
            {
                case BoxCollider box:
                    Gizmos.DrawCube(box.center, box.size);
                    break;
                case SphereCollider sphere:
                    Gizmos.DrawSphere(sphere.center, sphere.radius);
                    break;
                case MeshCollider mesh:
                    Gizmos.DrawMesh(mesh.sharedMesh);
                    break;
            }

            Gizmos.matrix = oldGizmosMatrix;
        }
#endif
    }
}
