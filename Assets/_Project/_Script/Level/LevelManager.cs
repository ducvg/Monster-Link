using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public Dictionary<int, BoardManager> BoardLevels { get; private set; }
    public int CurrentLevel { get; private set; }

    [SerializeField] private PlayerDataProfileSO playerDataProfile;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnInit()
    {
        (int startLevel, int endLevel) = LevelLoader.GetRangeOfLevel(playerDataProfile.PlayerData.LastPlayedLevel);
        BoardLevels = LevelLoader.LoadBoardLevelRange(startLevel, endLevel);
    }

    public void SpawnLevel(int level)
    {
        playerDataProfile.PlayerData.LastPlayedLevel = level;
        CurrentLevel = level;
        TryAddLevel(level);
        Instantiate(BoardLevels[level], Vector3.zero, Quaternion.identity);
    }

    private void OnLevelComplete()
    {
        playerDataProfile.PlayerData.MaxCompletedLevel = Mathf.Max(playerDataProfile.PlayerData.MaxCompletedLevel, CurrentLevel);
        playerDataProfile.Save();
    }

    public void TryAddLevel(int level)
    {
        if(!BoardLevels.ContainsKey(level) || BoardLevels[level] == null)
        {
            BoardLevels[level] = LevelLoader.LoadBoardLevel(level);
        }
    }

    private void OnEnable()
    {
        GameState.OnGameWon += OnLevelComplete;
    }

    private void OnDisable()
    {
        GameState.OnGameWon -= OnLevelComplete;
    }
}
