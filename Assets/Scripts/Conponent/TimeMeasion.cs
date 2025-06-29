using System.Collections;
using DG.Tweening;
using UnityEngine;
public class TimeMeasion : ABaseControlAble , IChangeWithTime
{
    public SpriteRenderer Renderer;
    protected override void Start()
    {
        base.Start();
        TimeController.Instance.SubObj(this);
        OnControl += () => {
            BoPian();
        };
    }
    public void ChangeWithWeather(GameTimeEnum time)
    {
        switch (time)
        {
            case GameTimeEnum.Day:
                Renderer.DOFade(1, 0.5f);
                break;
            case GameTimeEnum.Night:
                Renderer.DOFade(0, 0.5f);
                break;
        }
    }
    public override void Input(ControlType type, object param)
    {
    }
    protected void BoPian()
    {
        StartCoroutine(ChangeTime());
    }
    protected IEnumerator ChangeTime()
    {
        TimeController.Instance.ChangeWeatherNext();
        DialogManager.Instance.ShowDialog("Made in Haven！", transform.position);
        yield return new WaitForSeconds(ConstClass.TIME_CHANGE_TIME);
        LeaveControl();
    }
}