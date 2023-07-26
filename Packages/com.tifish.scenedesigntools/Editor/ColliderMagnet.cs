using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    public static class ColliderMagnet
    {
        public static bool CanMagnent => Selection.gameObjects.Length == 2;

        public static void DoMagnet()
        {
            if (!CanMagnent)
                return;

            var movingObj = Selection.activeGameObject.transform;
            var fixedObj = (Selection.gameObjects[0] == Selection.activeGameObject
                ? Selection.gameObjects[1]
                : Selection.gameObjects[0]).transform;

            bool isMovingObjBoxCollider = movingObj.GetComponent<BoxCollider>();
            bool isFixedObjBoxCollider = fixedObj.GetComponent<BoxCollider>();

            if (isMovingObjBoxCollider && isFixedObjBoxCollider)
                DoBoxColliderMagnet(movingObj, fixedObj);
            else if (!isMovingObjBoxCollider && !isFixedObjBoxCollider)
                QuadMagnet.DoQuadMagnet(movingObj, fixedObj);
        }

        private static void DoBoxColliderMagnet(Transform movingObj, Transform fixedObj)
        {
            Undo.RecordObject(movingObj, "Box Collider Magnet");

            RotateBoxToParallel(movingObj, fixedObj);

            if (!RaycastNinePointsToObject(movingObj, fixedObj, out var delta))
            {
                if (RaycastNinePointsToObject(fixedObj, movingObj, out delta))
                    delta = -delta;
                else
                    return;
            }

            var position = movingObj.position;
            position += delta;
            movingObj.position = position;
        }

        private static void RotateBoxToParallel(Transform sourceObject, Transform targetObject)
        {
            // 强制保证y轴朝上。
            var rotation = sourceObject.eulerAngles;
            rotation.x = 0;
            rotation.z = 0;
            sourceObject.eulerAngles = rotation;

            rotation = targetObject.eulerAngles;
            rotation.x = 0;
            rotation.z = 0;
            targetObject.eulerAngles = rotation;

            // 根据策划需求，因为y轴永远朝上，所以只需要处理绕y轴旋转的情况即可。
            var targetForwardDirection = targetObject.rotation * Vector3.forward;

            var sourceDirections = new Vector3[4];
            sourceDirections[0] = sourceObject.rotation * Vector3.forward;
            sourceDirections[1] = sourceObject.rotation * Vector3.back;
            sourceDirections[2] = sourceObject.rotation * Vector3.left;
            sourceDirections[3] = sourceObject.rotation * Vector3.right;

            var minAngle = float.MaxValue;
            var closeSourceDirection = Vector3.zero;
            foreach (var sourceDirection in sourceDirections)
            {
                var angle = Vector3.Angle(sourceDirection, targetForwardDirection);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    closeSourceDirection = sourceDirection;
                }
            }

            sourceObject.rotation = Quaternion.FromToRotation(closeSourceDirection, targetForwardDirection) *
                                    sourceObject.rotation;
        }

        private static bool RaycastNinePointsToObject(Transform sourceObject, Transform targetObject, out Vector3 deltaDistance)
        {
            RaycastHit hitTargetInfo;
            RaycastHit hitSourceInfo;
            var sourceCollider = sourceObject.GetComponent<BoxCollider>();
            var targetCollider = targetObject.GetComponent<BoxCollider>();
            var boxSize = Vector3.Scale(sourceCollider.size, sourceObject.lossyScale);
            boxSize /= 2;
            var boxExtends = new Vector3[4];
            boxExtends[0] = boxSize;
            boxExtends[1] = boxSize;
            boxExtends[2] = boxSize;
            boxExtends[3] = boxSize;
            boxExtends[1].x *= -1;
            boxExtends[2].y *= -1;
            boxExtends[3].x *= -1;
            boxExtends[3].y *= -1;

            if (RaycastSixDirectionsToObject(sourceObject.localToWorldMatrix.MultiplyPoint(sourceCollider.center),
                    sourceObject, sourceCollider, targetCollider, out hitTargetInfo, out hitSourceInfo)
                || RaycastSixDirectionsToObject(
                    sourceObject.localToWorldMatrix.MultiplyPoint(sourceCollider.center + boxExtends[0]), sourceObject,
                    sourceCollider, targetCollider, out hitTargetInfo, out hitSourceInfo)
                || RaycastSixDirectionsToObject(
                    sourceObject.localToWorldMatrix.MultiplyPoint(sourceCollider.center - boxExtends[0]), sourceObject,
                    sourceCollider, targetCollider, out hitTargetInfo, out hitSourceInfo)
                || RaycastSixDirectionsToObject(
                    sourceObject.localToWorldMatrix.MultiplyPoint(sourceCollider.center + boxExtends[1]), sourceObject,
                    sourceCollider, targetCollider, out hitTargetInfo, out hitSourceInfo)
                || RaycastSixDirectionsToObject(
                    sourceObject.localToWorldMatrix.MultiplyPoint(sourceCollider.center - boxExtends[1]), sourceObject,
                    sourceCollider, targetCollider, out hitTargetInfo, out hitSourceInfo)
                || RaycastSixDirectionsToObject(
                    sourceObject.localToWorldMatrix.MultiplyPoint(sourceCollider.center + boxExtends[2]), sourceObject,
                    sourceCollider, targetCollider, out hitTargetInfo, out hitSourceInfo)
                || RaycastSixDirectionsToObject(
                    sourceObject.localToWorldMatrix.MultiplyPoint(sourceCollider.center - boxExtends[2]), sourceObject,
                    sourceCollider, targetCollider, out hitTargetInfo, out hitSourceInfo)
                || RaycastSixDirectionsToObject(
                    sourceObject.localToWorldMatrix.MultiplyPoint(sourceCollider.center + boxExtends[3]), sourceObject,
                    sourceCollider, targetCollider, out hitTargetInfo, out hitSourceInfo)
                || RaycastSixDirectionsToObject(
                    sourceObject.localToWorldMatrix.MultiplyPoint(sourceCollider.center - boxExtends[3]), sourceObject,
                    sourceCollider, targetCollider, out hitTargetInfo, out hitSourceInfo)
               )
            {
                deltaDistance = hitTargetInfo.point - hitSourceInfo.point;
                return true;
            }

            deltaDistance = Vector3.zero;
            return false;
        }

        private static bool RaycastSixDirectionsToObject(Vector3 sourcePosition, Transform sourceObject, Collider sourceCollider,
            Collider targetCollider, out RaycastHit hitTargetInfo, out RaycastHit hitSourceInfo)
        {
            if (RaycastToObject(sourcePosition, sourceObject.rotation * Vector3.forward, targetCollider, out hitTargetInfo)
                || RaycastToObject(sourcePosition, sourceObject.rotation * Vector3.back, targetCollider, out hitTargetInfo)
                || RaycastToObject(sourcePosition, sourceObject.rotation * Vector3.left, targetCollider, out hitTargetInfo)
                || RaycastToObject(sourcePosition, sourceObject.rotation * Vector3.right, targetCollider, out hitTargetInfo)
                || RaycastToObject(sourcePosition, sourceObject.rotation * Vector3.up, targetCollider, out hitTargetInfo)
                || RaycastToObject(sourcePosition, sourceObject.rotation * Vector3.down, targetCollider, out hitTargetInfo))
                return RaycastToObject(hitTargetInfo.point, sourcePosition - hitTargetInfo.point, sourceCollider,
                    out hitSourceInfo);

            hitTargetInfo = new RaycastHit();
            hitSourceInfo = new RaycastHit();
            return false;
        }

        private static bool RaycastToObject(Vector3 startPosition, Vector3 direction, Collider targetCollider,
            out RaycastHit result)
        {
            return targetCollider.Raycast(new Ray(startPosition, direction), out result, Mathf.Infinity);
        }
    }
}
