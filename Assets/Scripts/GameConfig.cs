using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class PlayerData
{
    public float speed;
}

[Serializable]
public class PulpitData
{
    public float min_pulpit_destroy_time;
    public float max_pulpit_destroy_time;
    public float pulpit_spawn_time;
}

[Serializable]
public class GameConfigData
{
    public PlayerData player_data;
    public PulpitData pulpit_data;
}

public class GameConfig : MonoBehaviour
{
    public static GameConfig Instance { get; private set; }

    [Header("Config Sources (Priority: File > URL > Default)")]
    public TextAsset configFile;

    [SerializeField]private string configURL = "https://s3.ap-south-1.amazonaws.com/superstars.assetbundles.testbuild/doofus_game/doofus_diary.json";

    public GameConfigData configData;
    public bool IsConfigLoaded { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(LoadConfigWithPriority());
    }

    private IEnumerator LoadConfigWithPriority()
    {
        // Priority 1: Check for local file
        if (configFile != null)
        {
            LoadFromFile();
            yield break;
        }


        // Priority 2: Try loading from Resources folder
        TextAsset resourceFile = Resources.Load<TextAsset>("doofus_diary");
        if (resourceFile != null)
        {
            configFile = resourceFile;
            LoadFromFile();
            yield break;
        }


        // Priority 3: Try fetching from URL
        if (!string.IsNullOrEmpty(configURL))
        {
            yield return StartCoroutine(FetchFromURL());

            if (IsConfigLoaded)
            {
                yield break;
            }
        }

        // Priority 4: Fallback to default values
        LoadDefaultConfig();
    }

    private void LoadFromFile()
    {
        try
        {
            string jsonText = configFile.text;
            Debug.Log("Config file content: " + jsonText);

            configData = JsonUtility.FromJson<GameConfigData>(jsonText);
            IsConfigLoaded = true;

        }
        catch (Exception e)
        {
            Debug.LogError("Failed to parse JSON from file: " + e.Message);
            StartCoroutine(FetchFromURL());
        }
    }

    private IEnumerator FetchFromURL()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(configURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonText = request.downloadHandler.text;

                try
                {
                    configData = JsonUtility.FromJson<GameConfigData>(jsonText);
                    IsConfigLoaded = true;
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to parse JSON from URL: " + e.Message);
                    LoadDefaultConfig();
                }
            }
            else
            {
                Debug.LogError("Failed to fetch config from URL: " + request.error);
                LoadDefaultConfig();
            }
        }
    }

    private void LoadDefaultConfig()
    {
        Debug.LogWarning("Using default configuration values");
        configData = new GameConfigData
        {
            player_data = new PlayerData { speed = 3f },
            pulpit_data = new PulpitData
            {
                min_pulpit_destroy_time = 4f,
                max_pulpit_destroy_time = 5f,
                pulpit_spawn_time = 2.5f
            }
        };
        IsConfigLoaded = true;
    }


    public float GetPlayerSpeed()
    {
        return configData?.player_data.speed ?? 3f;
    }

    public float GetMinPulpitDestroyTime()
    {
        return configData?.pulpit_data.min_pulpit_destroy_time ?? 4f;
    }

    public float GetMaxPulpitDestroyTime()
    {
        return configData?.pulpit_data.max_pulpit_destroy_time ?? 5f;
    }

    public float GetPulpitSpawnTime()
    {
        return configData?.pulpit_data.pulpit_spawn_time ?? 2.5f;
    }

    public float GetRandomPulpitLifetime()
    {
        return UnityEngine.Random.Range(GetMinPulpitDestroyTime(), GetMaxPulpitDestroyTime());
    }
}