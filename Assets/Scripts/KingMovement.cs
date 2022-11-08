using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovement : EnemyMovement
{
    protected override void Start() {
        base.Start();
        GetTraversableCells();
    }
    public override List<Vector2Int> GetTraversableCells()
    {
        int minX = (position.x > 0) ? position.x - 1 : 0;
        int minY = (position.y > 0) ? position.y - 1 : 0;

        int maxX = (position.x < grid.XSize - 1) ? position.x + 1 : grid.XSize - 1;
        int maxY = (position.y < grid.YSize - 1) ? position.y + 1 : grid.YSize - 1;

        List<Vector2Int> cells = new List<Vector2Int>();
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (grid.GetGridValue(x, y).IsEmpty()) {
                    cells.Add(new Vector2Int(x, y));
                }
            }
        }

        return cells;
    }

    public override Queue<Vector2Int> GetPathToCell(Vector2Int cellPosition)
    {
        Queue<Vector2Int> path = new Queue<Vector2Int>();
        path.Enqueue(cellPosition);
        return path;
    }
}
