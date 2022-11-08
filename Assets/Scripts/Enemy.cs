using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    public float AttackInterval;

    public Transform playerTransform;

    private EnemyMovement mover;

    public EnemyGrid grid;
    
    void Start() {
        mover = GetComponent<EnemyMovement>();
        mover.Init();
        grid.ReserveGridCell(mover.GetPosition().x, mover.GetPosition().y, this);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(ChooseNextCell(mover.GetTraversableCells()));
        }
    }

    public Vector2Int ChooseNextCell(List<Vector2Int> traversableCells) {
        Vector2Int relativePlayerPosition = grid.WorldToCell(playerTransform.position) - mover.GetPosition();

        Vector2Int bestCell = traversableCells[0];
        float bestAngle = Vector2.Angle(relativePlayerPosition, bestCell);

        foreach (var cell in traversableCells) {
            Vector2Int relativeCellPosition = cell - mover.GetPosition();
            float angle = Vector2.Angle(relativePlayerPosition, relativeCellPosition);
            if (angle < bestAngle) {
                bestCell = cell;
                bestAngle = angle;
            }
        }

        return bestCell;
    }
}
