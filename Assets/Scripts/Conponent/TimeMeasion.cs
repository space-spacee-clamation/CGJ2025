using System.Collections;
using UnityEngine;
public class TimeMeasion : ABaseControlAble , IChangeWithTime
{

    private Animator _animator;
    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
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
                _animator.SetBool("turnDark",false);
                break;
            case GameTimeEnum.Night:
                _animator.SetBool("turnDark",true);
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