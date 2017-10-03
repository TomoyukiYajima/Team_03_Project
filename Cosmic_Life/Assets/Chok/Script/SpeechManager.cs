using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class OrderDictionary : Serialize.TableBase<string, OrderStatus, OrderPair> { }

/// <summary>
/// ジェネリックを隠すために継承してしまう
/// [System.Serializable]を書くのを忘れない
/// </summary>
[System.Serializable]
public class OrderPair : Serialize.KeyAndValue<string, OrderStatus>
{
    public OrderPair(string key, OrderStatus value) : base(key, value){}
}
public class SpeechManager : MonoBehaviour
{
    [SerializeField] private string m_path;
    [SerializeField] private OrderDictionary m_orderDictionary;

    private List<string> fileList;
    private KeywordRecognizer m_orderRecognizer;
    private KeywordRecognizer m_unlockRecognizer;

    private Dictionary<string,List<string>> m_orderKeyword = new Dictionary<string, List<string>>();

    //#if !UNITY_EDITOR
    void Start()
    {
        List<string> keywords = new List<string>();
        foreach(var list in m_orderDictionary.GetTable())
        {
            List<string> keywordList = new List<string>();
            //ストリームの生成、Open読み込み専門
            FileStream fs = new FileStream(m_path + list.Key + ".txt", FileMode.Open);
            //ストリームから読み込み準備
            StreamReader sr = new StreamReader(fs);
            //読み込んで表示
            while (!sr.EndOfStream)
            {//最後の行に（なる以外）
                string line = sr.ReadLine();
                keywordList.Add(line);
                Debug.Log(line);
            }
            //ストリームも終了させる
            sr.Close();
            m_orderKeyword.Add(list.Key, keywordList);
            keywords.AddRange(keywordList);
        }

        // キーワードを格納
        m_orderRecognizer = new KeywordRecognizer(keywords.ToArray());
        m_orderRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_orderRecognizer.Start();
    }

    // キーワードを読み取ったら実行するメソッド
    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        //ログ出力
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        // オーダー初期化
        OrderStatus orderType = OrderStatus.NULL;

        foreach (var list in m_orderKeyword)
        {
            foreach(var order in list.Value)
            {
                if (args.text != order) continue;
                m_orderDictionary.GetTable().TryGetValue(list.Key, out orderType);
                break;
            }
        }

        //認識したキーワードで処理判定
        //switch (args.text)
        //{
        //    case "すすめ":
        //    case "すすんで":
        //    case "ぜんしんせよ":
        //    case "いどうしろ":
        //        orderType = OrderStatus.MOVE;
        //        break;
        //    case "とまれ":
        //        orderType = OrderStatus.STOP;
        //        break;
        //    case "じばくしろ":
        //        orderType = OrderStatus.MOVE;
        //        break;
        //    case "みぎにまわれ":
        //        orderType = OrderStatus.TURN_RIGHT;
        //        break;
        //    case "ひだりにまわれ":
        //        orderType = OrderStatus.TURN_LEFT;
        //        break;
        //    case "こうげきしろ":
        //    case "こわせ":
        //    case "うて":
        //    case "はかいしろ":
        //        orderType = OrderStatus.ATTACK;
        //        break;
        //}

        if (orderType == OrderStatus.NULL) return;

        SendOrder(orderType, OrderDirection.NULL);

    }

    private void SendOrder(OrderStatus order, OrderDirection dir)
    {
        // ワーカーリスト取得
        //List<GameObject> workerList = new List<GameObject>();
        //workerList.AddRange(GameObject.FindGameObjectsWithTag("Robot"));
        //workerList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        var robotList = GameObject.FindGameObjectsWithTag("Robot");

        // 全部のワーカにオーダーを出す
        foreach (var robot in robotList)
        {
            // IRobotEventが実装されていなければreturn
            if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(robot))
            {
                Debug.Log("IOrderEvent未実装");
                return;
            }

            ExecuteEvents.Execute<IOrderEvent>(
                robot,
                null,
                (receive, y) => receive.onOrder(order, dir));
        }

        var enemyList = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemyList)
        {
            // IRobotEventが実装されていなければreturn
            if (!ExecuteEvents.CanHandleEvent<IEnemyEvent>(enemy))
            {
                Debug.Log("IEnemyEvent未実装");
                return;
            }

            ExecuteEvents.Execute<IEnemyEvent>(
                enemy,
                null,
                (receive, y) => receive.onHear());
        }
    }


    private void OnApplicationQuit()
    {
        OnDestroy();
    }

    private void OnDestroy()
    {
        if (m_orderRecognizer == null || !m_orderRecognizer.IsRunning) return;
        m_orderRecognizer.OnPhraseRecognized -= OnPhraseRecognized;
        m_orderRecognizer.Start();
    }
    //#endif
}
