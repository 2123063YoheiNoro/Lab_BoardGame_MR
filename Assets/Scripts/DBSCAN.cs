using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBSCAN
{
    // パラメータ
    public float Epsilon = 1.0f; // クラスター半径
    public int MinPoints = 3;   // クラスターを形成するための最小点数

    // DBSCANの結果: 各点のクラスタID (-1はノイズ)
    public List<int> ClusterLabels { get; private set; }

    // DBSCANを実行するメソッド
    public void RunDBSCAN(List<Vector2> points)
    {
        int clusterId = 0;
        ClusterLabels = new List<int>(new int[points.Count]);
        for (int i = 0; i < ClusterLabels.Count; i++)
        {
            ClusterLabels[i] = -1; // 未分類
        }

        for (int i = 0; i < points.Count; i++)
        {
            if (ClusterLabels[i] != -1) continue; // すでに分類済み

            // 現在の点の近傍点を取得
            List<int> neighbors = GetNeighbors(points, i);

            if (neighbors.Count < MinPoints)
            {
                // ノイズとしてラベル付け
                ClusterLabels[i] = -1;
                continue;
            }

            // 新しいクラスタを開始
            clusterId++;
            ExpandCluster(points, i, neighbors, clusterId);
        }
    }

    // クラスタを広げる
    private void ExpandCluster(List<Vector2> points, int pointIndex, List<int> neighbors, int clusterId)
    {
        ClusterLabels[pointIndex] = clusterId;

        Queue<int> neighborQueue = new Queue<int>(neighbors);

        while (neighborQueue.Count > 0)
        {
            int currentPoint = neighborQueue.Dequeue();

            if (ClusterLabels[currentPoint] == -1)
            {
                // ノイズだった点をクラスタに追加
                ClusterLabels[currentPoint] = clusterId;
            }

            if (ClusterLabels[currentPoint] != 0) continue;

            // クラスタに含める
            ClusterLabels[currentPoint] = clusterId;

            // 現在の点の近傍を探索
            List<int> currentNeighbors = GetNeighbors(points, currentPoint);
            if (currentNeighbors.Count >= MinPoints)
            {
                foreach (int neighbor in currentNeighbors)
                {
                    if (!neighborQueue.Contains(neighbor))
                    {
                        neighborQueue.Enqueue(neighbor);
                    }
                }
            }
        }
    }

    // 点の近傍を取得
    private List<int> GetNeighbors(List<Vector2> points, int pointIndex)
    {
        List<int> neighbors = new List<int>();

        for (int i = 0; i < points.Count; i++)
        {
            if (i == pointIndex) continue;

            float distance = Vector2.Distance(points[pointIndex], points[i]);
            if (distance <= Epsilon)
            {
                neighbors.Add(i);
            }
        }

        return neighbors;
    }
}