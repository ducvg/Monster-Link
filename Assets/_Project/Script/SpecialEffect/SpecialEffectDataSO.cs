using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialEffectData : ScriptableObject
{
    public string label;
    public Color tileColor = Color.white;
    // [SerializeReference] public List<SpecialEffect> effects;

    void OnEnable()
    {
        if (string.IsNullOrEmpty(label)) label = name; //the asset name in editor
        // if (effects == null) effects = new List<SpecialEffect>();
    }

    public abstract void ApplyEffect();
    public abstract void RemoveEffect();
}


