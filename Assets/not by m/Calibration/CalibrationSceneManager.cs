using EHBOVR.PlayerCharacter;
using EHBOVR.SceneManagement;
using UnityEngine;

namespace EHBOVR.Calibration
{
    public class CalibrationSceneManager : MonoBehaviour
    {
        [SerializeField] private PlayAreaBehaviour playAreaBehaviour;

        [SerializeField] private Vector2 minimumBounds;

        [SerializeField] private BoundIndicator boundIndicatorHorizontal;
        [SerializeField] private BoundIndicator boundIndicatorVertical;
        [SerializeField] private GameObject startButton;

        private bool _isValid;

        void Start()
        {
            boundIndicatorHorizontal.SetDimension(minimumBounds.x, minimumBounds.y * .5f);
            boundIndicatorVertical.SetDimension(minimumBounds.y, minimumBounds.x * .5f);
        }

        // TODO: Verify bounds based on some bounds-changed-event rather than update
        private void Update()
        {
            VerifyBounds();
            ToggleStartButton(_isValid);
        }

        private void ToggleStartButton(bool activate)
        {
            startButton.SetActive(activate);
        }

        private void VerifyBounds()
        {
            Vector2 bounds = playAreaBehaviour.GetDimensions();

            bool isHorizontalValid = bounds.x >= minimumBounds.x;
            bool isVerticalValid = bounds.y >= minimumBounds.y;
            
            boundIndicatorHorizontal.SetValid(isHorizontalValid);
            boundIndicatorVertical.SetValid(isVerticalValid);

            _isValid = isVerticalValid && isHorizontalValid;
        }

        // TODO: Currently unused, ought to be bound to some 'OnReorient' event.
        public void OnTransitionButtonPressed()
        {
            PersistentManager.Instance.LoadSuburbanLevel();
        }
    }
}
