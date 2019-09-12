using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kultie.FlowField
{
    public class FFCell
    {
        static Quaternion
            northRotation = Quaternion.Euler(0f, 0f, 0f),
            eastRotation = Quaternion.Euler(0f, 0f, 270f),
            southRotation = Quaternion.Euler(0f, 0f, 180f),
            westRotation = Quaternion.Euler(0f, 0f, 90f);

        public Vector2Int position;

        public Quaternion direction;

        public bool walkable = true;

        public bool IsAlternative { get; set; }

        FFCell north, east, south, west;

        public FFCell nextOnPath;
        int distance;

        public bool HasPath
        {
            get
            {
                return distance != int.MaxValue;
            }
        }

        public FFCell GrowPathNorth
        {
            get
            {
                return GrowPathTo(north);
            }
        }

        public FFCell GrowPathEast
        {
            get
            {
                return GrowPathTo(east);
            }
        }


        public FFCell GrowPathSouth
        {
            get
            {
                return GrowPathTo(south);
            }
        }

        public FFCell GrowPathWest
        {
            get
            {
                return GrowPathTo(west);
            }
        }

        private FFCell()
        {
        }

        public FFCell(Vector2Int _position)
        {
            position = _position;
        }

        public static void MakeEastWestNeighbors(FFCell east, FFCell west)
        {
            Debug.Assert(
            west.east == null && east.west == null, "Redefined neighbors!"
            );
            west.east = east;
            east.west = west;
        }

        public static void MakeNorthSouthNeighbors(FFCell north, FFCell south)
        {
            Debug.Assert(
                south.north == null && north.south == null, "Redefined neighbors!"
            );
            south.north = north;
            north.south = south;
        }

        public void ClearPath()
        {
            distance = int.MaxValue;
            nextOnPath = null;
        }

        public void BecomeDestination()
        {
            distance = 0;
            nextOnPath = null;
        }

        FFCell GrowPathTo(FFCell neighbor)
        {
            Debug.Assert(HasPath, "No path!");
            if (neighbor == null || neighbor.HasPath)
            {
                return null;
            }
            neighbor.distance = distance + 1;
            neighbor.nextOnPath = this;
            return neighbor.walkable? neighbor:null;
        }

        public Quaternion GetDirection()
        {
            if (distance == 0)
            {
                
                return Quaternion.Euler(0,0,0);
            }
            //arrow.gameObject.SetActive(true);
           direction =
                nextOnPath == north ? northRotation :
                nextOnPath == east ? eastRotation :
                nextOnPath == south ? southRotation :
                westRotation;
            return direction;
        }
    }
}

