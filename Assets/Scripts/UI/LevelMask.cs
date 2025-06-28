
    using System;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;
    public class LevelMask : MonoBehaviour
    {
        [SerializeField] private Image m_Image;
        [SerializeField] private float m_Duration;
        public void Open(Action callBack=null)
        {
            m_Image.DOFade(1, m_Duration).OnComplete(()=>callBack?.Invoke());
        }
        public void Close(Action callBack=null)
        {
            m_Image.DOFade(0, m_Duration).OnComplete(()=>callBack?.Invoke());;
        }
    }
