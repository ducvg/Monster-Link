using UnityEngine;

public class PlayerControl : Singleton<PlayerControl>
{
    public MatchTile selectedTile1, selectedTile2;

    void Start()
    {
        selectedTile1 = null;
        selectedTile2 = null;
    }

    public void SelectTile(MatchTile tile)
    {
        if (selectedTile1 == null)
        {
            selectedTile1 = tile;
        }
        else if (selectedTile2 == null && selectedTile1 != tile)
        {
            selectedTile2 = tile;
            // Add logic to handle the selection of two tiles
        }
        else
        {
            // Reset selection if the same tile is clicked or both tiles are already selected
            selectedTile1 = tile;
            selectedTile2 = null;
        }
    }
}
