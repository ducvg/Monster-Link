using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUI : Singleton<LevelSelectionUI>
{
    [SerializeField] private LevelSlot[] levelSlots;
    [SerializeField] private Button loadUpButton, loadDownButton;
    [SerializeField] private PlayerDataProfileSO playerDataProfile;

    void Start()
    {
        (int startLevel, int endLevel) = LevelLoader.GetRangeOfLevel(playerDataProfile.PlayerData.LastPlayedLevel);
        UpdateLevelSlots(startLevel);
    }



    public void LoadUp()
    {
        SoundManager.Instance.PlayFx(FxID.Level_Load);
        var startLevel = levelSlots[^1].LevelIndex+1;
        UpdateLevelSlots(startLevel);

    }

    public void LoadDown()
    {
        SoundManager.Instance.PlayFx(FxID.Level_Load);
        var startLevel = levelSlots[0].LevelIndex-5;
        UpdateLevelSlots(startLevel);
    }

    private void UpdateLevelSlots(int startLevel)
    {
        startLevel = Mathf.Max(1, startLevel);

        for (int i = 0; i < 5; i++)
        {
            var levelIndex = startLevel + i;
            levelSlots[i].LevelIndex = levelIndex;
            levelSlots[i].UpdateLevelText();
            levelSlots[i].ToggleSlot(playerDataProfile.PlayerData.MaxCompletedLevel + 1 >= levelIndex);
        }

        loadDownButton.interactable = levelSlots[0].LevelIndex != 1; //disable if first level
        loadUpButton.interactable = levelSlots[^1].LevelIndex <= playerDataProfile.PlayerData.MaxCompletedLevel; //disable if last slot completed
    }
}
