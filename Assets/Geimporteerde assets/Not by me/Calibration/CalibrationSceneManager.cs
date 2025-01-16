using EHBOVR.PlayerCharacter;
using EHBOVR.SceneManagement;
using UnityEngine;

namespace EHBOVR.Calibration
{
    public class CalibrationSceneManager : MonoBehaviour
    {
        [SerializeField] private PlayAreaBehaviour playAreaBehaviour;

        [SerializeField] private BoundIndicator boundIndicatorHorizontal;
        [SerializeField] private BoundIndicator boundIndicatorVertical;

        void Start()
        {
            // Dynamically set the bound indicators based on the detected play area size
            UpdateBoundIndicators();
        }

        void Update()
        {
            // Keep indicators visible until the game starts
            if (!playAreaBehaviour.gameStarted)
            {
                UpdateBoundIndicators();
            }
            else
            {
/*                boundIndicatorHorizontal.SetVisibility(false);
                boundIndicatorVertical.SetVisibility(false);*/
            }
        }

        private void UpdateBoundIndicators()
        {
            // Get the current play area dimensions
            Vector2 bounds = playAreaBehaviour.GetDimensions();

            // Set dimensions for the indicators without moving their positions
            boundIndicatorHorizontal.SetDimension(bounds.x, bounds.y * 0.5f);
            boundIndicatorVertical.SetDimension(bounds.y, bounds.x * 0.5f);

            // Keep the indicators valid and at their current positions
            boundIndicatorHorizontal.SetValid(true);
            boundIndicatorVertical.SetValid(true);
        }
    }
}
