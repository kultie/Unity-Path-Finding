using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using Kultie.AStar;

public class Controller : MonoBehaviour
{
    ASGrid grid;
    SpriteRenderer[,] gameObjects;
    ASPathFinding pathFinding;

    public int gridWidth;
    public int gridHeight;

    public SpriteRenderer nodePrefabs;
    public Transform nodeContainer;

    Vector2Int startPosition = new Vector2Int(-99, -99);
    Vector2Int endPosition = new Vector2Int(-99, -99);

    List<Vector2Int> startPoints = new List<Vector2Int>();

    // Use this for initialization
    void Start()
    {
        grid = new ASGrid(gridWidth, gridHeight);
        gameObjects = new SpriteRenderer[gridWidth, gridHeight];
        foreach (var node in grid.GetNodes())
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

        pathFinding = new ASPathFinding(grid);
    }

    Vector2 ConvertPostiion(Vector2 original)
    {
        Vector2 newPos = new Vector2(original.x - gridWidth / 2, original.y - gridHeight / 2);
        return newPos;
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

    bool startFindPath;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int currentTilePos = MouseToNode(mousePos);
            if (grid.GetNodes()[currentTilePos.x, currentTilePos.y].walkable)
            {
                if (startPosition == Vector2.one * -99)
                {
                }

                gameObjects[currentTilePos.x, currentTilePos.y].color = Color.red;
                startPosition = currentTilePos;
                startPoints.Add(startPosition);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            startFindPath = true;
        }

        if (startFindPath)
        {
            // Stopwatch sw = new Stopwatch();
            // sw.Start();
            // Reset();
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int currentTilePos = MouseToNode(mousePos);
            if (currentTilePos == startPosition) return;
            if (currentTilePos == endPosition) return;
            endPosition = currentTilePos;

            foreach (var node in grid.GetNodes())
            {
                if(startPoints.Contains(node.position)) continue;
                gameObjects[node.position.x, node.position.y].color = node.walkable ? Color.white : Color.black;
            }

            if (grid.GetNodes()[currentTilePos.x, currentTilePos.y].walkable)
            {
                pathFinding.Reset();
                pathFinding.RequestFindPath(startPoints.ToArray(), currentTilePos);
                foreach (var path in pathFinding.path)
                {
                    foreach (var node in path)
                    {
                        if (startPoints.Contains(node.position)) continue;
                        gameObjects[node.position.x, node.position.y].color = Color.green;
                    }
                }
            }
            // sw.Stop();
            // UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");
        }
    }

    // private void OnDrawGizmos()
    // {
    //     if (pathFinding != null)
    //     {
    //         foreach (var path in pathFinding.path)
    //         {
    //             foreach (var node in path)
    //             {
    //                 Gizmos.DrawCube(gameObjects[node.position.x, node.position.y].transform.position, Vector3.one);
    //             }
    //         }
    //     }
    // }

    void Reset()
    {
        //gameObjects[startPosition.x, startPosition.y].color = Color.white;

        //startPosition = new Vector2Int(-99, -99);
        //endPosition = new Vector2Int(-99, -99);

        pathFinding.Reset();
    }

    void ChangePath()
    {
        foreach (var node in grid.GetNodes())
        {
            SpriteRenderer go = Instantiate(nodePrefabs, nodeContainer);
            if (!node.walkable)
            {
                go.color = Color.black;
            }

            go.transform.localPosition = ConvertPostiion(node.position);
            gameObjects[(int)node.position.x, (int)node.position.y] = go;
        }

        pathFinding.Reset();
        gameObjects[startPosition.x, startPosition.y].color = Color.red;
    }
}