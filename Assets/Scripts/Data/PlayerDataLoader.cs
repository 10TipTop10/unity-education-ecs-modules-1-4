using System.Collections;
using System.IO;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDataLoader : MonoBehaviour
{
    [SerializeField] private string _downloadUrl;
    private const string FileName = "PlayerData.json";
    private string LocalFilePath => Path.Combine(Application.persistentDataPath, FileName);


    private void Start()
    {
        StartCoroutine(DownloadPlayerData());
    }

    private PlayerData LoadPlayerDataFromFile()
    {
        if (!File.Exists(LocalFilePath)) return null;

        string json = File.ReadAllText(LocalFilePath);

        PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);

        if (playerData == null) return null;

        return playerData;
    }

    private void CreatePlayerDataUpdate(PlayerData loadedPlayerData)
    {
        World world = World.DefaultGameObjectInjectionWorld;
        EntityManager entityManager = world.EntityManager;
        Entity updateEntity = entityManager.CreateEntity();

        entityManager.AddComponentData(updateEntity, new LoadedPlayerDataComponent
        {
            MaxHealth = loadedPlayerData.MaxHealth,
            MoveSpeed = loadedPlayerData.MoveSpeed,
            DashDistance = loadedPlayerData.DashDistance
        });
    }

    private IEnumerator DownloadPlayerData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(_downloadUrl))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{request.error}");
                yield break;
            }

            File.WriteAllText(LocalFilePath, request.downloadHandler.text);
        }

        PlayerData loadedPlayerData = LoadPlayerDataFromFile();
        if (loadedPlayerData == null) yield break;

        CreatePlayerDataUpdate(loadedPlayerData);

        Debug.Log($"Чистые данные из PlayerData (Google docs). Здоровье:{loadedPlayerData.MaxHealth} Скорость:{loadedPlayerData.MoveSpeed} Дальность рывка:{loadedPlayerData.DashDistance}");
    }
}
