using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.EventSystems;

public class SpeechManager : MonoBehaviour {
    //音声コマンドのキーワード
    private string[] m_Keywords = { "すすめ", "すすんで", "ぜんしんせよ", "いどうしろ", "とまれ","じばくしろ","みぎにあわれ","ひだりにまわれ" };

    private KeywordRecognizer m_Recognizer;

    public GameObject m_player;

    void Start()
    {
        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        //ログ出力
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        // オーダー初期化
        OrderStatus order = OrderStatus.NULL;
        
        //認識したキーワードで処理判定
        switch (args.text)
        {
            case "すすめ":
            case "すすんで":
            case "ぜんしんせよ":
            case "いどうしろ":
                order = OrderStatus.MOVE;
                break;
            case "とまれ":
                order = OrderStatus.STOP;
                break;
            case "じばくしろ":
                order = OrderStatus.MOVE;
                break;
            case "みぎにあわれ":
                order = OrderStatus.TURN_RIGHT;
                break;
            case "ひだりにまわれ":
                order = OrderStatus.TURN_LEFT;
                break;
        }

        // ワーカーリスト取得
        var workerList = GameObject.FindGameObjectsWithTag("Worker");

        // 全部のワーカにオーダーを出す
        foreach (var worker in workerList)
        {
            // IWorkerEventが実装されていなければreturn
            if (!ExecuteEvents.CanHandleEvent<IWorkerEvent>(worker))
            {
                Debug.Log("IWorkerEvent未実装");
                return;
            }

            ExecuteEvents.Execute<IWorkerEvent>(
                worker,
                null,
                (receive, y) => receive.onOrder(order));
        }

    }
}
