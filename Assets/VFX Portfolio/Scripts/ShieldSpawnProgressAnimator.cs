// ShieldSpawnProgressAnimator.cs

using System.Collections;
using UnityEngine;

public class ShieldSpawnProgressAnimator : MonoBehaviour
{
    [Header("Shield Renderers")]
    [Tooltip("Renderers using the EnergyShield and EdgeGlow shaders.")]
    [SerializeField] private Renderer[] shieldRenderers;

    [Header("Shader Property")]
    [Tooltip("Shader float property used to reveal the shield.")]
    [SerializeField] private string spawnProgressProperty = "_SpawnProgress";

    [Header("Animation Settings")]
    [Tooltip("How long the shield takes to reveal from bottom to top.")]
    [SerializeField] private float spawnDuration = 0.75f;

    [Tooltip("Animation curve for the spawn reveal. X = time, Y = spawn progress.")]
    [SerializeField] private AnimationCurve spawnCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Tooltip("If true, _SpawnProgress is reset to 0 when this object is disabled.")]
    [SerializeField] private bool resetOnDisable = true;

    [Header("Spawn Rotation")]
    [Tooltip("The object whose local rotation should be forced when the shield activates.")]
    [SerializeField] private Transform objectToRotate;

    [Tooltip("Local rotation applied to the target object when the shield activates.")]
    [SerializeField] private Vector3 spawnLocalEulerRotation = new Vector3(8f, 0f, -7f);

    private MaterialPropertyBlock propertyBlock;
    private Coroutine spawnRoutine;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();


    }

    private void OnEnable()
    {
        if (propertyBlock == null)
            propertyBlock = new MaterialPropertyBlock();

        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        ApplySpawnRotation();

        SetSpawnProgress(0f);
        spawnRoutine = StartCoroutine(AnimateSpawnProgress());
    }

    private void OnDisable()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        if (resetOnDisable)
            SetSpawnProgress(0f);
    }

    private IEnumerator AnimateSpawnProgress()
    {
        float timer = 0f;

        while (timer < spawnDuration)
        {
            timer += Time.deltaTime;

            float normalizedTime = Mathf.Clamp01(timer / spawnDuration);
            float progress = spawnCurve.Evaluate(normalizedTime);

            SetSpawnProgress(progress);

            yield return null;
        }

        SetSpawnProgress(1f);
        spawnRoutine = null;
    }

    private void SetSpawnProgress(float value)
    {
        if (shieldRenderers == null)
            return;

        foreach (Renderer shieldRenderer in shieldRenderers)
        {
            if (shieldRenderer == null)
                continue;

            shieldRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat(spawnProgressProperty, value);
            shieldRenderer.SetPropertyBlock(propertyBlock);
        }
    }

    private void ApplySpawnRotation()
    {
        if (objectToRotate == null)
            return;

        objectToRotate.localRotation = Quaternion.Euler(spawnLocalEulerRotation);
    }
}