using System.Collections.Generic;
using UnityEngine;

namespace EHBOVR.PlayerCharacter
{
    public class PlayAreaBehaviour : MonoBehaviour
    {
            private Transform _playerTransform;
            [SerializeField] private Transform boundSquare; // Visual bounds
            [SerializeField] private Transform autoCenterReference; // Reference point for auto-centering
            [SerializeField] private GameObject prefabToSpawn; // Prefab to place in the play area
            [SerializeField] private float edgeBuffer = 0.5f; // Distance from edges for prefab placement
            [SerializeField] private bool visualizeBounds;

            public bool gameStarted = false; // Control when the game starts

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
                if (OVRManager.boundary != null && OVRManager.boundary.GetConfigured())
                {
                    UpdateBounds();
                    CenterPlayer();

                    if (gameStarted)
                    {
                        StartGame();
                    }
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

            private void CenterPlayer()
            {
                Vector3 pointA = (_boundCorners[0] + _boundCorners[1]) * 0.5f;
                Vector3 pointB = (_boundCorners[2] + _boundCorners[3]) * 0.5f;
                Vector3 pointC = (_boundCorners[1] + _boundCorners[2]) * 0.5f;
                Vector3 pointD = (_boundCorners[3] + _boundCorners[0]) * 0.5f;

                _length = (pointB - pointA).magnitude;
                _width = (pointD - pointC).magnitude;

                Quaternion boundsRotation;
                Vector3 boundsPosition = Vector3.Lerp(pointA, pointB, 0.5f);
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

                if (autoCenterReference != null)
                {
                    Vector3 referencePosition = autoCenterReference.position;
                    boundsPosition += referencePosition;
                    _playerTransform.transform.rotation = Quaternion.Inverse(boundsRotation);
                    _playerTransform.transform.position = _playerTransform.transform.rotation * -boundsPosition;
                }

                boundSquare.position = Vector3.zero;
                boundSquare.rotation = Quaternion.identity;
            }

            private void StartGame()
            {
                if (prefabToSpawn != null && boundSquare.gameObject.activeSelf)
                {
                    Vector3 spawnLocation = GenerateRandomLocation();
                    Instantiate(prefabToSpawn, spawnLocation, Quaternion.identity);

                    // Hide the bounds after starting the game
                    UpdateVisibility(false);
                }
            }

            private Vector3 GenerateRandomLocation()
            {
                Vector3 center = boundSquare.position;
                float halfWidth = _width / 2f - edgeBuffer;
                float halfLength = _length / 2f - edgeBuffer;

                float randomX = Random.Range(-halfWidth, halfWidth);
                float randomZ = Random.Range(-halfLength, halfLength);

                return new Vector3(center.x + randomX, 0, center.z + randomZ);
            }
        }
    }