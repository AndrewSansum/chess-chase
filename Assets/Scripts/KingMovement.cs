using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovement : EnemyMovement
{
    private List<Vector2Int> GetTraversableCells()
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

    public override Vector2Int? GetNextMovementCell(Vector3 playerPosition) {
        Vector2Int relativePlayerPosition = grid.WorldToCell(playerPosition) - position;

        List<Vector2Int> traversableCells = GetTraversableCells();

        if (traversableCells.Count > 0) {
            Vector2Int bestCell = traversableCells[0];
            float bestAngle = Vector2.Angle(relativePlayerPosition, bestCell);

            foreach (var cell in traversableCells) {
                Vector2Int relativeCellPosition = cell - position;
                float angle = Vector2.Angle(relativePlayerPosition, relativeCellPosition);
                if (angle > bestAngle) {
                    bestCell = cell;
                    bestAngle = angle;
                }
            }

            return bestCell;
        } else {
            return null;
        }  
    }

    public override Vector2Int? GetNextAttackCell(Vector3 playerPosition) {
        Vector2Int relativePlayerPosition = grid.WorldToCell(playerPosition) - position;

        List<Vector2Int> traversableCells = GetTraversableCells();

        if (traversableCells.Count > 0) {
            Vector2Int bestCell = traversableCells[0];
            float bestAngle = Vector2.Angle(relativePlayerPosition, bestCell);

            foreach (var cell in traversableCells) {
                Vector2Int relativeCellPosition = cell - position;
                float angle = Vector2.Angle(relativePlayerPosition, relativeCellPosition);
                if (angle < bestAngle) {
                    bestCell = cell;
                    bestAngle = angle;
                }
            }

            return bestCell;
        } else {
            return null;
        }  
    }

    public override bool HasAttackOppurtunity(Vector3 playerPosition)
    {
        var playerCell = grid.WorldToCell(playerPosition);
        return playerCell.x - 1 <= position.x && position.x <= playerCell.x + 1 && playerCell.y - 1 <= position.y && position.y <= playerCell.y + 1;
    }
}
