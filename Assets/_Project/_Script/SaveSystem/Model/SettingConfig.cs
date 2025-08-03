using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class SettingConfig
{
    [SerializeField] private float backgroundVolume = 1f;
    [SerializeField] private float fxVolume = 1f;

    [JsonIgnore]
    public float BackgroundVolume { get => backgroundVolume; set => backgroundVolume = value; }
    [JsonIgnore]
    public float FxVolume { get => fxVolume; set => fxVolume = value; }
}