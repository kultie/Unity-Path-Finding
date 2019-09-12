using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kultie.FlowField;
public class FFController : MonoBehaviour {
    FFGrid grid;
    SpriteRenderer[,] gameObjects;
    FFPathFinding pathFinding;

    public int gridWidth;
    public int gridHeight;

    public SpriteRenderer nodePrefabs;
    public Transform nodeContainer;
    Vector2Int target;
    // Use this for initialization
    void Start () {
        grid = new FFGrid(gridWidth, gridHeight);
        gameObjects = new SpriteRenderer[gridWidth, gridHeight];
        foreach (var node in grid.GetCells())
        {
            SpriteRenderer go = Instantiate(nodePrefabs, nodeContainer);
            if (!node.walkable)
            {
                go.color = Color.black;
            }
            //go.color = new Color(node.movementPenalty,node.movementPenalty,node.movementPenalty,1);
            go.transform.localPosition = ConvertPostiion(node.position);
            gameObjects[(int)node.position.x, (int)node.position.y] = go;
        }
        pathFinding = new FFPathFinding(grid);
        target = new Vector2Int(gridWidth/2, gridHeight/2);
        pathFinding.FindPaths(target);
        UpdateArrow();
    }

    Vector2Int MouseToNode(Vector2 worldPos)
    {
        float x = worldPos.x + gridWidth / 2;
        float y = worldPos.y + gridHeight / 2;

        x = Mathf.RoundToInt(worldPos.x + gridWidth / 2);
        y = Mathf.RoundToInt(worldPos.y + gridHeight / 2);

        x = Mathf.Max(0, x);
        y = Mathf.Min(gridHeight - 1, y);
        x = Mathf.Min(gridWidth - 1, x);
        y = Mathf.Max(0, y);

        return new Vector2Int((int)x, (int)y);
    }

    Vector2 ConvertPostiion(Vector2 original)
    {
        Vector2 newPos = new Vector2(original.x - gridWidth / 2, original.y - gridHeight / 2);
        return newPos;
    }

    // Update is called once per frame
    void UpdateArrow () {
        gameObjects[(int)target.x, (int)target.y].gameObject.SetActive(false);
        foreach (var node in grid.GetCells())
        {
            float angle = Mathf.Atan2(node.direction.x, node.direction.y) * Mathf.Rad2Deg;
            gameObjects[(int)node.position.x, (int)node.position.y].transform.localRotation = node.GetDirection();
            //Debug.Log("Position: " + node.position.x + node.position.y + " Direction: " + node.direction + ": " + node.distance);
        }
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0)) {
        //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2Int currentTilePos = MouseToNode(mousePos);
        //    grid.GetCells()[currentTilePos.x, currentTilePos.y].walkable = false;
        //    gameObjects[currentTilePos.x, currentTilePos.y].color = Color.black;
        //    pathFinding.UpdatePath(target);
        //    UpdateArrow();
        //}

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int currentTilePos = MouseToNode(mousePos);
            FFCell cell = grid.GetCells()[currentTilePos.x, currentTilePos.y];
            if (cell.walkable)
            {
                FFCell currentCell = cell;
                while (currentCell.HasPath)
                {
                    gameObjects[currentCell.position.x, currentCell.position.y].color = Color.green;
                    //Gizmos.DrawCube(gameObjects[currentCell.position.x, currentCell.position.y].transform.position, Vector3.one);
                    currentCell = currentCell.nextOnPath;
                    if (currentCell == null) {
                        return;
                    }
                }
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Vector2Int currentTilePos = MouseToNode(mousePos);
    //        FFCell cell = grid.GetCells()[currentTilePos.x, currentTilePos.y];
    //        if (cell.walkable) {
    //            FFCell currentCell = cell;
    //            while (currentCell.HasPath) {
    //                Gizmos.DrawCube(gameObjects[currentCell.position.x, currentCell.position.y].transform.position, Vector3.one);
    //                currentCell = cell.nextOnPath;
    //            }
    //        }
    //    }
    //}
}
