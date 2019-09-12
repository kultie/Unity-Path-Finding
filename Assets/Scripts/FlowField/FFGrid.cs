using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kultie.FlowField
{
    public class FFGrid
    {
        int width, height;

        FFCell[,] cells;

        public FFCell[,] GetCells()
        {
            return cells;
        }

        public FFGrid(int _width, int _height)
        {
            width = _width;
            height = _height;
            Generate();
        }

        public void Generate()
        {
            cells = new FFCell[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    FFCell cell = new FFCell(new Vector2Int(i, j));
                    cells[i, j] = cell;
                    if (Perlinoise(i, j, 64) > 0.75)
                    {
                        cells[i, j].walkable = false;
                    }
                    if (i > 0)
                    {
                        FFCell.MakeEastWestNeighbors(cell, cells[i - 1,j]);
                    }
                    if (j > 0)
                    {
                        FFCell.MakeNorthSouthNeighbors(cell, cells[i,j - 1]);
                    }

                    cell.IsAlternative = (i & 1) == 0;
                }
            }
        }

        float Perlinoise(int x, int y, float size)
        {
            float xCoord = x * size * 0.1f;
            float yCoord = y * size * 0.1f;
            float value = Mathf.PerlinNoise(xCoord, yCoord);
            return value;
        }


    }
}
