using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFarm.Astar
{
    public class GridNodes
    {
        private int width;
        private int height;
        private Node[,] gridNodes;

        public GridNodes(int width, int height)
        {
            this.width = width;
            this.height = height;
            gridNodes = new Node[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    gridNodes[x, y] = new Node(new Vector2Int(x, y));
                }
            }
        }
        //根据坐标返回节点信息
        public Node GetNodeDetails(int posX, int posY)
        {
            if (posX <= width && posY <= height)
            {
                return gridNodes[posX, posY];
            }
            else
            {
                Debug.Log("超出网格范围");
                return null;
            }
        }
    }
}
