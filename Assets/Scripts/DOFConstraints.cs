using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.Interaction.Toolkit
{
    [DefaultExecutionOrder(XRInteractionUpdateOrder.k_BeforeRenderOrder+1)]
    public class DOFConstraints : MonoBehaviour
    {
        [Tooltip("Whether to constrain the position along the local X-axis.")]
        public bool PositionX;

        [Tooltip("Whether to constrain the position along the local Y-axis.")]
        public bool PositionY;

        [Tooltip("Whether to constrain the position along the local Z-axis.")]
        public bool PositionZ;

        [Tooltip("Whether to constrain the rotation around the local X-axis.")]
        public bool RotationX;

        [Tooltip("Whether to constrain the rotation around the local Y-axis.")]
        public bool RotationY;

        [Tooltip("Whether to constrain the rotation around the local Z-axis.")]
        public bool RotationZ;

        // The GameObject's original parent.
        private Transform originalParent;

        // Cache of the local position and rotation constraints relative to the original parent.
        private Vector3 localPositionConstraints;
        private Vector3 localRotationConstraints;

        // The following is a hack around the XRInteractionManager's control.
        protected virtual void OnEnable()
        {
            Application.onBeforeRender += LateUpdate;
        }

        // The following is a hack around the XRInteractionManager's control.
        protected virtual void OnDisable()
        {
            Application.onBeforeRender -= LateUpdate;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Store the original parent.
            originalParent = transform.parent;

            // Store the local positions and rotations as constraints.
            localPositionConstraints = transform.localPosition;
            localRotationConstraints = transform.localRotation.eulerAngles;
        }

        // LateUpdate is called once per frame after Update
        // The following is a hack around the XRInteractionManager's control.
        [BeforeRenderOrder(XRInteractionUpdateOrder.k_BeforeRenderOrder+1)]
        void LateUpdate()
        {

            // If the GameObject originally was a child.
            if (originalParent != null)
            {
                // Get the current world rotation.
                Quaternion worldRotation = transform.rotation;

                // Get the curent local rotation relative to the original parent.
                Quaternion localRotation = Quaternion.Inverse(originalParent.transform.rotation) * worldRotation;

                // Get the current local euler angles.
                Vector3 localEulerAngles = localRotation.eulerAngles;

                // Check the X-axis.
                if (RotationX) { localEulerAngles.x = localRotationConstraints.x; }

                // Check the Y-axis.
                if (RotationY) { localEulerAngles.y = localRotationConstraints.y; }

                // Check the Z-axis.
                if (RotationZ) { localEulerAngles.z = localRotationConstraints.z; }

                // Convert back to a quaternion.
                localRotation.eulerAngles = localEulerAngles;

                // Convert the new local rotation back to a world rotation.
                worldRotation = originalParent.transform.rotation * localRotation;

                // Apply the constrained world rotation.
                transform.rotation = worldRotation;

                // Get the current world position.
                Vector3 worldPosition = transform.position;

                // Get the current local position relative to the original parent.
                Vector3 localPosition = originalParent.InverseTransformPoint(worldPosition);

                // Check the X-axis.
                if (PositionX) { localPosition.x = localPositionConstraints.x; }

                // Check the Y-axis.
                if (PositionY) { localPosition.y = localPositionConstraints.y; }

                // Check the Z-axis.
                if (PositionZ) { localPosition.z = localPositionConstraints.z; }

                // Convert the new local position back to a world position.
                worldPosition = originalParent.TransformPoint(localPosition);

                // Apply the constrained world position.
                transform.position = worldPosition;
            }
            // If the GameObject originally was a top node.
            else
            {
                // Get the current world rotation.
                Quaternion worldRotation = transform.rotation;

                // Get the current world euler angles.
                Vector3 worldEulerAngles = worldRotation.eulerAngles;

                // Check the X-axis.
                if (RotationX) { worldEulerAngles.x = localRotationConstraints.x; }

                // Check the Y-axis.
                if (RotationY) { worldEulerAngles.y = localRotationConstraints.y; }

                // Check the Z-axis.
                if (RotationZ) { worldEulerAngles.z = localRotationConstraints.z; }

                // Convert back to a quaternion.
                worldRotation.eulerAngles = worldEulerAngles;

                // Apply the constrained world rotation.
                transform.rotation = worldRotation;

                // Get the current world position.
                Vector3 worldPosition = transform.position;

                // Check the X-axis.
                if (PositionX) { worldPosition.x = localPositionConstraints.x; }

                // Check the Y-axis.
                if (PositionY) { worldPosition.y = localPositionConstraints.y; }

                // Check the Z-axis.
                if (PositionZ) { worldPosition.z = localPositionConstraints.z; }

                // Apply the constrained world position.
                transform.position = worldPosition;
            }
        }
    }
}

