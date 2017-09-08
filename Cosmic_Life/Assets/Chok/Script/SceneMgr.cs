using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    LogoScene,
    Title,
    sample,
}

public class SceneMgr : SingletonBehaviour<SceneMgr>
{
    // 現在のシーン
    private SceneType m_currentScene = SceneType.LogoScene;
    // 遷移完了か
    private bool m_isEnd = true;
    // フェードしたか
    private bool m_isFade = false;
    // フェード時間の長さ
    private float m_duration = 0.5f;
    // 遷移の作業
    private AsyncOperation m_async;

    /// <summary>
    /// Fadeしてから遷移
    /// </summary>
    /// <param name="name">シーンの名前</param>
    public void SceneTransition(SceneType name)
    {
        if (!m_isEnd) return;
        m_isFade = true;
        m_isEnd = false;
        FadeMgr.Instance.FadeOut(m_duration, () => { m_isFade = false; });
        StartCoroutine(transitionAsync(name, m_duration));
    }

    IEnumerator transition(SceneType name,float duration)
    {
        yield return new WaitWhile(() => m_isFade);

        yield return SceneManager.LoadSceneAsync(name.ToString(), LoadSceneMode.Additive);

        UnLoadScene(m_currentScene);
        m_currentScene = name;

        if (duration != 0)
        {
            FadeMgr.Instance.FadeIn(duration, () =>
            {
                Debug.Log(name.ToString() + "_Scene : LoadComplete!!");
            });
        }
        else
        {
            Debug.Log(name.ToString() + "_Scene : LoadComplete!!");
        }

    }

    IEnumerator transitionAsync(SceneType name, float duration)
    {
        //FadeMgr.Instance.countText.enabled = true;
        //FadeMgr.Instance.animeObj.SetActive(true);

        yield return new WaitWhile(() => m_isFade);

        m_async = SceneManager.LoadSceneAsync(name.ToString(), LoadSceneMode.Additive);
        // 読み込みが終わっても表示しない
        m_async.allowSceneActivation = false;

        while (!m_async.isDone)
        {
            FadeMgr.Instance.FillBar(m_async.progress / 0.9f);
            yield return null;
        }

        //// 読み込み終わるまで待機(0.9までしか増えない)
        //while (m_async.progress < 0.9f)
        //{
        //    FadeMgr.Instance.SetCounter(m_async.progress);
        //    yield return 0;
        //}

        //// 強制的に100％に
        //FadeMgr.Instance.SetCounter(1);

        // 90%->100%のTwennが終わるまで待機
        //yield return new WaitWhile(() => FadeMgr.Instance.Counter != 100);

        // Load完了から少し待つ
        yield return new WaitForSeconds(2);

        // 読み込みが完了しているSceneを表示
        m_async.allowSceneActivation = true;

        yield return null;

        UnLoadScene(m_currentScene);
        m_currentScene = name;

        if (duration != 0)
        {
            FadeMgr.Instance.FadeIn(duration, () =>
            {
                //FadeMgr.Instance.countText.enabled = false;
                //FadeMgr.Instance.animeObj.SetActive(false);
                //FadeMgr.Instance.SetCounter(0);
                //MyDebug.Log(name.ToString() + "_Scene : LoadComplete!!");
            });
        }
        else
        {
            //FadeMgr.Instance.countText.enabled = false;
            //FadeMgr.Instance.animeObj.SetActive(false);
            //FadeMgr.Instance.SetCounter(0);
            //MyDebug.Log(name.ToString() + "_Scene : LoadComplete!!");
        }
    }

    // 現在読み込んでいるシーンType
    public SceneType GetCurrentSceneType()
    {
        return m_currentScene;
    }

    public void UnLoadScene(SceneType name)
    {
        Debug.Log(name.ToString() + "_Scene : UnLoad!!");
        SceneManager.UnloadSceneAsync(name.ToString());
    }


}
