using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Dictionary<string, AudioResourceDataPAre> AudioResources = new Dictionary<string, AudioResourceDataPAre>();
    public List<AudioSource> AudioSources = new List<AudioSource>();
    public AudioAssetSO audioAsset; // 拖拽分配ScriptableObject资源
    
    private AudioSource _musicSource;      // 专用的背景音乐源
    private Coroutine _activeFadeCoroutine; // 当前活动的渐变协程
    private const float FADE_DURATION = 1.5f; // 默认淡入淡出时间

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保持跨场景
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Start()
    {
        InitializeAudioResources();
        SetupAudioSources();
    }

    // 初始化音频资源字典
    private void InitializeAudioResources()
    {
        if (audioAsset == null)
        {
            Debug.LogError("AudioAssetSO not assigned!");
            return;
        }

        foreach (var clipData in audioAsset.clips)
        {
            if (!AudioResources.ContainsKey(clipData.name))
            {
                AudioResources.Add(clipData.name, clipData);
            }
            else
            {
                Debug.LogWarning($"Duplicate audio name found: {clipData.name}");
            }
        }
    }

    // 设置音频源
    private void SetupAudioSources()
    {
        if (AudioSources.Count > 0)
        {
            // 使用第一个AudioSource作为背景音乐专用源
            _musicSource = AudioSources[0];
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;
        }
    }

    // 播放单次音效
    public void PlayOnce(string clipName)
    {
        if (!AudioResources.TryGetValue(clipName, out var audioData))
        {
            Debug.LogWarning($"Audio clip not found: {clipName}");
            return;
        }

        if (AudioSources.Count < 2)
        {
            Debug.LogError("No available audio sources for SFX!");
            return;
        }

        // 查找空闲音频源（从索引1开始，0是背景音乐）
        AudioSource availableSource = null;
        for (int i = 1; i < AudioSources.Count; i++)
        {
            if (!AudioSources[i].isPlaying)
            {
                availableSource = AudioSources[i];
                break;
            }
        }

        // 没有空闲源则使用最早结束的源
        if (availableSource == null)
        {
            availableSource = AudioSources[1];
            // 这里不停止当前播放，可能重叠，实际项目可优化
        }

        // 播放音效
        availableSource.clip = audioData.clip;
        availableSource.Play();
    }

    // 播放背景音乐
    public void PlayMusic(string musicName, bool fade = true)
    {
        if (_musicSource == null)
        {
            Debug.LogError("Background music source not set!");
            return;
        }

        if (!AudioResources.TryGetValue(musicName, out var musicData))
        {
            Debug.LogWarning($"Music clip not found: {musicName}");
            return;
        }

        // 相同音乐且正在播放则忽略
        if (_musicSource.clip == musicData.clip && _musicSource.isPlaying)
            return;

        // 停止当前渐变
        if (_activeFadeCoroutine != null)
        {
            StopCoroutine(_activeFadeCoroutine);
        }

        // 开始播放（带淡入效果）
        _activeFadeCoroutine = StartCoroutine(
            fade ? FadeMusic(musicData.clip, FADE_DURATION) : PlayMusicDirectly(musicData.clip)
        );
    }

    // 直接播放音乐
    private IEnumerator PlayMusicDirectly(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
        yield break;
    }

    // 淡入淡出背景音乐
    private IEnumerator FadeMusic(AudioClip newClip, float duration)
    {
        // 淡出当前音乐
        float fadeTime = 0;
        float startVolume = _musicSource.volume;

        while (fadeTime < duration && _musicSource.isPlaying)
        {
            fadeTime += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(startVolume, 0, fadeTime / duration);
            yield return null;
        }

        // 切换音乐并淡入
        _musicSource.clip = newClip;
        _musicSource.Play();
        _musicSource.volume = 0;

        fadeTime = 0;
        while (fadeTime < duration)
        {
            fadeTime += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(0, startVolume, fadeTime / duration);
            yield return null;
        }

        _musicSource.volume = startVolume; // 确保最终音量准确
    }

    // 停止背景音乐
    public void StopMusic(bool fade = true)
    {
        if (!_musicSource.isPlaying) return;

        if (_activeFadeCoroutine != null)
        {
            StopCoroutine(_activeFadeCoroutine);
        }

        _activeFadeCoroutine = StartCoroutine(
            fade ? FadeOutMusic(FADE_DURATION) : StopMusicDirectly()
        );
    }

    // 直接停止音乐
    private IEnumerator StopMusicDirectly()
    {
        _musicSource.Stop();
        yield break;
    }

    // 淡出音乐
    private IEnumerator FadeOutMusic(float duration)
    {
        float fadeTime = 0;
        float startVolume = _musicSource.volume;

        while (fadeTime < duration && _musicSource.isPlaying)
        {
            fadeTime += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(startVolume, 0, fadeTime / duration);
            yield return null;
        }

        _musicSource.Stop();
        _musicSource.volume = startVolume; // 恢复原始音量
    }
}