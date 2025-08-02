using Unity.VisualScripting;
using UnityEngine;

public class LevelSelectionUI : Singleton<LevelSelectionUI>
{
    [SerializeField] private LevelSlot[] levelSlots;

    [SerializeField] private PlayerDataProfileSO playerDataProfile;

    void Start()
    {
        (int startLevel, _) = LevelLoader.GetRangeOfLevel(playerDataProfile.PlayerData.LastPlayedLevel);
        UpdateLevelSlots(startLevel);
    }



    public void LoadUp()
    {
        var startLevel = levelSlots[^1].LevelIndex+1;
        UpdateLevelSlots(startLevel);

    }

    public void LoadDown()
    {
        var startLevel = levelSlots[0].LevelIndex-5;
        UpdateLevelSlots(startLevel);

    }

    private void UpdateLevelSlots(int startLevel)
    {
        for (int i = 0; i < 5; i++)
        {
            var levelIndex = startLevel + i;
            levelSlots[i].LevelIndex = levelIndex;
            levelSlots[i].UpdateLevelText();
            levelSlots[i].ToggleSlot(playerDataProfile.PlayerData.MaxLevel + 1 >= levelIndex);
        }
    }
}
