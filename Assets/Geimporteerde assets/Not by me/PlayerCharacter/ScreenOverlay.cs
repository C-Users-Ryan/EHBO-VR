using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace EHBOVR.PlayerCharacter
{
    public class ScreenOverlay : MonoBehaviour
    {
        private MeshRenderer _renderer;
        private Material _material;

        public bool InTransition { private set; get; }        
        
        void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _material = _renderer.material;
            _renderer.bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);
        }
        
        public void FadeIn ( float speed )
        {
            StartCoroutine(OverlayTransition( Color.clear, Color.black, speed ));
        }
        
        public void FadeOut ( float speed )
        {
            StartCoroutine(OverlayTransition( Color.black, speed ));
        }

        public void SetColor( Color color )
        {
            _material.SetColor( "_Color", color );
        }
        
        private IEnumerator OverlayTransition( Color targetColor, float transitionTime = 0f )
        {
            return OverlayTransition( targetColor, _material.GetColor("_Color"), transitionTime);
        }
        
        private IEnumerator OverlayTransition( Color targetColor, Color startColor, float transitionTime = 0f )
        {
            InTransition = true;
            float progress = 0f;

            while (progress < 1f)
            {
                if (transitionTime <= 0f) // In the case of the transition time being empty or unprovided, skip the gradual transition
                    progress = 1f;
                else
                    progress += Time.deltaTime / transitionTime;
                
                _material.SetColor("_Color", Color.Lerp(startColor, targetColor, progress));
                
                yield return null;
            }

            InTransition = false;
        }
    }
}
