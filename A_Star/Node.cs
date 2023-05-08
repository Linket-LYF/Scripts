using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MyFarm.Astar
{
    public class Node : IComparable<Node>
    {

        public Vector2Int gridPos;//网格坐标
        public int gCost;//距离起点距离
        public int hCost;//距离终点距离
        public int F_Cost => gCost + hCost;//当前网格权重
        public bool isObstacle;//是否有障碍物
        public Node parentNode;
        public Node(Vector2Int currentPos)
        {
            gridPos = currentPos;
            parentNode = null;
        }

        public int CompareTo(Node other)
        {
            //选出最小的F。返回-1,0,1
            int result = F_Cost.CompareTo(other.F_Cost);
            if (result == 0)
                result = hCost.CompareTo(other.hCost);
            return result;

        }
    }
}

