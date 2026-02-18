using System.Collections;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private float flashDuration = 0.08f;
    [SerializeField] private float emissionIntensity = 3.5f; // höher = stärker

    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorProp = Shader.PropertyToID("_Color");
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private MaterialPropertyBlock _mpb;

    private Color[] _origBase;
    private Color[] _origEmission;
    private bool[] _hasBase;
    private bool[] _hasColor;
    private bool[] _hasEmission;

    private void Awake()
    {
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<Renderer>(true);

        _mpb = new MaterialPropertyBlock();

        int n = renderers.Length;
        _origBase = new Color[n];
        _origEmission = new Color[n];
        _hasBase = new bool[n];
        _hasColor = new bool[n];
        _hasEmission = new bool[n];

        for (int i = 0; i < n; i++)
        {
            var mat = renderers[i].sharedMaterial;
            if (mat == null) continue;

            _hasBase[i] = mat.HasProperty(BaseColor);
            _hasColor[i] = mat.HasProperty(ColorProp);
            _hasEmission[i] = mat.HasProperty(EmissionColor);

            if (_hasBase[i]) _origBase[i] = mat.GetColor(BaseColor);
            else if (_hasColor[i]) _origBase[i] = mat.GetColor(ColorProp);
            else _origBase[i] = Color.white;

            _origEmission[i] = _hasEmission[i] ? mat.GetColor(EmissionColor) : Color.black;
        }
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // ON
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].GetPropertyBlock(_mpb);

            // optional: base color heller
            if (_hasBase[i]) _mpb.SetColor(BaseColor, Color.white);
            else if (_hasColor[i]) _mpb.SetColor(ColorProp, Color.white);

            // emission flash (sichtbar!)
            if (_hasEmission[i])
                _mpb.SetColor(EmissionColor, Color.white * emissionIntensity);

            renderers[i].SetPropertyBlock(_mpb);
        }

        yield return new WaitForSeconds(flashDuration);

        // OFF (restore)
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].GetPropertyBlock(_mpb);

            if (_hasBase[i]) _mpb.SetColor(BaseColor, _origBase[i]);
            else if (_hasColor[i]) _mpb.SetColor(ColorProp, _origBase[i]);

            if (_hasEmission[i])
                _mpb.SetColor(EmissionColor, _origEmission[i]);

            renderers[i].SetPropertyBlock(_mpb);
        }
    }
}
