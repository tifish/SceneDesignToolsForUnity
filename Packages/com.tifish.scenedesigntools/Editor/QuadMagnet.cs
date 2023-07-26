using UnityEditor;
using UnityEngine;

namespace SceneDesignTools
{
    public static class QuadMagnet
    {
        public static void DoQuadMagnet(Transform movingObj, Transform fixedObj)
        {
            Undo.RecordObject(movingObj, "Magnet");

            RotateToParallel(movingObj, fixedObj);
        }

        private static void RotateToParallel(Transform sourceObject, Transform targetObject)
        {
            // 计算两个平面的四边中点位置。
            var sourcePosition = sourceObject.position;
            var sourceRotation = sourceObject.rotation;
            var sourceScale = sourceObject.lossyScale;
            var sourceExtendX = sourceScale.x / 2;
            var sourceExtendY = sourceScale.y / 2;
            var sourceEdgeMiddlePoints = new[]
            {
                sourcePosition + sourceRotation * Vector3.up * sourceExtendY,
                sourcePosition + sourceRotation * Vector3.down * sourceExtendY,
                sourcePosition + sourceRotation * Vector3.left * sourceExtendX,
                sourcePosition + sourceRotation * Vector3.right * sourceExtendX,
            };

            var targetPosition = targetObject.position;
            var targetRotation = targetObject.rotation;
            var targetScale = targetObject.lossyScale;
            var targetExtendX = targetScale.x / 2;
            var targetExtendY = targetScale.y / 2;
            var targetEdgeMiddlePoints = new[]
            {
                targetPosition + targetRotation * Vector3.up * targetExtendY,
                targetPosition + targetRotation * Vector3.down * targetExtendY,
                targetPosition + targetRotation * Vector3.left * targetExtendX,
                targetPosition + targetRotation * Vector3.right * targetExtendX,
            };

            // 计算两两边之间的距离，取距离最小的两条边。
            var minDistance = float.MaxValue;
            var sourceMinDistanceIndex = 0;
            var targetMinDistanceIndex = 0;
            for (var i = 0; i < sourceEdgeMiddlePoints.Length; i++)
            {
                for (var j = 0; j < targetEdgeMiddlePoints.Length; j++)
                {
                    var distance = Vector3.Distance(sourceEdgeMiddlePoints[i], targetEdgeMiddlePoints[j]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        sourceMinDistanceIndex = i;
                        targetMinDistanceIndex = j;
                    }
                }
            }

            // 把距离最小的两条边，旋转到一致的方向。
            // 先计算所有可能的旋转方向。
            var sourceEdgeDirections = new Vector3[2];
            if (sourceMinDistanceIndex == 0 || sourceMinDistanceIndex == 1)
            {
                sourceEdgeDirections[0] = sourceRotation * Vector3.left;
                sourceEdgeDirections[1] = sourceRotation * Vector3.right;
            }
            else if (sourceMinDistanceIndex == 2 || sourceMinDistanceIndex == 3)
            {
                sourceEdgeDirections[0] = sourceRotation * Vector3.up;
                sourceEdgeDirections[1] = sourceRotation * Vector3.down;
            }

            var targetEdgeDirections = new Vector3[2];
            if (targetMinDistanceIndex == 0 || targetMinDistanceIndex == 1)
            {
                targetEdgeDirections[0] = targetRotation * Vector3.left;
                targetEdgeDirections[1] = targetRotation * Vector3.right;
            }
            else if (targetMinDistanceIndex == 2 || targetMinDistanceIndex == 3)
            {
                targetEdgeDirections[0] = targetRotation * Vector3.up;
                targetEdgeDirections[1] = targetRotation * Vector3.down;
            }

            // 计算两两方向之间的旋转角度，得到最小的旋转角度的旋转方式。
            var minAngle = float.MaxValue;
            var sourceMinAngleIndex = 0;
            var targetMinAngleIndex = 0;
            for (var i = 0; i < sourceEdgeDirections.Length; i++)
            {
                for (var j = 0; j < targetEdgeDirections.Length; j++)
                {
                    var angle = Vector3.Angle(sourceEdgeDirections[i], targetEdgeDirections[j]);
                    if (angle < minAngle)
                    {
                        minAngle = angle;
                        sourceMinAngleIndex = i;
                        targetMinAngleIndex = j;
                    }
                }
            }

            // 把旋转角度最小的边旋转成一致的方向。
            sourceObject.rotation =
                Quaternion.FromToRotation(sourceEdgeDirections[sourceMinAngleIndex],
                    targetEdgeDirections[targetMinAngleIndex])
                * sourceObject.rotation;

            sourceRotation = sourceObject.rotation;
            targetRotation = targetObject.rotation;

            sourceEdgeMiddlePoints = new[]
            {
                sourcePosition + sourceRotation * Vector3.up * sourceExtendY,
                sourcePosition + sourceRotation * Vector3.down * sourceExtendY,
                sourcePosition + sourceRotation * Vector3.left * sourceExtendX,
                sourcePosition + sourceRotation * Vector3.right * sourceExtendX,
            };
            targetEdgeMiddlePoints = new[]
            {
                targetPosition + targetRotation * Vector3.up * targetExtendY,
                targetPosition + targetRotation * Vector3.down * targetExtendY,
                targetPosition + targetRotation * Vector3.left * targetExtendX,
                targetPosition + targetRotation * Vector3.right * targetExtendX,
            };

            // 把2条边在贴合在一起，并尽量保持当前的相对位置。
            // 通过投影在同一个垂直平面来实现。
            var sourceProjectedPoint = Vector3.ProjectOnPlane(
                sourceEdgeMiddlePoints[sourceMinDistanceIndex], targetEdgeDirections[targetMinAngleIndex]);
            var targetProjectedPoint = Vector3.ProjectOnPlane(
                targetEdgeMiddlePoints[targetMinDistanceIndex], targetEdgeDirections[targetMinAngleIndex]);

            var positionDelta = targetProjectedPoint - sourceProjectedPoint;
            sourceObject.position += positionDelta;
        }
    }
}
