using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    [SerializeField, Tooltip("カメラリスト")] private List<GameObject> m_cameraList;

    private GameObject m_currentCamera;

    //private int m_currentCamera = 0;

    public void Start()
    {
        m_currentCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    StartCoroutine(SwitchingCamera(1));
        //}
    }

    public void SwitchCamera(GameObject camera)
    {
        StartCoroutine(SwitchingCamera(camera));
    }
    public void SwitchCamera(Transform camera,float duration)
    {
        StartCoroutine(SwitchingCamera(camera, duration));
    }

    //private IEnumerator SwitchingCamera(int cameraNum)
    //{
    //    // フェードアウト
    //    FadeMgr.Instance.FadeOut(0.5f);
    //    // 0.5秒待つ
    //    yield return new WaitForSeconds(0.5f);
    //    // 今使っているカメラを非アクティブする
    //    m_cameraList[m_currentCamera].SetActive(false);
    //    // 次のカメラに移行
    //    m_currentCamera += cameraNum;
    //    if (m_currentCamera >= m_cameraList.Count) m_currentCamera %= m_cameraList.Count;
    //    if (m_currentCamera < 0) m_currentCamera = m_cameraList.Count - 1;
    //    // 新しいカメラをアクティブする
    //    m_cameraList[m_currentCamera].SetActive(true);
    //    // フェードイン
    //    FadeMgr.Instance.FadeIn(0.5f);
    //}

    /// <summary>
    /// カメラを切り替える
    /// </summary>
    /// <param name="camera">カメラオブジェクト</param>
    /// <returns></returns>
    private IEnumerator SwitchingCamera(GameObject camera)
    {
        // フェードアウト
        FadeMgr.Instance.FadeOut(0.5f);
        // 0.5秒待つ
        yield return new WaitForSeconds(0.5f);
        // 今使っているカメラを非アクティブする
        m_currentCamera.SetActive(false);
        // 次のカメラに移行
        m_currentCamera = camera;
        // 新しいカメラをアクティブする
        m_currentCamera.SetActive(true);
        m_currentCamera.transform.GetChild(0).gameObject.SetActive(true);
        // フェードイン
        FadeMgr.Instance.FadeIn(0.5f);
    }

    /// <summary>
    /// カメラを切り替える（移動）
    /// </summary>
    /// <param name="obj">目標トランスフォーム</param>
    /// <param name="duration">時間</param>
    /// <returns></returns>
    private IEnumerator SwitchingCamera(Transform obj,float duration)
    {
        m_currentCamera.transform.DOMove(obj.position, duration).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(1.0f);
    }

    public void AddCamera(GameObject camera)
    {
        m_cameraList.Add(camera);
    }

    public void EraseCamera(GameObject camera)
    {
        foreach (var c in m_cameraList)
        {
            if (camera != c) continue;
            m_cameraList.Remove(camera);
            break;
        }
    }
}
