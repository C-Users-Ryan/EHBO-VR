using System.Collections;
using EHBOVR.SceneManagement;
using UnityEngine;

namespace EHBOVR.PlayerCharacter
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private GameObject characterCollider;
        [SerializeField]
        private ScreenOverlay overlay;

        public bool OverlayInTransition => overlay.InTransition;

        [SerializeField] public float fadeTransitionTime = 1f;

        void Awake()
        {
            // Register the currently active player entity to the persistent manager
            PersistentManager.Instance.RegisterPlayer(this);
        }

        void Start()
        {
            OnSpawn();
        }
        
        void Update()
        {
            Transform colliderTransform = characterCollider.transform;
            colliderTransform.rotation = Quaternion.identity;
            Vector3 eyeCenterTransform = colliderTransform.parent.position;
            colliderTransform.position = new Vector3( eyeCenterTransform.x, 1f, eyeCenterTransform.z );
        }

        public void OnSpawn()
        {
            overlay.FadeIn( fadeTransitionTime );
        }
        
        public void OnDespawnStart()
        {
            overlay.FadeOut( fadeTransitionTime );
        }

        public void OnPlayerKill()
        {
            overlay.SetColor( new Color( 1f, 0f, 0f, 0.5f ));
        }
    }
}
