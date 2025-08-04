using UnityEngine;

public class HomeCanvas : BaseCanvas
{
    public void OpenSetting()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        UIManager.Instance.Open<HomeSettingCanvas>();
    }   

    public void ToggleInventory()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        if (UIManager.Instance.IsOpened<InventoryCanvas>())
        {
            UIManager.Instance.Close<InventoryCanvas>();
        }
        else
        {
            UIManager.Instance.Open<InventoryCanvas>();
        }
    }

    public void OpenShop()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
    }

}
