using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float floatUpSpeed = 1.5f;
    [SerializeField] private float lifetime = 0.8f;

    private float _t;

    public void SetValue(float amount)
    {
        if (text != null)
            text.text = Mathf.CeilToInt(amount).ToString();
    }

    private void Update()
    {
        _t += Time.deltaTime;
        transform.position += Vector3.up * floatUpSpeed * Time.deltaTime;

        if (_t >= lifetime)
            Destroy(gameObject);
    }
}