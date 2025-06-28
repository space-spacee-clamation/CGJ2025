using System;
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
    private GameTimeSetting _setting;
    private int nowIndex=0;
    public GameTimeEnum GameTime {
        get; private set;
    }
    private void OnEnable()
    {
        Destroy(Instance);
        Instance = this;
        var settingTrans=  GameObject.Find("LevelSetting");
        LevelSetting setting = settingTrans.GetComponent<LevelSetting>();
        OnChangeTime = null;
        RefreshSetting(setting.gameTimeSetting);
    }

    private void RefreshSetting(GameTimeSetting settingGameTimeSetting)
    {
        _setting = settingGameTimeSetting;
        ChangeWeather(0);
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
        GameTime=_setting.WeaterLists[index].Weather;
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
        nowIndex = (nowIndex+1)%_setting.WeaterLists.Count;
        ChangeWeather(nowIndex);
    }
}