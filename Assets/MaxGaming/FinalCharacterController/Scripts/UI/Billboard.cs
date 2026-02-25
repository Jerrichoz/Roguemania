using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (cam == null) return;
        transform.forward = cam.transform.forward; // simple billboard
    }
}