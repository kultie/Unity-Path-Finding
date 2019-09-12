using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
namespace Kultie.FlowField
{
    public class FFPathFinding
    {
        FFGrid grid;
        Queue<FFCell> searchFrontier = new Queue<FFCell>();

        public FFPathFinding(FFGrid _grid) {
            grid = _grid;
        }

        public void FindPaths(Vector2Int target)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            FFCell[,] cells = grid.GetCells();
            foreach (FFCell _tile in cells)
            {
                _tile.ClearPath();
            }
            cells[target.x,target.y].BecomeDestination();
            searchFrontier.Enqueue(cells[target.x, target.y]);
            while (searchFrontier.Count > 0)
            {
                FFCell tile = searchFrontier.Dequeue();
                if (tile != null)
                {
                    searchFrontier.Enqueue(tile.GrowPathNorth);
                    searchFrontier.Enqueue(tile.GrowPathSouth);
                    searchFrontier.Enqueue(tile.GrowPathEast);
                    searchFrontier.Enqueue(tile.GrowPathWest);
                }
            }
            sw.Stop();
            UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");
        }

        public void UpdatePath(Vector2Int target) {
            FindPaths(target);
        }
    }
}