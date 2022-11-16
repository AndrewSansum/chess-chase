using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    public float attackInterval;

    public Transform playerTransform;

    private EnemyMovement mover;

    public EnemyGrid grid;

    private Transform tf;

    public float moveSpeed;
    private bool moving = false;
    private Queue<Vector2Int> moveQueue = new Queue<Vector2Int>();
    private bool moveAvailable = true;

    void Start() {
        tf = this.gameObject.transform;

        mover = GetComponent<EnemyMovement>();
        mover.Init();
        grid.ReserveGridCell(mover.GetPosition().x, mover.GetPosition().y, this);
        StartCoroutine(MoveToCell(mover.GetPosition()));
    }

    void Update() {
        if (!moving && moveQueue.Count != 0) {
            StartCoroutine(MoveToCell(moveQueue.Dequeue()));
            if (moveQueue.Count == 0) {
                StartCoroutine(MoveCooldown());
            }
        }

        if (moveAvailable && !moving) {
            Vector2Int? nextCell = mover.ChooseNextCell(playerTransform.position);
            if (nextCell.HasValue) {
                Queue<Vector2Int> newQueue = mover.GetPathToCell(nextCell.Value);
                bool free = true;
                foreach (var cell in newQueue) {
                    if (!grid.GetGridValue(cell.x, cell.y).IsEmpty()) {
                        free = false;
                    } 
                }
                
                if (free) {
                    moveQueue = newQueue;
                    foreach (var cell in moveQueue) {
                        grid.ReserveGridCell(cell.x, cell.y, this);
                    }
                    moveAvailable = false;
                } 
            }
              
        }
    }

    private IEnumerator MoveCooldown() {
        yield return new WaitForSeconds(attackInterval);
        moveAvailable = true;
    }

    private IEnumerator MoveToCell(Vector2Int cell) {
        moving = true;
        grid.ReleaseReservation(mover.position.x, mover.position.y, this);
        Vector3 targetPosition = grid.CellToWorld(cell);
        while ((targetPosition - tf.position).magnitude > moveSpeed/100f) {
            tf.position = Vector3.MoveTowards(tf.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        tf.position = targetPosition;
        moving = false;
        mover.position = cell;
    }
}