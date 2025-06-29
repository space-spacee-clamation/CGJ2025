using UnityEngine;
public class FinishDoor : ABaseControlAble
{
    [SerializeField] private GameObject effect;
    [SerializeField] private string nextLevel;
    private bool open  ;
    private void Start()
    {
        CollectionManager.Instance.OnFinishCollect += OnFinish;
        OnControl += Next;
        effect.SetActive(false);
    }
    private void Next()
    {
        GameManager.Instance.LoadLevel(nextLevel);
        AudioManager.Instance.PlayOnce("Pass");
    }
    public override void Input(ControlType type, object param)
    {
    }
    public override bool ControllAble()
    {
        return open;
    }
    private void OnFinish()
    {
        open = true;
        effect.SetActive( true);
    }
}