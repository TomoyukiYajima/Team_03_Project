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
    public OrderPair(string key, OrderStatus value) : base(key, value) { }
}

[System.Serializable]
public class DirectionDictionary : Serialize.TableBase<string, OrderDirection, DirectionPair> { }

/// <summary>
/// ジェネリックを隠すために継承してしまう
/// [System.Serializable]を書くのを忘れない
/// </summary>
[System.Serializable]
public class DirectionPair : Serialize.KeyAndValue<string, OrderDirection>
{
    public DirectionPair(string key, OrderDirection value) : base(key, value) { }
}
public class SpeechManager : SingletonBehaviour<SpeechManager>
{
    [SerializeField] private string m_path;
    [SerializeField] private string m_unlockFile;
    [SerializeField] private OrderDictionary m_orderDictionary;
    [SerializeField] private DirectionDictionary m_directionrDictionary;

    private KeywordRecognizer m_orderRecognizer;
    private KeywordRecognizer m_unlockRecognizer;

    private Dictionary<string,List<string>> m_orderKeyword = new Dictionary<string, List<string>>();
    private List<string> m_unlockKeyword = new List<string>();

    private GameObject m_robot;

    //#if !UNITY_EDITOR
    void Start()
    {
        List<string> keywords = new List<string>();
        foreach (var list in m_orderDictionary.GetTable())
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

        //{
        //    string[] key = { "すすめ", "いどうしろ" };
        //    m_orderRecognizer = new KeywordRecognizer(key);
        //    m_orderRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        //    m_orderRecognizer.Start();
        //}

        {
            //ストリームの生成、Open読み込み専門
            FileStream fs = new FileStream(m_path + m_unlockFile + ".txt", FileMode.Open);
            //ストリームから読み込み準備
            StreamReader sr = new StreamReader(fs);
            //読み込んで表示
            while (!sr.EndOfStream)
            {//最後の行に（なる以外）
                string line = sr.ReadLine();
                m_unlockKeyword.Add(line);
                Debug.Log(line);
            }
            //ストリームも終了させる
            sr.Close();
        }

        m_unlockRecognizer = new KeywordRecognizer(m_unlockKeyword.ToArray());
        m_unlockRecognizer.OnPhraseRecognized += OnUnlockPhrase;
        m_unlockRecognizer.Start();
        Debug.Log("キーワード読み取り完了");

        m_robot = GameObject.FindGameObjectWithTag("Robot");
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
        OrderDirection orderDir = OrderDirection.NULL;

        // キーワードがどのリストに入ったをチェック
        foreach (var list in m_orderKeyword)
        {
            foreach (var order in list.Value)
            {
                if (args.text != order) continue;
                m_orderDictionary.GetTable().TryGetValue(list.Key, out orderType);

                if (orderType != OrderStatus.LOOK) break;
                // 方向チェック
                foreach (var dir in m_directionrDictionary.GetTable())
                {
                    if (!args.text.Contains(dir.Key)) continue;
                    orderDir = dir.Value;
                    break;
                }
                break;
            }
        }

        if (orderType == OrderStatus.NULL) return;

        SendOrder(orderType, orderDir);
    }

    private void OnUnlockPhrase(PhraseRecognizedEventArgs args)
    {
        //ログ出力
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        foreach (var phrase in m_unlockKeyword)
        {
            if (args.text != phrase) continue;
            break;
        }

        UnlockDoor(args.text);
    }

    private void SendOrder(OrderStatus order, OrderDirection dir)
    {
        // ワーカーリスト取得
        //List<GameObject> workerList = new List<GameObject>();
        //workerList.AddRange(GameObject.FindGameObjectsWithTag("Robot"));
        //workerList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        //// シーンにいる全部のロボットを入れる
        //var robotList = GameObject.FindGameObjectsWithTag("Robot");

        //// 全部のロボットにオーダーを出す
        //foreach (var robot in robotList)
        //{
            // IRobotEventが実装されていなければreturn
            if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(m_robot))
            {
                Debug.Log("IOrderEvent未実装");
                return;
            }

            if (dir == OrderDirection.NULL)
            {
                ExecuteEvents.Execute<IOrderEvent>(
                    m_robot,
                    null,
                    (receive, y) => receive.onOrder(order));
            }
            else
            {
                ExecuteEvents.Execute<IOrderEvent>(
                    m_robot,
                    null,
                    (receive, y) => receive.onOrder(order, dir));
            }
        //}

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

    private void UnlockDoor(String password)
    {
        var doorList = GameObject.FindGameObjectsWithTag("LockedDoor");

        foreach (var door in doorList)
        {
            // IRobotEventが実装されていなければreturn
            if (!ExecuteEvents.CanHandleEvent<IGimmickEvent>(door))
            {
                Debug.Log("IEnemyEvent未実装");
                return;
            }

            ExecuteEvents.Execute<IGimmickEvent>(
                door,
                null,
                (receive, y) => receive.onActivate(password));
        }

    }


    private void OnApplicationQuit()
    {
        OnDestroy();
    }

    private void OnDestroy()
    {
        if (m_orderRecognizer != null && m_orderRecognizer.IsRunning)
        {
            m_orderRecognizer.OnPhraseRecognized -= OnPhraseRecognized;
            m_orderRecognizer.Start();
        }
        if (m_unlockRecognizer != null && m_unlockRecognizer.IsRunning)
        {
            m_unlockRecognizer.OnPhraseRecognized -= OnUnlockPhrase;
            m_unlockRecognizer.Start();
        }
    }
//#endif
}
