using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopMovement : EnemyMovement
{
    private List<Vector2Int> GetTraversableCells() {
        List<Vector2Int> cells = new List<Vector2Int>();

        for (int offset = 1; position.x + offset < grid.XSize && position.y + offset < grid.YSize; offset++) {
            if (grid.GetGridValue(position.x + offset, position.y + offset).IsEmpty()) {
                cells.Add(new Vector2Int(position.x + offset, position.y + offset));
            } else {
                break;
            }
        }

        for (int offset = 1; position.x + offset < grid.XSize && position.y - offset >= 0; offset++) {
            if (grid.GetGridValue(position.x + offset, position.y - offset).IsEmpty()) {
                cells.Add(new Vector2Int(position.x + offset, position.y - offset));
            } else {
                break;
            }
        }

        for (int offset = 1; position.x - offset >= 0 && position.y - offset >= 0; offset++) {
            if (grid.GetGridValue(position.x - offset, position.y - offset).IsEmpty()) {
                cells.Add(new Vector2Int(position.x - offset, position.y - offset));
            } else {
                break;
            }
        }

        for (int offset = 1; position.x - offset >= 0 && position.y + offset < grid.YSize; offset++) {
            if (grid.GetGridValue(position.x - offset, position.y + offset).IsEmpty()) {
                cells.Add(new Vector2Int(position.x - offset, position.y + offset));
            } else {
                break;
            }
        }

        return cells;
    }

    public override Queue<Vector2Int> GetPathToCell(Vector2Int cellPosition) {
        Queue<Vector2Int> cellQueue = new Queue<Vector2Int>();

        int xDirMod = Mathf.RoundToInt(Mathf.Sign(cellPosition.x - position.x));
        int yDirMod = Mathf.RoundToInt(Mathf.Sign(cellPosition.y - position.y));

        for (int offset = 1; 
            xDirMod * (position.x + (xDirMod * offset)) <= cellPosition.x * xDirMod
            && yDirMod * (position.y + (yDirMod * offset)) <= cellPosition.y * yDirMod; offset ++) {

            cellQueue.Enqueue(new Vector2Int(
                position.x + xDirMod * offset,
                position.y + yDirMod * offset
            ));
        }

        return cellQueue;
    }

    public override bool HasAttackOppurtunity(Vector3 playerPosition) {
        Vector2Int playerCell = grid.WorldToCell(playerPosition);

        List<Vector2Int> traversableCells = GetTraversableCells();

        return traversableCells.Exists(cell => playerCell.x - 1 <= cell.x && cell.x <= playerCell.x + 1 && playerCell.y - 1 <= cell.y && cell.y <= playerCell.y + 1);
    }

    public override Vector2Int? GetNextMovementCell(Vector3 playerPosition) {
        Vector2Int playerCell = grid.WorldToCell(playerPosition);

        List<Vector2Int> traversableCells = GetTraversableCells();

        if (traversableCells.Count <= 0) {
            return null;
        }

        List<Vector2Int> optimalCells = traversableCells.FindAll(
            cell => 
            Vector2.Angle(cell + Vector2Int.up, playerCell) == 45
            || Vector2.Angle(cell + Vector2Int.up, playerCell) == 135
        );

        if (optimalCells.Count > 0) {
            // Get cell with minimum travel distance 
            // 
            // i.e. 
            // x ∈ optimalCells : ∀ c ∈ optimalCells . |x - playerPosition| <= |c - playerPosition|
            // where optimalCells ⊆ ℕ²
            return optimalCells.Find(
                cell => optimalCells.TrueForAll(
                    innerCell => Vector2.Distance(cell, position) <= Vector2.Distance(innerCell, position)
                    )
                ); 
        } else {
            return traversableCells.Find(
                cell => traversableCells.TrueForAll(
                    innerCell => Vector2.Distance(cell, playerCell) <= Vector2.Distance(innerCell, playerCell)
                    )
                );
        }
    }

    public override Vector2Int? GetNextAttackCell(Vector3 playerPosition)
    {
        Vector2Int playerCell = grid.WorldToCell(playerPosition);

        List<Vector2Int> traversableCells = GetTraversableCells();

        if (traversableCells.Count <= 0) {
            return null;
        }

        List<Vector2Int> validCells = new List<Vector2Int>();

        int xDirMod = Mathf.RoundToInt(Mathf.Sign(playerCell.x - position.x));
        int yDirMod = Mathf.RoundToInt(Mathf.Sign(playerCell.y - position.y));

        if (traversableCells.Contains(playerCell)) {
            validCells.Add(playerCell);
        }
        //have gotten:
        // - - - -
        // - - - -
        // - p - -
        // - - - -
        // where p is player

        validCells.AddRange(traversableCells.FindAll(
            cell =>
                (cell.x == playerCell.x + xDirMod && playerCell.y - 1 <= cell.y && cell.y <= playerCell.y + 1)
            ||
                (cell.y == playerCell.y + yDirMod && playerCell.x - 1 <= cell.x && cell.x <= playerCell.x + 1)
        ));
        //have gotten:
        // - - - -
        // x x x -
        // - p x -
        // - - x -
        
        validCells.AddRange(traversableCells.FindAll(
            cell =>
                validCells.Contains(new Vector2Int(cell.x - xDirMod, cell.y - yDirMod))
        ));
        //final pattern:
        // - x x x
        // x x x x
        // - p x x
        // - - x -

        return validCells.Find(
                cell => validCells.TrueForAll(
                    innerCell => Vector2.Distance(cell, position) >= Vector2.Distance(innerCell, position)
                )
            );
    }
}
