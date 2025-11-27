using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulpitManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pulpitPrefab;
    [SerializeField] private Transform pulpitParent;

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 firstPulpitPosition = Vector3.zero;
    [SerializeField] private float pulpitSize = 9f; 
    [SerializeField] private int maxSimultaneousPulpits = 2;

    private List<Pulpit> activePulpits = new List<Pulpit>();
    private Vector3 lastPulpitPosition;
    private Vector3 lastSpawnDirection;
    private bool gameStarted = false;
    private bool isSpawnPending = false;

    private float minDestroyTime;
    private float maxDestroyTime;
    private float spawnTime;

    private void Start()
    {
        StartCoroutine(InitializeManager());
    }

    private IEnumerator InitializeManager()
    {
        while (GameConfig.Instance == null || !GameConfig.Instance.IsConfigLoaded)
        {
            yield return null;
        }

        minDestroyTime = GameConfig.Instance.GetMinPulpitDestroyTime();
        maxDestroyTime = GameConfig.Instance.GetMaxPulpitDestroyTime();
        spawnTime = GameConfig.Instance.GetPulpitSpawnTime();

        SpawnInitialPulpit();
        gameStarted = true;
    }

    private void SpawnInitialPulpit()
    {
        lastPulpitPosition = firstPulpitPosition;
        SpawnPulpit(firstPulpitPosition);
    }

    private void SpawnPulpit(Vector3 position)
    {
        if (activePulpits.Count >= maxSimultaneousPulpits)
        {
            return;
        }

        GameObject pulpitObj = Instantiate(pulpitPrefab, position, Quaternion.identity, pulpitParent);
        Pulpit pulpit = pulpitObj.GetComponent<Pulpit>();

        if (pulpit == null)
        {
            Debug.LogError("Pulpit prefab doesn't have Pulpit component!");
            Destroy(pulpitObj);
            return;
        }

        float lifetime = GameConfig.Instance.GetRandomPulpitLifetime();
        pulpit.Initialize(lifetime);

        pulpit.OnPulpitDestroyed += OnPulpitDestroyed;

        activePulpits.Add(pulpit);
        lastPulpitPosition = position;

        StartCoroutine(ScheduleNextSpawn(lifetime));
    }

    private IEnumerator ScheduleNextSpawn(float pulpitLifetime)
    {
        float waitTime = pulpitLifetime - spawnTime;

        if (waitTime < 0)
        {
            waitTime = 0;
        }

        yield return new WaitForSeconds(waitTime);

        if (gameStarted)
        {
            isSpawnPending = true;
            TrySpawnNextPulpit();
        }
    }

    private void TrySpawnNextPulpit()
    {
        if (isSpawnPending && activePulpits.Count < maxSimultaneousPulpits)
        {
            isSpawnPending = false;
            Vector3 nextPosition = GetNextPulpitPosition();
            SpawnPulpit(nextPosition);
        }
    }

    private Vector3 GetNextPulpitPosition()
    {
        List<Vector3> possiblePositions = new List<Vector3>
        {
            lastPulpitPosition + Vector3.forward * pulpitSize,
            lastPulpitPosition + Vector3.back * pulpitSize,
            lastPulpitPosition + Vector3.right * pulpitSize,
            lastPulpitPosition + Vector3.left * pulpitSize
        };

        possiblePositions.RemoveAll(pos => IsPulpitAtPosition(pos));

        if (possiblePositions.Count == 0)
        {
            int randomDir = Random.Range(0, 4);
            Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
            return lastPulpitPosition + directions[randomDir] * pulpitSize;
        }

        Vector3 chosenPosition = possiblePositions[Random.Range(0, possiblePositions.Count)];
        lastSpawnDirection = (chosenPosition - lastPulpitPosition).normalized;
        return chosenPosition;
    }

    private bool IsPulpitAtPosition(Vector3 position)
    {
        foreach (Pulpit pulpit in activePulpits)
        {
            if (pulpit != null && Vector3.Distance(pulpit.transform.position, position) < 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    private void OnPulpitDestroyed(Pulpit pulpit)
    {
        if (activePulpits.Contains(pulpit))
        {
            activePulpits.Remove(pulpit);
            TrySpawnNextPulpit();
        }
    }
}