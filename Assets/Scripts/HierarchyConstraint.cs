using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.Interaction.Toolkit
{
    [DefaultExecutionOrder(XRInteractionUpdateOrder.k_BeforeRenderOrder + 2)]
    public class HierarchyConstraint : MonoBehaviour
    {
        // The GameObject's original parent.
        private Transform originalParent;

        // Cache of the position and rotation of the parent relative to the child.
        private Vector3 relativePosition;
        private Quaternion relativeRotation;

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

            // If the GameObject originally is a child.
            if (originalParent != null)
            {
                // Get the world rotation of the parent.
                Quaternion worldRotation = originalParent.rotation;

                // Get the rotation of the parent relative to this child.
                relativeRotation = Quaternion.Inverse(transform.rotation) * worldRotation;

                // Get the world position of the parent.
                Vector3 worldPosition = originalParent.position;

                // Get the position of the parent relative to the child.
                relativePosition = transform.InverseTransformPoint(worldPosition);
            }
        }

        // LateUpdate is called once per frame after Update
        // The following is a hack around the XRInteractionManager's control.
        [BeforeRenderOrder(XRInteractionUpdateOrder.k_BeforeRenderOrder + 2)]
        void LateUpdate()
        {

            // If the GameObject originally was a child.
            if (originalParent != null)
            {
                // Convert the parent's original relative rotation to a world rotation.
                Quaternion worldRotation = transform.rotation * relativeRotation;

                // Apply it to the parent.
                originalParent.rotation = worldRotation;

                // Convert the parent's original relative position to a world position.
                Vector3 worldPosition = transform.TransformPoint(relativePosition);

                // Apply it to the parent.
                originalParent.position = worldPosition;
            }
        }
    }
}

