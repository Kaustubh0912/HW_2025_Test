using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    SpeedBoost,
    Invincibility
}

[Serializable]
public class PowerUpConfig
{
    public PowerUpType type;
    public GameObject powerUpPrefab; 
    public float duration = 5f;
    public float speedMultiplier = 2f;
}

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private List<PowerUpConfig> powerUps;
    [SerializeField] [Range(0f, 1f)] private float spawnChance = 0.3f;

    public event EventHandler OnPowerUpCollected;

    private Dictionary<PowerUpType, float> activePowerUpTimers = new Dictionary<PowerUpType, float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TrySpawnPowerUp(Pulpit pulpit)
    {
        if (UnityEngine.Random.value < spawnChance)
        {
            SpawnRandomPowerUp(pulpit);
        }
    }

    private void SpawnRandomPowerUp(Pulpit pulpit)
    {
        if (powerUps == null || powerUps.Count == 0) return;

        PowerUpConfig config = powerUps[UnityEngine.Random.Range(0, powerUps.Count)];
        
        if (config.powerUpPrefab != null)
        {
            Vector3 spawnPos = pulpit.transform.position + Vector3.up;
            Collider pulpitCollider = pulpit.GetComponent<Collider>();
            if (pulpitCollider != null)
            {
                spawnPos.y = pulpitCollider.bounds.max.y + .5f;
            }

            GameObject go = Instantiate(config.powerUpPrefab, spawnPos, Quaternion.identity);
            
            PowerUp powerUpScript = go.GetComponent<PowerUp>();
            if (powerUpScript == null)
            {
                powerUpScript = go.AddComponent<PowerUp>();
            }
            
            powerUpScript.Initialize(config.type, pulpit);
        }
    }

    private void Update()
    {
        List<PowerUpType> toRemove = new List<PowerUpType>();
        
        List<PowerUpType> keys = new List<PowerUpType>(activePowerUpTimers.Keys);
        foreach (var type in keys)
        {
            activePowerUpTimers[type] -= Time.deltaTime;
            if (activePowerUpTimers[type] <= 0)
            {
                toRemove.Add(type);
            }
        }

        foreach (var type in toRemove)
        {
            PowerUpConfig config = powerUps.Find(p => p.type == type);
            if (config != null)
            {
                EndEffect(config);
            }
            activePowerUpTimers.Remove(type);
        }
    }

    public void ActivatePowerUp(PowerUpType type)
    {
        PowerUpConfig config = powerUps.Find(p => p.type == type);
        if (config == null) return;

        OnPowerUpCollected?.Invoke(this, EventArgs.Empty);

        if (!activePowerUpTimers.ContainsKey(type))
        {
            StartEffect(config);
        }

        activePowerUpTimers[type] = config.duration;
    }

    public Dictionary<PowerUpType, float> GetActivePowerUps()
    {
        return activePowerUpTimers;
    }

    private void StartEffect(PowerUpConfig config)
    {
        switch (config.type)
        {
            case PowerUpType.SpeedBoost:
                PlayerController.Instance.SetSpeedMultiplier(config.speedMultiplier);
                break;
            case PowerUpType.Invincibility:
                PlayerController.Instance.SetInvincible(true);
                break;
        }
    }

    private void EndEffect(PowerUpConfig config)
    {
        switch (config.type)
        {
            case PowerUpType.SpeedBoost:
                PlayerController.Instance.SetSpeedMultiplier(1f);
                break;
            case PowerUpType.Invincibility:
                PlayerController.Instance.SetInvincible(false);
                break;
        }
    }
}
