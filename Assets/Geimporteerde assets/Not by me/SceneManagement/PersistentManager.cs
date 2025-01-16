using System;
using System.Collections;
using EHBOVR.PlayerCharacter;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EHBOVR.SceneManagement
{
    /// <summary>
    /// This singleton implementation handles all forms of scene transitions
    /// </summary>
    public class PersistentManager : MonoBehaviour
    {
        // Singleton properties
        private static PersistentManager _instance;
        public static PersistentManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    GameObject go = new GameObject("PersistentManager");
                    go.AddComponent<PersistentManager>();
                }
                return _instance;
            }
        }

        // Object references
        private Player _player;
        
        void Awake()
        {
            _instance = this;
        }

        public void RegisterPlayer(Player player)
        {
            _player = player;
        }
        
        public void LoadSuburbanLevel()
        {
            StartCoroutine(LoadScene("EHBOVR/Scenes/SuburbanScene/SuburbanLevel_Scene"));
        }
        
        // TODO: Configure scene loading without lag spike
        IEnumerator LoadScene(string levelName)
        {
            // Wait until player isn't still in a transition
            while (_player.OverlayInTransition)
                yield return null;
            
            // initiate transition
            _player.OnDespawnStart();
            
            // Start scene setup
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
            if (asyncLoad == null)
                throw new NullReferenceException("Referenced scene could not be found");
            asyncLoad.allowSceneActivation = false;

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                // Activate scene when both the async preload and overlay has completed
                if (asyncLoad.progress >= 0.9f && !_player.OverlayInTransition)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                
                yield return null;
            }
        }
    }
}
