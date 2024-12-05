using UnityEngine;
using UnityEngine.Tilemaps;

public class GridLockManager : MonoBehaviour
{
    public Tilemap tilemap; // Reference to your Tilemap
    public TileBase lockedTile; // The tile used for locked areas
    public TileBase unlockedTile; // The tile used for unlocked areas

    // Example locked grid cells
    public Vector3Int[] lockedCells;

    void Start()
    {
        LockGridCells();
    }

    public void LockGridCells()
    {
        foreach (Vector3Int cell in lockedCells)
        {
            tilemap.SetTile(cell, lockedTile);
        }
    }

    public void UnlockCell(Vector3Int cellPosition)
    {
        tilemap.SetTile(cellPosition, unlockedTile);
    }
}
