using UnityEngine;
public class FinishDoor : ABaseControlAble
{
    [SerializeField] private GameObject effect;
    private string nextLevel;
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