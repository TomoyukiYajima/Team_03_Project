using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechTest : MonoBehaviour {
    //音声コマンドのキーワード
    private string[] m_Keywords = { "すすめ", "とまれ"};

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

        //認識したキーワードで処理判定
        switch (args.text)
        {
            case "すすめ":
                Move();
                break;
            case "とまれ":
                Stop();
                break;
        }
    }

    private void Move()
    {
        m_player.GetComponent<Rigidbody>().velocity = Vector3.one;
    }
    private void Stop()
    {
        m_player.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
