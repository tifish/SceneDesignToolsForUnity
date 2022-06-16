using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    [ExecuteInEditMode]
    public class ShowPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool showMe = true;
        public static float Multiple = -1f;
        public const string ShowPointMultipleKey = "ShowPointMultiple";

        private void OnDrawGizmos()
        {
            if (!showMe)
                return;

            Gizmos.color = Color.yellow;
            const float size = 0.2f;

            if (Multiple <= 0)
                Multiple = EditorPrefs.GetFloat(ShowPointMultipleKey, 1f);

            DrawPoint(transform.position, size * Multiple);
        }

        private void DrawPoint(in Vector3 position, float size)
        {
            Gizmos.DrawSphere(position + new Vector3(0, size / 2, 0), size);
        }
#endif
    }
}
