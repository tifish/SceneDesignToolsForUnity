using System.Linq;
using UnityEngine;

namespace SceneDesignTools
{
    [ExecuteInEditMode]
    public class ShowPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool showMe = true;

        private void OnDrawGizmos()
        {
            if (!showMe)
                return;

            Gizmos.color = Color.yellow;
            const float size = 0.2f;

            DrawPoint(transform.position, size);
        }

        private void DrawPoint(in Vector3 position, float size)
        {
            Gizmos.DrawSphere(position + new Vector3(0, size / 2, 0), size);
        }
#endif
    }
}
