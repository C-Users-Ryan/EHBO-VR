using System;
using EHBOVR.PlayerCharacter;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
/*using UnityEngine.Splines;*/

namespace EHBOVR.Car
{
    public class CarBehaviour : MonoBehaviour
    {
        private enum State { Ready, Driving, Braking, Collision, Stationary }
        private State _state = State.Ready;

        public UnityEvent onCarCollision;
        
        private Transform _transform;
/*        private SplineAnimate _splineAnimate;*/
        [SerializeField] private AudioSource drivingAudioSource;
        [SerializeField] private AudioSource impactAudioSource;
        
        private Vector3 _velocity;
        
        [SerializeField] private float drivingSpeed = 1f;
        [SerializeField] private float brakeVelocity = 5.8f;

        [SerializeField] private AudioClip drivingClip;
        [SerializeField] private AudioClip brakingClip;
        [SerializeField] private AudioClip impactClip;

        public UnityEvent playerHit;
        
        void Awake()
        {
            _transform = gameObject.transform;
/*            _splineAnimate = GetComponent<SplineAnimate>();*/
            
            // Calculate initial velocity
            _velocity = _transform.localRotation * Vector3.forward;
            _velocity *= drivingSpeed;
        }
        
        public void InitiateDriving()
        {
            _state = State.Driving;
            drivingAudioSource.clip = drivingClip;
            drivingAudioSource.loop = true;
            drivingAudioSource.Play();
            StartCoroutine(DriveRoutine());
        }

        private IEnumerator DriveRoutine()
        {
            while (_state == State.Driving || _state == State.Braking && _velocity.magnitude > 0.05)
            {
/*                _splineAnimate.ElapsedTime += _velocity.magnitude * Time.deltaTime;*/

                // Slowdown in case of braking
                if (_state == State.Braking)
                {
                    OnDriverReaction();
                    float currentVelocity = _velocity.magnitude;
                    float relativeSlowdown = (currentVelocity - brakeVelocity * Time.deltaTime) / currentVelocity;
                    _velocity *= relativeSlowdown;
                }
                
                yield return null; // Wait for 1 tick
            }

            _state = State.Stationary;
        }

        public void OnTriggerEnter(Collider otherCollider)
        {
            Player player = otherCollider.GetComponentInParent<Player>();
            if (player is not null)
            {
                Debug.Log(otherCollider.gameObject.name);
                InitiateCarCollision();
            }
        }

        public void OnDriverReaction()
        {
            InitiateCarBraking();
        }
        
        private void InitiateCarBraking()
        {
            if ( _state != State.Driving ) return;
            _state = State.Braking;
            drivingAudioSource.Stop();
            drivingAudioSource.clip = brakingClip;
            drivingAudioSource.loop = false;
            drivingAudioSource.Play();
        }

        private void InitiateCarCollision()
        {
            if ( _state is State.Collision or State.Stationary ) return;
            onCarCollision.Invoke();
            _state = State.Collision;
            impactAudioSource.Play();
            drivingAudioSource.Stop();
            
            playerHit.Invoke();
        }
    }
}
