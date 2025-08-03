using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button button;
    [SerializeField] private GameObject locker;

    [field: SerializeField] public int LevelIndex { get; set; } = 0;
    public Button Button { get => button; }


    public void UpdateLevelText()
    {
        levelText.text = LevelIndex.ToString();
    }

    public void ToggleSlot(bool isEnable)
    {
        button.interactable = isEnable;
        locker.SetActive(!isEnable);
    }

    public void StartLevel()
    {
        SoundManager.Instance.PlayFx(FxID.Level_Select);
        GameManager.Instance.LoadSceneQuick(GameScene.Gameplay, () =>
        {
            LevelManager.Instance.SpawnLevel(LevelIndex);
            SoundManager.Instance.ChangeSound(SoundID.Gameplay_BG, 1);
        });
    }
}
