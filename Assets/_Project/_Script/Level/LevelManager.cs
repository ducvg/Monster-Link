using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : PersistentSingleton<LevelManager>
{
    public Dictionary<int, BoardManager> BoardLevels { get; private set; }
    public int CurrentLevel { get; private set; }

    [SerializeField] private PlayerDataProfileSO playerDataProfile;

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
        playerDataProfile.PlayerData.MaxLevel = Mathf.Max(playerDataProfile.PlayerData.MaxLevel, CurrentLevel);
        playerDataProfile.Save();
    }

    public void TryAddLevel(int level)
    {
        if(!BoardLevels.ContainsKey(level) || BoardLevels[level] == null)
        {
            BoardLevels[level] = LevelLoader.LoadBoardLevel(level);
        }
    }

    public void AddLevelRange(int startLevel, int endLevel)
    {
        for (int i = startLevel; i <= endLevel; i++)
        {
            TryAddLevel(i);
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
