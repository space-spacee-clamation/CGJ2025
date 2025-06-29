using System;
using System.Collections.Generic;
using UnityEngine;
public enum GameTimeEnum
{
    Day,
    Night,
}
public class TimeController : MonoBehaviour
{
    [SerializeField] private  List<GameTimeEnum> _setting = new List<GameTimeEnum>
        { GameTimeEnum.Day, GameTimeEnum.Night };
    private int nowIndex ;
    public Action<GameTimeEnum> OnChangeTime;
    public static TimeController Instance {
        get;
        private set;
    }
    public GameTimeEnum GameTime {
        get;
        private set;
    }

    public float Time {
        get;
    }
    private void OnEnable()
    {
        Destroy(Instance);
        Instance = this;
        OnChangeTime = null;
    }
    public void SubObj(IChangeWithTime obj)
    {
        OnChangeTime += obj.ChangeWithWeather;
        obj.ChangeWithWeather(GameTime);
    }
    public void UnSubObj(IChangeWithTime obj)
    {
        OnChangeTime -= obj.ChangeWithWeather;
    }
    private void ChangeWeather(int index)
    {
        nowIndex = index;
        GameTime = _setting[index];
        OnChangeTime?.Invoke(GameTime);
    }
    // public void ChangeTick(float tickTime)
    // {
    //     Time -= tickTime;
    //     if (Time < 0)
    //         ChangeWeatherNext();
    // }
    public void ChangeWeatherNext()
    {
        // if(_setting==null)NewLevel();
        nowIndex = (nowIndex + 1) % _setting.Count;
        ChangeWeather(nowIndex);
    }
}