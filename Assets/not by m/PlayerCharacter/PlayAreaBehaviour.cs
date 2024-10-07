using System.Collections.Generic;
using UnityEngine;

namespace EHBOVR.PlayerCharacter
{
    // written based on: https://communityforums.atmeta.com/t5/Unity-VR-Development/Aligning-world-with-Guardian-Play-Area/td-p/739453
    public class PlayAreaBehaviour : MonoBehaviour
    {
        private Transform _playerTransform;
        [SerializeField] private Transform boundSquare;

        [SerializeField] private bool visualizeBounds;
        
        private List<Vector3> _boundCorners = new();

        private float _length;
        private float _width;

        void Awake()
        {
            _playerTransform = transform.parent;
        }
        
        void Start()
        {
            UpdateVisibility(visualizeBounds);
        }
        
        void LateUpdate()
        {
            // TODO: Ideally the player's centering gets called by an event, but I've yet to find an event to bind this functionality to.
            if (OVRManager.boundary != null && OVRManager.boundary.GetConfigured())
            {
                UpdateBounds();
                CenterPlayer();
            }
        }

        public void SetVisibility(bool visibility)
        {
            if (visibility != visualizeBounds)
            {
                UpdateVisibility(visibility);
                visualizeBounds = visibility;
            }
        }

        private void UpdateVisibility(bool visibility)
        {
            boundSquare.gameObject.SetActive(visibility);
        }

        /// <summary>
        /// Returns a vector2 containing the area bounds dimensions in meters. The first of the two values is always the longest.
        /// </summary>
        /// <returns>A vector2 containing the area bounds dimensions in meters</returns>
        public Vector2 GetDimensions()
        {
            return _width > _length ? new Vector2(_width, _length) : new Vector2(_length, _width);
        }

        private void UpdateBounds()
        {
            Vector3[] bounds = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
            for (int i = 0; i < 4; i++)
            {
                Vector3 coord = bounds[i];
                    
                if (_boundCorners.Count <= i)
                {
                    _boundCorners.Add(coord);
                }
                else
                {
                    _boundCorners[i] = coord;
                }
            }
        }

        // TODO: This horrid function badly needs a code cleanup.
        private void CenterPlayer()
        {
            // Find the position and lengths of the bounds' edges
            Vector3 pointA = (_boundCorners[0] + _boundCorners[1]) * .5f;
            Vector3 pointB = (_boundCorners[2] + _boundCorners[3]) * .5f;
            Vector3 pointC = (_boundCorners[1] + _boundCorners[2]) * .5f;
            Vector3 pointD = (_boundCorners[3] + _boundCorners[0]) * .5f;

            _length = (pointB - pointA).magnitude;
            _width = (pointD - pointC).magnitude;
            
            // Determine rotation and scale
            Quaternion boundsRotation;
            Vector3 boundsPosition = Vector3.Lerp(pointA, pointB, .5f);
            if (_length < _width)
            {
                boundSquare.localScale = new Vector3(_width, 1, _length);
                boundsRotation = Quaternion.LookRotation(pointA - boundsPosition);
            }
            else
            {
                boundSquare.localScale = new Vector3(_length, 1, _width);
                boundsRotation = Quaternion.LookRotation(pointC - boundsPosition);
            }
            
            // Apply inverse transformation to the player
            _playerTransform.transform.rotation = Quaternion.Inverse(boundsRotation);
            _playerTransform.transform.position = _playerTransform.transform.rotation * -boundsPosition;
            
            // Place the player area at the world origin
            boundSquare.position = Vector3.zero;
            boundSquare.rotation = Quaternion.identity;
        }
    }
}
