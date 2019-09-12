using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kultie.AStar
{
    public class ASNode: IHeapItem<ASNode>
    {
        public bool walkable;
        public Vector2Int position;

        public int gCost;
        public int hCost;

        public float movementPenalty;

        public ASNode parrent;

        int heapIndex;

        public ASNode(bool _walkable, Vector2Int _pos)
        {
            walkable = _walkable;
            position = _pos;
        }

        public int fCost {
            get
            {

                return gCost + hCost;
            }
        }

        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }

            set
            {
                heapIndex = value;
            }
        }

        public int CompareTo(ASNode other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0) {
                compare = hCost.CompareTo(other.hCost);
            }
            return -compare;
        }
    }
}

