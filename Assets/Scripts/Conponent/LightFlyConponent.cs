using UnityEngine;

public class LightFlyConponent : MonoBehaviour , IChangeWithWeather
{
    void Start()
    {
        TimeController.Instance.SubObj(this);
    }
    public void ChangeWithWeather(GameTimeEnum gameTime)
    {
        
    }

}
