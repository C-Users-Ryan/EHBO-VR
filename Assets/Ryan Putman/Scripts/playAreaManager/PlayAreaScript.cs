using EHBOVR.Calibration;
using EHBOVR.PlayerCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaScript : MonoBehaviour
{
    [SerializeField] private PlayAreaBehaviour playAreaBehaviour; // To get the dimensions of the play area
    [SerializeField] private GameObject prefabToSpawn; // The prefab to spawn in the play area
    [SerializeField] private BoundIndicator boundIndicatorHorizontal; // Indicator for horizontal bounds
    [SerializeField] private BoundIndicator boundIndicatorVertical;   // Indicator for vertical bounds

    private Vector2 playAreaBounds;

    private void Start()
    {
        // Measure the play area dimensions at the start
        playAreaBounds = playAreaBehaviour.GetDimensions();

        // Update the bound indicators to match the play area size
        UpdateBoundIndicators(playAreaBounds);

        // Generate a random spawn point within the detected bounds
        Vector3 spawnPoint = GenerateSpawnPoint(playAreaBounds);

        // Instantiate the prefab at the generated spawn point
        Instantiate(prefabToSpawn, spawnPoint, Quaternion.identity);
    }

    /// <summary>
    /// Updates the dimensions of the bound indicators based on the play area size.
    /// </summary>
    /// <param name="bounds">The dimensions of the play area.</param>
    private void UpdateBoundIndicators(Vector2 bounds)
    {
        // Horizontal bounds visualization
        boundIndicatorHorizontal.SetDimension(bounds.x, bounds.y * 0.5f);

        // Vertical bounds visualization
        boundIndicatorVertical.SetDimension(bounds.y, bounds.x * 0.5f);

        // Ensure the indicators are set to valid since there are no restrictions now
        boundIndicatorHorizontal.SetValid(true);
        boundIndicatorVertical.SetValid(true);
    }

    /// <summary>
    /// Generates a random spawn point within the play area bounds.
    /// </summary>
    /// <param name="bounds">The bounds of the play area.</param>
    /// <returns>A Vector3 position within the play area.</returns>
    private Vector3 GenerateSpawnPoint(Vector2 bounds)
    {
        float x = Random.Range(-bounds.x / 2f, bounds.x / 2f);
        float z = Random.Range(-bounds.y / 2f, bounds.y / 2f);

        // Assuming Y is at ground level
        return new Vector3(x, 0, z);
    }
}
