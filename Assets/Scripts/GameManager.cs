using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public LevelMask levelMask;
    private IControlAble _player;
    public Action OnNewLevel;
    public Transform PlayerTran{get;private set;}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        OnNewLevel += OpenMask;
    }
    private void OpenMask()
    {
        levelMask.Close();
    }
    public void SubPlayer(IControlAble controlAble,Transform transform)
    {
        _player = controlAble;
        PlayerTran = transform;
    }
    public IControlAble GetPlayer()
    {
        if (_player != null)
            return _player;
        throw new Exception("未注册玩家");
    }
    public void ReStart()
    {
        StopAllCoroutines();
        StartCoroutine(LoadLevelCon(SceneManager.GetActiveScene().name));
    }
    public IEnumerator LoadLevelCon(string name)
    {
        levelMask.Open();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(name);
        yield return new WaitForSeconds(1.5f);
        EnterLevel();
    }
    private void EnterLevel()
    {
        levelMask.Close(OnNewLevel);
    }
    public void LoadLevel(string levelName)
    {
        StopAllCoroutines();
        StartCoroutine(LoadLevelCon(levelName));
    }
}