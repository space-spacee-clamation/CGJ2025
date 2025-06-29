using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Dictionary<string, AudioResourceDataPAre> AudioResources = new Dictionary<string, AudioResourceDataPAre>();
    public List<AudioSource> AudioSources = new List<AudioSource>();
    public AudioAssetSO audioAsset; // 拖拽分配ScriptableObject资源
    
    private AudioSource _musicSource;      // 专用的背景音乐源
    private Coroutine _activeFadeCoroutine; // 当前活动的渐变协程
    private const float FADE_DURATION = 1.5f; // 默认淡入淡出时间
    
    // 音效追踪机制
    private Dictionary<string, Coroutine> _activeCooldownTimers = new Dictionary<string, Coroutine>();
    private Dictionary<string, bool> _onCooldown = new Dictionary<string, bool>();
    private const float COOLDOWN_MULTIPLIER = 0.7f; // 音效冷却时间系数
    
    // 暂停状态管理
    private Dictionary<string, List<PausedSfxInfo>> _pausedSfx = new Dictionary<string, List<PausedSfxInfo>>();
    
    // 暂停信息结构体
    private struct PausedSfxInfo
    {
        public AudioSource Source;
        public float PlaybackTime;
    }

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

    // 播放单次音效（避免重复播放）
    public void PlayOnce(string clipName)
    {
        // 检查音效是否在冷却中
        if (IsOnCooldown(clipName))
        {
            Debug.Log($"Audio clip skipped: {clipName} is on cooldown");
            return;
        }

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
            // 查找播放时间最长的源（最可能结束）
            float longestTime = 0;
            for (int i = 1; i < AudioSources.Count; i++)
            {
                if (AudioSources[i].time > longestTime)
                {
                    availableSource = AudioSources[i];
                    longestTime = AudioSources[i].time;
                }
            }
        }

        if (availableSource == null)
        {
            Debug.LogWarning("No available audio sources for SFX!");
            return;
        }

        // 播放音效
        availableSource.clip = audioData.clip;
        availableSource.Play();
        
        // 启动冷却计时器
        StartCooldown(clipName, availableSource.clip.length);
    }

    // 检查音效是否在冷却中
    private bool IsOnCooldown(string clipName)
    {
        if (!AudioResources.ContainsKey(clipName)) return false;
        
        if (_onCooldown.TryGetValue(clipName, out bool isCooldown))
        {
            return isCooldown;
        }
        return false;
    }

    // 启动冷却计时器
    private void StartCooldown(string clipName, float clipLength)
    {
        // 如果已有计时器，停止它
        if (_activeCooldownTimers.TryGetValue(clipName, out Coroutine existingTimer))
        {
            if (existingTimer != null)
            {
                StopCoroutine(existingTimer);
            }
        }
        
        // 计算冷却时间 - 基于音效长度
        float cooldownTime = clipLength * COOLDOWN_MULTIPLIER;
        
        // 确保冷却时间至少为0.1秒
        if (cooldownTime < 0.1f) cooldownTime = 0.1f;
        
        // 设置冷却状态
        _onCooldown[clipName] = true;
        
        // 启动新的计时器
        _activeCooldownTimers[clipName] = StartCoroutine(CooldownTimer(clipName, cooldownTime));
    }

    // 冷却计时器协程
    private IEnumerator CooldownTimer(string clipName, float duration)
    {
        yield return new WaitForSeconds(duration);
        _onCooldown[clipName] = false;
        
        // 清理计时器
        if (_activeCooldownTimers.ContainsKey(clipName))
        {
            _activeCooldownTimers.Remove(clipName);
        }
    }
    
    // 暂停指定名字的音效
    public void PauseSfx(string clipName)
    {
        if (!AudioResources.ContainsKey(clipName))
        {
            Debug.LogWarning($"Tried to pause unknown sound effect: {clipName}");
            return;
        }
        
        // 清理旧的暂停记录
        if (_pausedSfx.ContainsKey(clipName))
        {
            _pausedSfx.Remove(clipName);
        }
        
        List<PausedSfxInfo> pausedInstances = new List<PausedSfxInfo>();
        
        // 遍历所有音效源（从1开始，0是背景音乐）
        for (int i = 1; i < AudioSources.Count; i++)
        {
            AudioSource source = AudioSources[i];
            
            // 检查是否正在播放指定音效
            if (source.isPlaying && source.clip == AudioResources[clipName].clip)
            {
                // 记录播放位置并暂停
                PausedSfxInfo info = new PausedSfxInfo
                {
                    Source = source,
                    PlaybackTime = source.time
                };
                
                pausedInstances.Add(info);
                source.Pause();
            }
        }
        
        // 如果有暂停的实例，添加到暂停字典
        if (pausedInstances.Count > 0)
        {
            _pausedSfx.Add(clipName, pausedInstances);
        }
    }
    
    // 恢复指定名字的音效
    public void ResumeSfx(string clipName)
    {
        if (!_pausedSfx.TryGetValue(clipName, out List<PausedSfxInfo> pausedInstances))
        {
            Debug.LogWarning($"No paused instances found for sound effect: {clipName}");
            return;
        }
        
        // 恢复所有暂停的实例
        foreach (PausedSfxInfo info in pausedInstances)
        {
            if (info.Source != null)
            {
                // 恢复播放位置并播放
                info.Source.time = info.PlaybackTime;
                info.Source.Play();
            }
        }
        
        // 从字典中移除
        _pausedSfx.Remove(clipName);
    }
    
    // 停止指定名字的音效
    public void StopSfx(string clipName)
    {
        if (!AudioResources.ContainsKey(clipName))
        {
            Debug.LogWarning($"Tried to stop unknown sound effect: {clipName}");
            return;
        }
        
        // 遍历所有音效源停止指定音效
        for (int i = 1; i < AudioSources.Count; i++)
        {
            AudioSource source = AudioSources[i];
            
            if (source.isPlaying && source.clip == AudioResources[clipName].clip)
            {
                source.Stop();
            }
        }
        
        // 如果这个音效被暂停过，清除暂停记录
        if (_pausedSfx.ContainsKey(clipName))
        {
            _pausedSfx.Remove(clipName);
        }
    }
    
    // 暂停所有音效
    public void PauseAllSfx()
    {
        // 先清除当前暂停记录
        _pausedSfx.Clear();
        
        // 按音效名分组暂停
        Dictionary<string, List<PausedSfxInfo>> newPauses = new Dictionary<string, List<PausedSfxInfo>>();
        
        for (int i = 1; i < AudioSources.Count; i++)
        {
            AudioSource source = AudioSources[i];
            
            if (source.isPlaying && source.clip != null)
            {
                // 查找音效名称
                string clipName = null;
                foreach (var kvp in AudioResources)
                {
                    if (kvp.Value.clip == source.clip)
                    {
                        clipName = kvp.Key;
                        break;
                    }
                }
                
                if (clipName != null)
                {
                    if (!newPauses.ContainsKey(clipName))
                    {
                        newPauses[clipName] = new List<PausedSfxInfo>();
                    }
                    
                    PausedSfxInfo info = new PausedSfxInfo
                    {
                        Source = source,
                        PlaybackTime = source.time
                    };
                    
                    newPauses[clipName].Add(info);
                    source.Pause();
                }
            }
        }
        
        _pausedSfx = newPauses;
    }
    
    // 恢复所有暂停的音效
    public void ResumeAllSfx()
    {
        List<string> clipNames = new List<string>(_pausedSfx.Keys);
        
        foreach (string clipName in clipNames)
        {
            ResumeSfx(clipName);
        }
    }
    
    // 停止所有音效
    public void StopAllSfx()
    {
        for (int i = 1; i < AudioSources.Count; i++)
        {
            AudioSource source = AudioSources[i];
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
        
        // 清除所有暂停记录
        _pausedSfx.Clear();
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
    
    // 清理冷却计时器
    private void OnDestroy()
    {
        StopAllCoroutines();
        foreach (var timer in _activeCooldownTimers.Values)
        {
            if (timer != null)
            {
                StopCoroutine(timer);
            }
        }
        _activeCooldownTimers.Clear();
        _onCooldown.Clear();
    }
}