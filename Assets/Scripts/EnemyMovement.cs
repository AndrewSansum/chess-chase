using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    protected Vector2Int position;
    public EnemyGrid grid;

    public virtual void Init() {
        position = grid.WorldToCell(this.gameObject.transform.position);
    }

    public abstract List<Vector2Int> GetTraversableCells();

    public abstract Queue<Vector2Int> GetPathToCell(Vector2Int cellPosition);

    public Vector2Int GetPosition() {
        return position;
    }
}
