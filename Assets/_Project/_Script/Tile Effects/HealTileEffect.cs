using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Scriptable Objects/Tile Effect/Heal")]
public class HealEffect : TileEffect
{
    [Header("Heal Tile Properties")]
    public int healAmount;

    public override void ApplyEffect()
    {
        // Logic to heal a character
        Debug.Log($"Healing for {healAmount} points.");
    }

    public override void RemoveEffect()
    {
        // Logic to remove healing effect
        Debug.Log("Healing effect removed.");
    }
}




