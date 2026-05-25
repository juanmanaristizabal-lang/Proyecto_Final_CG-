using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public class MissionListWrapper
{
    public List<MissionData> missions;
}



public class MisionManager : MonoBehaviour
{

    public static MisionManager Instance { get; private set; }

    private List<MissionData> _missions = new List<MissionData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadMissions();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetActiveMissionForScene(scene.name);
    }


    private void LoadMissions()
    {
        var data = JsonManager.Instance.Data;

        if (data?.missions == null)
        {
            Debug.LogWarning("[MissionManager] No hay misiones en game_data.json");
            return;
        }

        _missions = data.missions;
        Debug.Log($"[MissionManager] {_missions.Count} misiones cargadas desde game_data.json");
        SetActiveMissionForScene(SceneManager.GetActiveScene().name);
    }

    private void SetActiveMissionForScene(string sceneName)
    {
        foreach (var m in _missions)
        {
            if (m.scene == sceneName && !m.completed)
            {
                UIManager.Instance.UpdateMissionText(m.title, m.description);
                break;
            }
        }
    }
    public void CompleteMission(string missionId)
    {
        foreach (var m in _missions)
        {
            if (m.id == missionId && !m.Completed)
            {
                m.Completed = true;
                GameDataStructure.Instance.LogEvent($"Misión completada: {m.title}");
                GameManager.Instance?.SaveGame();
                SetActiveMissionForScene(SceneManager.GetActiveScene().name);
                return;
            }
        }
    }
}