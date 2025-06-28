using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    [SerializeField] private int count = 0;
    private List<CollectObject> hasGet = new List<CollectObject>();
    public static CollectionManager Instance;
    public Action OnFinishCollect;
    public bool Finish;
    private void OnEnable()
    {
        Destroy(Instance);
        Instance = this;
        Finish = false;
        OnFinishCollect+=()=>{
            Debug.Log("喵喵喵喵");
        };
    }
    public void Collection(CollectObject go)
    {
        if(hasGet.Contains( go))return;
        hasGet.Add(go);
        go.gameObject.SetActive(false);
        count--;
        if (count<=0)
        {
            Finish = true;
            OnFinishCollect?.Invoke();
        }
    }
}
