using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class TimeChange : MonoBehaviour , IChangeWithTime
{
    [SerializeField] private GameObject light;
    [SerializeField]
    private GameObject dark;
    [SerializeField] private Light2D _light2D;

    private void Start()
    {
        TimeController.Instance.SubObj(this);
    }
    private void Light()
    {
        light.SetActive(true);
        dark.SetActive(false);
        DOTween.To(() => _light2D.color, x => _light2D.color = x, new Color(1, 1, 1, 1), 1f);
    }
    private void Dark()
    {
        light.SetActive(false);
        dark.SetActive(true);
        DOTween.To(() => _light2D.color, x => _light2D.color = x, new Color(0.07785684f,0.1461979f,0.3056603f), 1f);
    }
    public void ChangeWithWeather(GameTimeEnum time)
    {
        switch (time)
        {
            case GameTimeEnum.Day:
                Light();
                break;
            case GameTimeEnum.Night:
                Dark();
                break;
        }
    }
}