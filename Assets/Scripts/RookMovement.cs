using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookMovement : EnemyMovement
{
    private List<Vector2Int> GetTraversableCells() {
        List<Vector2Int> cells = new List<Vector2Int>();

        for (int x = position.x - 1; x >= 0; x--) {
            if (grid.GetGridValue(x, position.y).IsEmpty()) {
                Debug.Log(new Vector2Int(x, position.y));
                cells.Add(new Vector2Int(x, position.y));
            } else {
                break;
            }
        }

        for (int x = position.x + 1; x < grid.XSize; x++) {
            if (grid.GetGridValue(x, position.y).IsEmpty()) {
                Debug.Log(new Vector2Int(x, position.y));
                cells.Add(new Vector2Int(x, position.y));
            } else {
                break;
            }
        }

        for (int y = position.y + 1; y < grid.YSize; y++) {
            if (grid.GetGridValue(position.x, y).IsEmpty()) {
                Debug.Log(new Vector2Int(position.x, y));
                cells.Add(new Vector2Int(position.x, y));
            } else {
                break;
            }
        }

        for (int y = position.y - 1; y >= 0; y--) {
            if (grid.GetGridValue(position.x, y).IsEmpty()) {
                Debug.Log(new Vector2Int(position.x, y));
                cells.Add(new Vector2Int(position.x, y));
            } else {
                break;
            }
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
        Vector2Int playerCell = grid.WorldToCell(playerPosition);
        Debug.Log("Player cell: " + playerCell.ToString());

        Vector2Int relativePlayerPosition = playerCell - position;

        List<Vector2Int> traversableCells = GetTraversableCells();

        if (traversableCells.Count <= 0) {
            Debug.Log("None traversable");
            return null;
        }

        if (playerCell.x == position.x && traversableCells.Contains(playerCell)) {
            if (playerCell.y >= position.y) {
                return new Vector2Int(position.x, Mathf.Min(playerCell.y + 2, grid.YSize - 1));
            } else {
                return new Vector2Int(position.x, Mathf.Max(playerCell.y - 2, 0));
            }
        } else if (playerCell.y == position.y && traversableCells.Contains(playerCell)) {
            if (playerCell.x >= position.x) {
                return new Vector2Int(Mathf.Min(playerCell.x + 2, grid.XSize - 1), position.y);
            } else {
                return new Vector2Int(Mathf.Max(playerCell.x - 2, 0), position.y);
            }
        } else {
            Debug.Log("Not in line");
            List<Vector2Int> optimalCells = traversableCells.FindAll(cell => cell.y == playerCell.y || cell.x == playerCell.x);
            if (optimalCells.Count > 0) {
                Debug.Log("Found in line");
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
                Debug.Log("Random");
                return traversableCells[Mathf.RoundToInt(Random.Range(0, traversableCells.Count - 1))];
            }
            
        }
    }
}
