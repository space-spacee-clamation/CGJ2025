using System;
using System.Collections.Generic;
using UnityEngine;
public enum GameTimeEnum
{
Day,
Night
}
public class TimeController : MonoBehaviour
{
    public static TimeController Instance
    {
        get;
        private set;
    }
    public Action<GameTimeEnum> OnChangeTime;
    private List<GameTimeEnum> _setting=new List<GameTimeEnum>(){GameTimeEnum.Day,GameTimeEnum.Night};
    private int nowIndex=0;
    public GameTimeEnum GameTime {
        get; private set;
    }
    private void OnEnable()
    {
        Destroy(Instance);
        Instance = this;
        OnChangeTime = null;
    }

    public float Time { get; private set; }
    public void SubObj(IChangeWithTime obj)
    {
        OnChangeTime += obj.ChangeWithWeather;
        obj.ChangeWithWeather(GameTime);
    }
    private void ChangeWeather(int index)
    {
        nowIndex = index;
        GameTime=_setting[index];
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
        nowIndex = (nowIndex+1)%_setting.Count;
        ChangeWeather(nowIndex);
    }
}