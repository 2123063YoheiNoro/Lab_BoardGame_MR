using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class PredictionAnalyser : MonoBehaviour
{
    [SerializeField] private float _epsilon = 100;  //クラスター半径.

    private TilesReceiver tilesReceiver;
    private void Start()
    {

        tilesReceiver = FindObjectOfType<TilesReceiver>();
        tilesReceiver.rpPredictions
            .Subscribe(t => OnTileChanged(t))
            .AddTo(this);
    }
    public void Clustering(IPredictions predictions)
    {
        List<Prediction> predictionList = predictions.GetAllPredictions();
        predictionList.OrderBy(obj => obj.x);
        List<Vector2> points = new();
        foreach (Prediction p in predictionList)
        {
            Vector2 vec2 = new Vector2(p.x, p.y);
            points.Add(vec2);
        }
        DBSCAN dbscan = new DBSCAN();
        dbscan.Epsilon = _epsilon;
        dbscan.MinPoints = 1;
        dbscan.RunDBSCAN(points);

        Dictionary<int, List<Prediction>> clusters = ConvertToClusterLists(predictionList, dbscan.ClusterLabels, true);

        string resultStr = "";
        // 辞書を列挙して内容を出力
        foreach (KeyValuePair<int, List<Prediction>> cluster in clusters)
        {
            int clusterId = cluster.Key;
            List<Prediction> pres = cluster.Value;

            resultStr += $"Cluster ID: {clusterId}";
            foreach (Prediction p in pres)
            {
                resultStr += $"  Point: {p.class_name}";
            }
            resultStr += "\n";
        }
        Debug.Log(resultStr);
    }

    private void OnTileChanged(IPredictions predictions)
    {
        Clustering(predictions);
    }
    public static Dictionary<int, List<Prediction>> ConvertToClusterLists(List<Prediction> predictions, List<int> clusterLabels, bool includeNoise = false)
    {
        // クラスターIDごとの辞書を作成
        Dictionary<int, List<Prediction>> clusters = new Dictionary<int, List<Prediction>>();
        for (int i = 0; i < clusterLabels.Count; i++)
        {
            int label = clusterLabels[i];

            // ノイズを無視する場合
            if (label == -1 && !includeNoise) continue;

            // クラスターが未登録の場合、新しく作成
            if (!clusters.ContainsKey(label))
            {
                clusters[label] = new List<Prediction>();
            }

            // 点をクラスターに追加
            clusters[label].Add(predictions[i]);
        }

        return clusters;
    }
}
