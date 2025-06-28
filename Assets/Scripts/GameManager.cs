using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action OnNewLevel;
    private IControlAble _player;
    
    public LevelMask levelMask;
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
    public void SubPlayer(IControlAble controlAble)
    {
        _player = controlAble;
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