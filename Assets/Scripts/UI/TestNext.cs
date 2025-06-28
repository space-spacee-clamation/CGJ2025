using UnityEngine;
public class TestNext : MonoBehaviour
{
    public void Next(string srcName)
    {
        GameManager.Instance.LoadLevel(srcName);
    }
}
