using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class EnemyGrid : MonoBehaviour
{
    private Tilemap tilemap;

    private int xOffset;
    private int yOffset;
    public int XSize {get; private set;}
    public int YSize {get; private set;}

    private GridValue[,] grid;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        BoundsInt tileBounds = tilemap.cellBounds;
        xOffset = tileBounds.xMin;
        yOffset = tileBounds.yMin;
        XSize = tileBounds.xMax - tileBounds.xMin;
        YSize = tileBounds.yMax - tileBounds.yMin;
        grid = new GridValue[XSize,YSize];
        for (int x = 0; x < XSize; x++)
        {
            for (int y = 0; y < YSize; y++) 
            {
                grid[x, y] = new GridValue();
                if (!tilemap.HasTile(new Vector3Int(x + xOffset, y + yOffset, 0))) {
                    grid[x, y].SetObstacle();
                }
            }
        }        
    }

    public GridValue GetGridValue(int x, int y) {
        return grid[x,y];
    }

    public bool ReserveGridCell(int x, int y, Object reserver) {
        GridValue cell = grid[x, y];
        if (cell.IsEmpty()) {
            cell.SetReserved(reserver.GetInstanceID());
            return true;
        } else {
            return false;
        }           
    }

    public void ReleaseReservation(int x, int y, Object reserver) {
        GridValue cell = grid[x, y];
        if (cell.IsReserved() && cell.GetValue() == reserver.GetInstanceID()) {
            cell.SetEmpty();
        }
    }

    public Vector2Int WorldToCell(Vector3 worldPosition) {
        Vector3Int tilemapPosition = tilemap.WorldToCell(worldPosition);
        Debug.Log(worldPosition);
        Debug.Log(tilemapPosition);
        return new Vector2Int(tilemapPosition.x - xOffset, tilemapPosition.y - yOffset);
    }
}

public class GridValue
{
    private enum GridStatus {
        Empty,
        Obstacle,
        Reserved
    }

    private GridStatus status;
    private int value = 0;

    public GridValue() {
        status = GridStatus.Empty;
    }

    public override string ToString() {
        return status.ToString();
    }

    public void SetEmpty() {
        status = GridStatus.Empty;
    }

    public bool IsEmpty() {
        return status == GridStatus.Empty;
    }

    public void SetObstacle() {
        status = GridStatus.Obstacle;
    }

    public bool IsObstacle() {
        return status == GridStatus.Obstacle;
    }

    public void SetReserved(int value) {
        status = GridStatus.Reserved;
        this.value = value;
    }

    public bool IsReserved() {
        return status == GridStatus.Reserved;
    }

    public int GetValue() {
        return value;
    }
}
