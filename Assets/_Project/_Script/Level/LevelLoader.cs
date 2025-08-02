using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class LevelLoader 
{
    public static BoardManager LoadBoardLevel(int level)
    {
        StringBuilder sb = new();
        sb.Append("Levels/Level_");sb.Append(level);

        BoardManager boardLevel = Resources.Load<BoardManager>(sb.ToString());
        if (boardLevel == null)
        {
            Debug.LogError("Board level " + level + " not found at " + sb.ToString());
            return null;
        }

        return boardLevel;
    }

    public static Dictionary<int, BoardManager> LoadBoardLevelRange(int startLevel, int endLevel)
    {
        Dictionary<int, BoardManager> boardLevels = new();
        StringBuilder sb = new();
        for (int i = startLevel; i <= endLevel; i++)
        {
            sb.Clear();
            sb.Append("Levels/Level_");sb.Append(i);
            BoardManager boardLevel = Resources.Load<BoardManager>(sb.ToString());
            if (boardLevel == null)
            {
                Debug.LogError("Board level " + i + " not found at " + sb.ToString());
                continue;
            }
            boardLevels.Add(i, boardLevel);
        }

        return boardLevels;
    }

    public static (int startLevel, int endLevel) GetRangeOfLevel(int level, int range = 5)
    {
        int rangeIndex = ((level - 1) / range) + 1;  //int division auto floor down
        int startLevel = ((rangeIndex - 1) * range) + 1;
        int endLevel = rangeIndex * range;
        return (startLevel, endLevel);
    }
}
