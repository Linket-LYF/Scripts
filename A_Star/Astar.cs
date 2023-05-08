using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFarm.Map;

namespace MyFarm.Astar
{
    public class Astar : Singleton<Astar>
    {
        private GridNodes currentMapGridNodes;//当前地图的所有节点信息

        private Node startNode;
        private Node targetNode;
        private int gridWidth;
        private int gridHeight;
        private int orginX;
        private int originY;
        private List<Node> aroundNode;//周围可选的8个结点
        private HashSet<Node> selectedNode;//所有被选中的不重复的点
        private bool isFindPath;//是否找到路径

        public void BuildPath(string sceneName, Vector2Int startPos, Vector2Int endPos, Stack<MovementStep> NpcMovementStep)
        {
            isFindPath = false;
            if (GenerateGridNodes(sceneName, startPos, endPos))
            {
                //查找最短路径
                if (FindShortestPath())
                {
                    //构建npc最短路径
                    UpdateMovementStepStack(sceneName, NpcMovementStep);
                }

            }
        }

        //生成网格结点 初始化列表
        public bool GenerateGridNodes(string sceneName, Vector2Int startPos, Vector2Int endPos)
        {
            if (GridMapManager.Instance.GetGridNodeRange(sceneName, out Vector2Int gridNodeRange, out Vector2Int gridOrigin))
            {
                //根据瓦片地图范围构建网格移动节点范围数组
                currentMapGridNodes = new GridNodes(gridNodeRange.x, gridNodeRange.y);
                gridWidth = gridNodeRange.x;
                gridHeight = gridNodeRange.y;
                orginX = gridOrigin.x;
                originY = gridOrigin.y;

                aroundNode = new List<Node>();
                selectedNode = new HashSet<Node>();
            }
            else
                return false;
            //gridNodes的范围是从0,0开始所以需要减去原点坐标得到实际位置
            startNode = currentMapGridNodes.GetNodeDetails(startPos.x - orginX, startPos.y - originY);
            targetNode = currentMapGridNodes.GetNodeDetails(endPos.x - orginX, endPos.y - originY);
            //print("targetNode:" + targetNode);
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    var key = (x + orginX) + "x" + (y + originY) + "y" + sceneName;
                    TileDetails tileDetails = GridMapManager.Instance.GetTileDetails(key);
                    if (tileDetails != null)
                    {
                        Node node = new Node(new Vector2Int(x, y));
                        if (tileDetails.isNPCObstacle)
                            node.isObstacle = true;
                    }

                }
            }
            return true;
        }
        //寻找最短路径
        public bool FindShortestPath()
        {
            aroundNode.Add(startNode);
            while (aroundNode.Count > 0)
            {
                aroundNode.Sort();
                Node shortestNode = aroundNode[0];
                aroundNode.RemoveAt(0);
                selectedNode.Add(shortestNode);
                if (shortestNode == targetNode)
                {
                    isFindPath = true;
                    break;
                }
                //计算周围8点个并添加到list中
                EvaluateNeighbourNodes(shortestNode);

            }
            return isFindPath;
        }
        //找到可用的8个结点
        private void EvaluateNeighbourNodes(Node currentNode)
        {
            Vector2Int currentNodePos = currentNode.gridPos;
            Node neighbourNode;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    neighbourNode = VaildNeighbourNode(currentNodePos.x + x, currentNodePos.y + y);
                    if (neighbourNode != null)
                    {
                        if (!aroundNode.Contains(neighbourNode))
                        {
                            neighbourNode.gCost = currentNode.gCost + GerDistance(neighbourNode, currentNode);
                            neighbourNode.hCost = GerDistance(neighbourNode, targetNode);
                            neighbourNode.parentNode = currentNode;
                            aroundNode.Add(neighbourNode);
                        }
                    }
                }
            }
        }
        //找到可用的结点
        private Node VaildNeighbourNode(int x, int y)
        {
            if (x > gridWidth || y > gridHeight || x < 0 || y < 0)
                return null;
            Node neighbourNode = currentMapGridNodes.GetNodeDetails(x, y);
            if (neighbourNode.isObstacle || selectedNode.Contains(neighbourNode))
                return null;
            else
                return neighbourNode;
        }
        //返回两点间的距离值
        private int GerDistance(Node A, Node B)
        {
            int distanceX = Mathf.Abs(B.gridPos.x - A.gridPos.x);
            int distanceY = Mathf.Abs(B.gridPos.y - A.gridPos.y);
            if (distanceX > distanceY)
                return 14 * distanceY + 10 * (distanceX - distanceY);
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
        //更新路径中每一步的网格坐标和场景名称
        private void UpdateMovementStepStack(string sceneName, Stack<MovementStep> NpcMovementStep)
        {

            Node currentNode = targetNode;
            while (currentNode != null)
            {
                MovementStep newMovementStep = new MovementStep();
                newMovementStep.sceneName = sceneName;
                newMovementStep.gridPos = new Vector2Int(currentNode.gridPos.x + orginX, currentNode.gridPos.y + originY);

                NpcMovementStep.Push(newMovementStep);

                currentNode = currentNode.parentNode;
            }
        }
    }
}