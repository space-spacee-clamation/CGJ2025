using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private DialogBox Dialog;
    public void OnEnable()
    {
        Destroy(Instance);
        Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.OnNewLevel+=()=>
        {
            _canvas.worldCamera=Camera.main;
        };
    }
    public void ShowDialog(string massage,Vector3 pos)
    {
        GameObject go = Instantiate(Dialog.gameObject,_canvas.transform);
        go.GetComponent<DialogBox>().Init(pos,massage);
        go.GetComponent<DialogBox>().Show(massage.Length*0.4f);
    }
}
