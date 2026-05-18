// EmissionOnCollision.cs

using System.Collections;
using UnityEngine;

public class EmissionOnCollision : MonoBehaviour
{
    [Header("Collision Filter")]
    [SerializeField] private string requiredCollisionTag = "Environment";
    [SerializeField] private bool activateOnlyOnce = true;

    [Header("Emission Target")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private int materialIndex = 0;

    [Header("Emission Settings")]
    [SerializeField] private Color emissionColor = Color.cyan;
    [SerializeField] private float targetEmissionIntensity = 5f;
    [SerializeField] private float rampDuration = 0.5f;

    [Header("Shader Property")]
    [SerializeField] private string emissionColorProperty = "_EmissionColor";

    private Material targetMaterial;
    private Coroutine emissionRoutine;
    private bool hasActivated;

    private void Awake()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        if (targetRenderer != null)
        {
            Material[] materials = targetRenderer.materials;

            if (materialIndex >= 0 && materialIndex < materials.Length)
            {
                targetMaterial = materials[materialIndex];
                targetMaterial.EnableKeyword("_EMISSION");
                SetEmissionIntensity(0f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(requiredCollisionTag))
            return;

        if (activateOnlyOnce && hasActivated)
            return;

        hasActivated = true;

        if (emissionRoutine != null)
            StopCoroutine(emissionRoutine);

        emissionRoutine = StartCoroutine(RampEmission());
    }

    private IEnumerator RampEmission()
    {
        float timer = 0f;

        while (timer < rampDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / rampDuration);
            float intensity = Mathf.Lerp(0f, targetEmissionIntensity, t);

            SetEmissionIntensity(intensity);

            yield return null;
        }

        SetEmissionIntensity(targetEmissionIntensity);
        emissionRoutine = null;
    }

    private void SetEmissionIntensity(float intensity)
    {
        if (targetMaterial == null)
            return;

        Color finalEmissionColor = emissionColor * intensity;
        targetMaterial.SetColor(emissionColorProperty, finalEmissionColor);
    }
}