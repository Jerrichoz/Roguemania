using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float floatUpSpeed = 1.5f;
    [SerializeField] private float lifetime = 0.8f;

    private float _t;
    private Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }
    public void SetValue(float amount)
    {
        if (text != null)
            text.text = Mathf.CeilToInt(amount).ToString();
    }

    private void Update()
    {
        _t += Time.deltaTime;
        transform.position += Vector3.up * floatUpSpeed * Time.deltaTime;

        // Zur Kamera ausrichten
        if (cam != null)
        {
            transform.forward = cam.transform.forward;
        }
        if (_t >= lifetime)
            Destroy(gameObject);
    }
}