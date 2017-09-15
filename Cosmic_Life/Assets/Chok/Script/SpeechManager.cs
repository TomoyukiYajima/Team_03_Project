using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpeechManager : MonoBehaviour
{
    //音声コマンドのキーワード
    //private string[] m_Keywords = { "すすめ", "すすんで", "ぜんしんせよ", "いどうしろ", "とまれ", "じばくしろ", "みぎにまわれ", "ひだりにまわれ" };

    private KeywordRecognizer m_Recognizer;

    private List<string> m_moveKeyword = new List<string>();

    //#if !UNITY_EDITOR
    void Start()
    {
        string path = "Assets/Resources/";
        //ストリームの生成、Open読み込み専門
        FileStream fs = new FileStream(path + "move.txt", FileMode.Open);
        //ストリームから読み込み準備
        StreamReader sr = new StreamReader(fs);
        //読み込んで表示
        while (!sr.EndOfStream)
        {//最後の行に（なる以外）
            string line = sr.ReadLine();
            m_moveKeyword.Add(line);
            Debug.Log(line);
        }
        //ストリームも終了させる
        sr.Close();

        string[] m_Keywords = new string[m_moveKeyword.Count];

        for (int i = 0; i < m_moveKeyword.Count; ++i)
        {
            m_Keywords[i] = m_moveKeyword[i];
        }

        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        // ワーカへ命令しているか
        bool leftorder = false;
        bool rightorder = false;

        if (PlayerInputManager.GetInputStay(InputState.INPUT_TRIGGER_LEFT)) leftorder = true;
        if (PlayerInputManager.GetInputStay(InputState.INPUT_TRIGGER_RIGHT)) rightorder = true;

        if (!leftorder && !rightorder) return;

        //ログ出力
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        // オーダー初期化
        OrderStatus orderType = OrderStatus.NULL;

        foreach(var text in m_moveKeyword)
        {
            if (args.text != text) continue;
            orderType = OrderStatus.MOVE;
            break;
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
        //}

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
                (receive, y) => receive.onOrder(orderType));
        }

    }

//#endif
}
