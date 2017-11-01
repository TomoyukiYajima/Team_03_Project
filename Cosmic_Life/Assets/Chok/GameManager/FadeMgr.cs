using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeMgr : SingletonBehaviour<FadeMgr>
{
    [SerializeField] private CanvasGroup m_fadeGroup;
    [SerializeField] private Image m_loadingBar;

    public void FadeIn(float duration, Action action = null)
    {
        m_fadeGroup.DOFade(0, duration).OnComplete(() => {
            m_fadeGroup.blocksRaycasts = false;
            if (action != null)
            {
                action();
            }
        });
    }

    public void FadeOut(float duration, Action action = null)
    {
        m_fadeGroup.DOFade(1, duration).OnComplete(() => {
            m_fadeGroup.blocksRaycasts = true;
            if (action != null)
            {
                action();
            }
        });
    }

    public void FillBar(float amount)
    {
        m_loadingBar.fillAmount = amount;
    }
}
