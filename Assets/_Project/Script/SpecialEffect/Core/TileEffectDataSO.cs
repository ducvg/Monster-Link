using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileEffectData : ScriptableObject
{
    public string label;
    public int minCount, maxCount;
    public ParticleSystem emitParticlePrefab;
    // [SerializeReference] public List<SpecialEffect> effects;

    private Dictionary<MatchTile, ParticleSystem> tileParticles = new();

    void OnEnable()
    {
        if (string.IsNullOrEmpty(label)) label = name; //the asset name in editor
        // if (effects == null) effects = new List<SpecialEffect>();
    }

    public abstract void ApplyEffect();
    public abstract void RemoveEffect();

    public virtual void OnInit(MatchTile tile)
    {
        var particle = Instantiate(emitParticlePrefab, tile.transform);
        tileParticles[tile] = particle;
    }

    public virtual void OnDespawn(MatchTile tile)
    {
        if(tileParticles.TryGetValue(tile, out var particle))
        {
            Destroy(particle.gameObject);
        }
    }
}


