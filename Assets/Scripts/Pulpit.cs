using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class Pulpit : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material warningMaterial;
    [SerializeField] private TextMeshPro timerText;

    [Header("Settings")]
    [SerializeField] private float warningTimeBeforeDestroy = 1f;

    private MeshRenderer meshRenderer;
    private float lifetime;
    private float elapsedTime;
    private bool isWarning;
    private bool isDestroying;

    public event Action<Pulpit> OnPulpitDestroyed;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize(float destroyTime)
    {
        lifetime = destroyTime;
        elapsedTime = 0f;
        isWarning = false;
        isDestroying = false;

        StartCoroutine(SpawnAnimation());
    }

    private void Update()
    {
        if (isDestroying) return;

        elapsedTime += Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = GetTimeRemaining().ToString("F1");
        }

        float timeRemaining = lifetime - elapsedTime;
        if (!isWarning && timeRemaining <= warningTimeBeforeDestroy)
        {
            isWarning = true;
            OnWarningState();
        }

        if (elapsedTime >= lifetime)
        {
            DestroyPulpit();
        }
    }

    private void OnWarningState()
    {
        if (timerText != null)
        {
            timerText.color = Color.red;
        }

        StartCoroutine(WarningAnimation());
    }

    private void DestroyPulpit()
    {
        if (isDestroying) return;
        isDestroying = true;

        OnPulpitDestroyed?.Invoke(this);

        StartCoroutine(DespawnAnimation());
    }

    public float GetTimeRemaining()
    {
        return Mathf.Max(0, lifetime - elapsedTime);
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public bool IsWarning()
    {
        return isWarning;
    }

    // ==================== ANIMATION METHODS ====================

    private IEnumerator SpawnAnimation()
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 targetScale = transform.localScale;
        transform.localScale = Vector3.zero;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float easeT = 1f - Mathf.Pow(1f - t, 3f);
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, easeT);
            yield return null;
        }

        transform.localScale = targetScale;
    }

    private IEnumerator WarningAnimation()
    {
        if (timerText == null) yield break;

        Vector3 originalScale = timerText.transform.localScale;
        float pulseSpeed = 10f;
        float pulseAmount = 0.2f;

        while (isWarning && !isDestroying)
        {
            float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            timerText.transform.localScale = originalScale * (1f + pulse);
            yield return null;
        }

        timerText.transform.localScale = originalScale;
    }

    private IEnumerator DespawnAnimation()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;
        Material mat = meshRenderer.material;
        Color originalColor = mat.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);

            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(1f, 0f, t);
            mat.color = newColor;

            yield return null;
        }

        Destroy(gameObject);
    }

}