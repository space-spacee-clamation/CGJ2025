using System;
using DG.Tweening;
using UnityEngine;
public class CameraTest : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private void Start()
    {
        TimeController.Instance.OnChangeTime += ChangeTime;
        _camera = GetComponent<Camera>();
    }
    private void ChangeTime(GameTimeEnum obj)
    {
        Debug.Log(obj.ToString());
        switch (obj)
        {
            case GameTimeEnum.Day:
                DOTween.To(() => _camera.backgroundColor, x => _camera.backgroundColor = x, Color.white, ConstClass.TIME_CHANGE_TIME);
                break;
            case GameTimeEnum.Night:
                DOTween.To(() => _camera.backgroundColor, x => _camera.backgroundColor = x, Color.gray, ConstClass.TIME_CHANGE_TIME);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
        }
    }
}