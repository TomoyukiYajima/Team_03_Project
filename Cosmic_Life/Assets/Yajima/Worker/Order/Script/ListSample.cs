using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListSample : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //[System.Serializable]
    //public class Trans {
    //    public Transform[] m_Points;

    //    public void PointList(Transform[] points)
    //    {
    //        m_Points = points;
    //    }
    //}

    //[SerializeField]
    //private Transform[PointList]

    // リスト
    [System.Serializable]
    public class PointList
    {
        public List<int> m_Points = new List<int>();

        public PointList(List<int> points)
        {
            m_Points = points;
        }
    }

    [SerializeField]
    private List<PointList> list = new List<PointList>();

    // [SerializeField]
    // private Transform[][] m_Transforms;
    // 配列
    [System.Serializable]
    public class TransformPoint
    {
        public Transform[] m_Points;

        public TransformPoint(Transform[] points)
        {
            m_Points = points;
        }
    }

    [SerializeField]
    private TransformPoint point;
}