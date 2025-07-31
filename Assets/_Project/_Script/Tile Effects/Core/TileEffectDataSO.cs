using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileEffect : ScriptableObject
{
    public ParticleSystem emitParticlePrefab;

    private Dictionary<MatchTile, ParticleSystem> tileParticles = new();

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


