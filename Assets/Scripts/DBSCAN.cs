using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBSCAN
{
    // �p�����[�^
    public float Epsilon = 1.0f; // �N���X�^�[���a
    public int MinPoints = 3;   // �N���X�^�[���`�����邽�߂̍ŏ��_��

    // DBSCAN�̌���: �e�_�̃N���X�^ID (-1�̓m�C�Y)
    public List<int> ClusterLabels { get; private set; }

    // DBSCAN�����s���郁�\�b�h
    public void RunDBSCAN(List<Vector2> points)
    {
        int clusterId = 0;
        ClusterLabels = new List<int>(new int[points.Count]);
        for (int i = 0; i < ClusterLabels.Count; i++)
        {
            ClusterLabels[i] = -1; // ������
        }

        for (int i = 0; i < points.Count; i++)
        {
            if (ClusterLabels[i] != -1) continue; // ���łɕ��ލς�

            // ���݂̓_�̋ߖT�_���擾
            List<int> neighbors = GetNeighbors(points, i);

            if (neighbors.Count < MinPoints)
            {
                // �m�C�Y�Ƃ��ă��x���t��
                ClusterLabels[i] = -1;
                continue;
            }

            // �V�����N���X�^���J�n
            clusterId++;
            ExpandCluster(points, i, neighbors, clusterId);
        }
    }

    // �N���X�^���L����
    private void ExpandCluster(List<Vector2> points, int pointIndex, List<int> neighbors, int clusterId)
    {
        ClusterLabels[pointIndex] = clusterId;

        Queue<int> neighborQueue = new Queue<int>(neighbors);

        while (neighborQueue.Count > 0)
        {
            int currentPoint = neighborQueue.Dequeue();

            if (ClusterLabels[currentPoint] == -1)
            {
                // �m�C�Y�������_���N���X�^�ɒǉ�
                ClusterLabels[currentPoint] = clusterId;
            }

            if (ClusterLabels[currentPoint] != 0) continue;

            // �N���X�^�Ɋ܂߂�
            ClusterLabels[currentPoint] = clusterId;

            // ���݂̓_�̋ߖT��T��
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

    // �_�̋ߖT���擾
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