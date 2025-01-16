using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

namespace EHBOVR.BoneRetargeting
{
    
    public class BoneRetargeter : MonoBehaviour
    {
        private struct BoneInfo
        {
            public BoneInfo(Transform transform, Vector3 originalPosition, Quaternion originalRotation)
            {
                Transform = transform;
                OriginalPosition = originalPosition;
                OriginalRotation = originalRotation;
            }
            
            public Transform Transform;
            public Vector3 OriginalPosition; // Original local position of the bone
            public Quaternion OriginalRotation; // Original local rotation of the bone
        }

        [SerializeField] private Transform bone;
        private List<BoneInfo> boneChildren;
        [SerializeField] private Vector3 positionBounds = new Vector3(.1f, .1f, .1f);
        
        private Vector3 boneOrigin; // Original local position of the bone
        private Vector3 positionOffset; // Offset between bone and target location

        void Awake()
        {
            boneOrigin = bone.transform.localPosition;
            positionOffset = transform.localPosition - boneOrigin;
            
            // Register dataset of child bones
            boneChildren = new List<BoneInfo>();
            foreach (Transform child in bone.transform)
            {
                BoneInfo childBone = new BoneInfo(child, child.localPosition, child.localRotation);
                boneChildren.Add(childBone);
            }
        }
        
        void LateUpdate()
        {
            bone.transform.localPosition = ClampVector3(transform.localPosition - positionOffset, -positionBounds + boneOrigin, positionBounds + boneOrigin);
            foreach (BoneInfo child in boneChildren)
            {
                child.Transform.localPosition = child.OriginalPosition + Quaternion.Inverse(bone.transform.localRotation) * (boneOrigin - bone.transform.localPosition);
            }
        }

        private Vector3 ClampVector3(Vector3 subject, Vector3 minVal, Vector3 maxVal)
        {
            float x = Mathf.Clamp(subject.x, minVal.x, maxVal.x);
            float y = Mathf.Clamp(subject.y, minVal.y, maxVal.y);
            float z = Mathf.Clamp(subject.z, minVal.z, maxVal.z);
            return new Vector3(x, y, z);
        }
        
        public void OnRelease()
        {
            transform.localPosition = bone.transform.localPosition + positionOffset;
        }
    }
}
