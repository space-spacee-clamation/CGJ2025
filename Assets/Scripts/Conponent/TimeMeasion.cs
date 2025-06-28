using System.Collections;
using UnityEngine;
public class TimeMeasion : ABaseControlAble
{

    protected override void Start()
    {
        base.Start();
        OnControl += () => {
            BoPian();
        };
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
        yield return new WaitForSeconds(ConstClass.TIME_CHANGE_TIME);
        LeaveControl();
    }
}