using System.Linq;
using UnityEngine;

namespace SceneDesignTools
{
    [ExecuteInEditMode]
    public class ShowPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool showMe = true;
        public bool showChildren;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            const float size = 0.2f;

            if (showMe)
                DrawPoint(transform.position, size);

            if (showChildren)
                foreach (var childTransform in GetComponentsInChildren<Transform>().Skip(1))
                    DrawPoint(childTransform.position, size);
        }

        private void DrawPoint(in Vector3 position, float size)
        {
            Gizmos.DrawSphere(position + new Vector3(0, size / 2, 0), size);
        }
#endif
    }
}
