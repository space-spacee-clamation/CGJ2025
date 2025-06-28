using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeatherSetting", menuName = "ScriptableObject/WeatherSetting")]
public class GameTimeSetting : ScriptableObject
{
    public List<SingleWeater> WeaterLists;
}
[Serializable]
public class SingleWeater
{
    public GameTimeEnum Weather;
    public float Time;
}