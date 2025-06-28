using System;
using System.Collections.Generic;
using UnityEngine;
public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;
    [SerializeField] private int count  ;
    public bool Finish;
    private readonly List<CollectObject> hasGet = new List<CollectObject>();
    public Action OnFinishCollect;
    private void OnEnable()
    {
        Destroy(Instance);
        Instance = this;
        Finish = false;
        OnFinishCollect += () => {
            Debug.Log("喵喵喵喵");
        };
    }
    public void Collection(CollectObject go)
    {
        if (hasGet.Contains( go)) return;
        hasGet.Add(go);
        go.gameObject.SetActive(false);
        count--;
        if (count <= 0)
        {
            Finish = true;
            OnFinishCollect?.Invoke();
        }
    }
}