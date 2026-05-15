using UnityEngine;

public class TargetHighlighter : MonoBehaviour
{
    [SerializeField] private TurretController turretController;

    private CubeHighlight _currentCube;

    private void Update()
    {
        ResetCurrentHighlight();

        if (!turretController.HasHit)
            return;

        CubeHighlight cubeHighlight = turretController
            .CurrentHit
            .collider
            .GetComponent<CubeHighlight>();

        if (cubeHighlight == null)
            return;

        _currentCube = cubeHighlight;
        _currentCube.Highlight();
    }

    private void ResetCurrentHighlight()
    {
        if (_currentCube == null)
            return;

        _currentCube.SetDefaultColor();
        _currentCube = null;
    }
}