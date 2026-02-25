using System.Collections;
using UnityEngine;

public class HitFlashSwap : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = 0.08f;

    private Material[][] _originalMats;

    private void Awake()
    {
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<Renderer>(true);

        _originalMats = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
            _originalMats[i] = renderers[i].sharedMaterials;
    }

    public void Flash()
    {
        if (flashMaterial == null)
        {
            Debug.LogWarning("HitFlashSwap: flashMaterial not set!");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // swap to flash mats
        for (int i = 0; i < renderers.Length; i++)
        {
            var mats = renderers[i].materials; // creates instances (ok for short flash)
            for (int m = 0; m < mats.Length; m++)
                mats[m] = flashMaterial;

            renderers[i].materials = mats;
        }

        yield return new WaitForSeconds(flashDuration);

        // restore
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].sharedMaterials = _originalMats[i];
    }
}
