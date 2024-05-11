using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private Camera _camera;
    private int _raycastDistance = 100;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var fromCameraRay = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(fromCameraRay, out RaycastHit hitInfo, _raycastDistance))
            {
                if (hitInfo.collider.TryGetComponent(out Subdividing subdivided))
                {
                    subdivided.Divide();
                }
            }
        }
    }
}
