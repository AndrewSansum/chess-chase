using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookMovement : EnemyMovement
{
    private List<Vector2Int> GetTraversableCells() {
        List<Vector2Int> cells = new List<Vector2Int>();

        int leftMost = -1;
        for (int x = position.x; x >= 0; x--) {
            if (!grid.GetGridValue(x, position.y).IsEmpty()) {
                continue;
            }

            leftMost = x;
        }
        if (leftMost > -1) {
            cells.Add(new Vector2Int(leftMost, position.y));
        }

        int rightMost = -1;
        for (int x = position.x; x < grid.XSize; x++) {
            if (!grid.GetGridValue(x, position.y).IsEmpty()) {
                continue;
            }

            rightMost = x;
        }
        if (rightMost > -1) {
            cells.Add(new Vector2Int(rightMost, position.y));
        }

        int upMost = -1;
        for (int y = position.y; y < grid.YSize; y++) {
            if (!grid.GetGridValue(position.x, y).IsEmpty()) {
                continue;
            }

            upMost = y;
        }
        if (upMost > -1) {
            cells.Add(new Vector2Int(position.x, upMost));
        }

        int downMost = -1;
        for (int y = position.y; y >= 0; y--) {
            if (!grid.GetGridValue(position.x, y).IsEmpty()) {
                continue;
            }

            downMost = y;
        }
        if (downMost > -1) {
            cells.Add(new Vector2Int(position.x, downMost));
        }

        return cells;
    }

    public override Queue<Vector2Int> GetPathToCell(Vector2Int cellPosition) {
        Queue<Vector2Int> cells = new Queue<Vector2Int>();
        if ((cellPosition - position).x > 0) {
            for (int x = position.x + 1; x <= cellPosition.x; x++) {
                cells.Enqueue(new Vector2Int(x, position.y));
            }
        } else if ((cellPosition - position).x < 0) {
            for (int x = position.x - 1; x >= cellPosition.x; x--) {
                cells.Enqueue(new Vector2Int(x, position.y));
            }
        } else if ((cellPosition - position).y > 0) {
            for (int y = position.y + 1; y <= cellPosition.y; y++) {
                cells.Enqueue(new Vector2Int(position.x, y));
            }
        } else if ((cellPosition - position).y < 0) {
            for (int y = position.y - 1; y >= cellPosition.y; y--) {
                cells.Enqueue(new Vector2Int(position.x, y));
            }
        }

        return cells;
    }

    public override Vector2Int? ChooseNextCell(Vector3 playerPosition) {
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
}
