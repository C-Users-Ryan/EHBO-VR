using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace EHBOVR.Calibration
{
    public class BoundIndicator : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textMesh;
        [SerializeField] private Transform arrowBody;
        [SerializeField] private Transform arrowTipLeft;
        [SerializeField] private Transform arrowTipRight;

        [SerializeField] private Color validColor;
        [SerializeField] private Color invalidColor;

        private float _breadthTarget;
        private bool _isValid;
        private MaterialPropertyBlock _materialProperties;
        private List<Renderer> _renderers;

        private void Awake()
        {
            _materialProperties = new MaterialPropertyBlock();
            _renderers = GetComponentsInChildren<Renderer>().ToList();
        }

        public void SetValid(bool newState)
        {
            if (_isValid != newState)
            {
                _isValid = newState;
                UpdateValidation();
            }
        }

        private void UpdateValidation()
        {
            if (_isValid)
            { // Present bound indicator as correctly configured
                textMesh.text = "Deze as is minstens " + _breadthTarget + " meter breed";
                UpdateVisuals( validColor);
            }
            else
            { // Present bound indicator as incorrectly configured
                textMesh.text = "Deze as moet minstens " + _breadthTarget + " meter breed zijn";
                UpdateVisuals(invalidColor);
            }
        }

        private void UpdateVisuals(Color color)
        {
            _materialProperties.SetColor("_Color", color);
            foreach (Renderer r in _renderers)
            {
                r.SetPropertyBlock(_materialProperties);
            }
        }

        public void SetDimension(float breadth, float distance)
        {
            _breadthTarget = breadth;
            UpdateValidation();
            
            transform.position *= distance;
            arrowTipLeft.localPosition = new Vector3(-breadth * .5f, .25f, 0);
            arrowTipRight.localPosition = new Vector3(breadth * .5f, .25f, 0);
            arrowBody.localScale = new Vector3(.1f, breadth * .5f - .2f, .1f);
        }
    }
}