using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDoor : ABaseControlAble
{
    private bool open = false;
    private string nextLevel;
    [SerializeField] GameObject effect;
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
        return;
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
