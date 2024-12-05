using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }

    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);

    private Queue<TetrominoData> upcomingPieces = new Queue<TetrominoData>();
    private TetrominoData heldPiece;
    private bool hasHeldPiece = false;
    private bool swapUsed = false;

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }

        InitializeUpcomingPieces();
    }

    private void InitializeUpcomingPieces()
    {
        for (int i = 0; i < 5; i++)
        {
            EnqueueNextPiece();
        }
    }

    private void EnqueueNextPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        upcomingPieces.Enqueue(tetrominoes[random]);
    }

    private void Start()
    {
        SpawnPiece();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwapPiece();
        }
    }

    public void SpawnPiece()
    {
        TetrominoData data = upcomingPieces.Dequeue();
        activePiece.Initialize(this, spawnPosition, data);

        EnqueueNextPiece();
        UpdateNextPiecesDisplay();

        if (!IsValidPosition(activePiece, spawnPosition))
        {
            GameOver();
        }
        else
        {
            Set(activePiece);
        }

        swapUsed = false;
    }

    private void UpdateNextPiecesDisplay()
    {
        UIManager.Instance.UpdateNextPieces(upcomingPieces);
    }

    private void SwapPiece()
    {
        if (swapUsed)
        {
            return;
        }

        Clear(activePiece);

        if (!hasHeldPiece)
        {
            heldPiece = activePiece.data;
            hasHeldPiece = true;
            SpawnPiece();
        }
        else
        {
            TetrominoData temp = activePiece.data;
            activePiece.Initialize(this, spawnPosition, heldPiece);
            heldPiece = temp;

            if (!IsValidPosition(activePiece, spawnPosition))
            {
                GameOver();
            }
            else
            {
                Set(activePiece);
            }
        }

        swapUsed = true;
    }

    public void GameOver()
    {
        tilemap.ClearAllTiles();
        ScoreManager.Instance.ResetScore();
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition) || tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;
        int linesCleared = 0;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
                linesCleared++;

                // Check if AudioManager.Instance is available and play the line-clear sound
                if (AudioManager.Instance != null)
                {
                    Debug.Log("AudioManager.Instance is available.");
                    AudioManager.Instance.PlayLineClearSound();
                }
                else
                {
                    Debug.LogError("AudioManager.Instance is null! Make sure AudioManager is present in the scene.");
                }
            }
            else
            {
                row++;
            }
        }

        if (linesCleared > 0)
        {
            ScoreManager.Instance.AddScore(linesCleared);
        }
        else
        {
            ScoreManager.Instance.ResetCombo();
        }
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    public void LineClear(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }
}
