using System.Collections.Generic;
using UnityEngine;
using Utility.SkibidiTween;

public class TileSelector : Singleton<TileSelector>
{
    [SerializeField] private GameObject selectHighlighter;
    [SerializeField] private LineDrawerSO lineDrawer;

    private MatchTile selectedTile1, selectedTile2;

    public void SelectTile(MatchTile tile)
    {
        SoundManager.Instance.PlayFx(FxID.Tile_Select);
        HighlightTileOn(tile);

        if (selectedTile1 == null) // select 1st tile
        {
            selectedTile1 = tile;
        }
        else if (selectedTile1 == tile) //  select same tile
        {
            HighlightTileOff(selectedTile1);
            selectedTile1 = null;
        }
        else if (selectedTile1.MatchTileData == tile.MatchTileData) // select 2nd tile of same type
        {
            selectedTile2 = tile;

            // Connect the tiles
            List<(int x, int y)> path = GameBoard.Connect(selectedTile1, selectedTile2);
            if (path != null)
            {
                lineDrawer.DrawLine(path, 1.4f);

                OnTilesConnected();
            }
            else
            {
                HighlightTileOff(selectedTile1);
                HighlightTileOff(selectedTile2);
            }

            selectedTile1 = null;
            selectedTile2 = null;
        }
        else // select 2nd tile of different type
        {
            HighlightTileOff(selectedTile1);
            selectedTile1 = tile;
            HighlightTileOn(selectedTile1);
        }
    }

    private void HighlightTileOn(MatchTile tile)
    {
        selectHighlighter.SetActive(true);
        selectHighlighter.transform.position = tile.transform.position;
        tile.OnSelect();
    }

    private void HighlightTileOff(MatchTile tile)
    {
        selectHighlighter.SetActive(false);
        tile.OnDeselect();
    }

    private void OnTilesConnected()
    {
        SoundManager.Instance.PlayFx(FxID.Tile_Connect);
        selectHighlighter.SetActive(false);
        
        selectedTile1.OnConnect();
        selectedTile2.OnConnect();

        BoardManager.Instance.TryApplyGravityAt(selectedTile1, selectedTile2);

        BoardManager.Instance.MatchTileCount -= 2;
        if(BoardManager.Instance.MatchTileCount <= 0)
        {
            GameState.OnGameWon?.Invoke();
            StartCoroutine(SkibidiTween.SkibidiDelay(1f, () =>
            {
                UIManager.Instance.Open<GameplayWinCanvas>();
                
            }));
            return;
        }

        if(GameBoard.FindAnyPath() == null)
        {
            Debug.Log("No path found, shuffling");
            GameBoard.Shuffle();
        }
    }

    public void Reset()
    {
        if(selectedTile1 != null)
        {
            selectedTile1.OnDeselect();
            selectedTile1 = null;
        }
        if(selectedTile2 != null)
        {
            selectedTile2.OnDeselect();
            selectedTile2 = null;
        }
        selectHighlighter.SetActive(false);
    }
}
