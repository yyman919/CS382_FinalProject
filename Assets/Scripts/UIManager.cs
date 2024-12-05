using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // Tilemaps for displaying each upcoming piece
    public Tilemap[] nextPieceDisplays; // Assign these Tilemaps in the Inspector

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateNextPieces(Queue<TetrominoData> upcomingPieces)
    {
        TetrominoData[] nextPieces = upcomingPieces.ToArray();

        for (int i = 0; i < nextPieceDisplays.Length; i++)
        {
            if (i < nextPieces.Length)
            {
                SetNextPieceDisplay(nextPieceDisplays[i], nextPieces[i]);
            }
        }
    }

    private void SetNextPieceDisplay(Tilemap displayTilemap, TetrominoData tetrominoData)
    {
        displayTilemap.ClearAllTiles();

        foreach (Vector2Int cell in tetrominoData.cells)
        {
            Vector3Int tilePosition = new Vector3Int(cell.x, cell.y, 0);
            displayTilemap.SetTile(tilePosition, tetrominoData.tile);
        }
    }
}
