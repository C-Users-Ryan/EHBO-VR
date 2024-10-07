using System.Collections;
using EHBOVR.BoneRetargeting;
using UnityEngine;

namespace EHBOVR.VictimA
{
    public class VictimA : MonoBehaviour
    {
        private enum State { Ready, Walking, Collapsing, Dying }
        private State _state = State.Ready;
        
        private Transform _transform;
        private Vector3 _velocity;
        private Animator _animator;

        [SerializeField] private float walkSpeed = 1f;
        [SerializeField] private BoneRetargeter shoulderBoneLeft;
        [SerializeField] private BoneRetargeter shoulderBoneRight;
        [SerializeField] private GameObject shoulderHandle;
        
        void Awake()
        {
            _transform = gameObject.transform;
            _velocity = _transform.localRotation * Vector3.forward;
            _animator = GetComponent<Animator>();
        }

        public void InitiateWalk()
        {
            _state = State.Walking;
            StartCoroutine(WalkRoutine());
        }

        public void InitiateHeartAttack()
        {
            _state = State.Collapsing;
        }

        public void InitiateRescueProcedure()
        {
            _state = State.Dying;
            
            shoulderBoneLeft.gameObject.SetActive(true);
            shoulderBoneRight.gameObject.SetActive(true);
        }

        IEnumerator WalkRoutine()
        {
            while (_state != State.Collapsing)
            {
                transform.localPosition += _velocity * (Time.deltaTime * walkSpeed);
                yield return null; // Wait for 1 tick
            }
            
            _animator.SetTrigger(Animator.StringToHash("HeartAttackTrigger"));
        }
    }
}
