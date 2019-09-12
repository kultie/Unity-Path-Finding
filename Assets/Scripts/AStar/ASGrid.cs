using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kultie.AStar
{
    public class ASGrid
    {
        ASNode[,] nodes;

        int width, height;


        public ASGrid(int _width, int _height)
        {
            width = _width;
            height = _height;
            nodes = new ASNode[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    nodes[i, j] = new ASNode(true, new Vector2Int(i, j));
                    if (Perlinoise(i, j, 64) > 0.75) {
                        nodes[i, j].walkable = false;
                    }
                    //nodes[i, j].movementPenalty = Perlinoise(i, j,1);
                }
            }
        }

        public List<ASNode> GetNeighBors(ASNode node)
        {
            List<ASNode> neighbors = new List<ASNode>();
            for (int x = -1; x <= 1; x++)
            {

                if (x == 0)
                {
                    continue;
                }
                int neighborsX = node.position.x + x;
                int neighborsY = node.position.y;
                if (neighborsX >= 0 && neighborsX <= width - 1)
                {
                    neighbors.Add(nodes[neighborsX, neighborsY]);
                }

            }

            for (int y = -1; y <= 1; y++)
            {
                if (y == 0)
                {
                    continue;
                }
                int neighborsX = node.position.x;
                int neighborsY = node.position.y + y;
                if (neighborsY >= 0 && neighborsY <= height - 1)
                {
                    neighbors.Add(nodes[neighborsX, neighborsY]);
                }
            }

            return neighbors;
        }

        public ASNode[,] GetNodes()
        {
            return nodes;
        }

        float Perlinoise(int x, int y, float size)
        {
            float xCoord = x * size * 0.1f;
            float yCoord = y * size * 0.1f;
            float value = Mathf.PerlinNoise(xCoord, yCoord);
            return value;
        }

        public int GridArea {
            get{
                return width * height;
            }
        }
    }
}
