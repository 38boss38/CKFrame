using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioManager : ManagerBase<AudioManager>
{
    [SerializeField] 
    private AudioSource BGAudioSource;
    [SerializeField]
    private GameObject prefab_AudioPlay;
    
    /// <summary>
    /// 场景中生效的所有特效音乐播放器
    /// </summary>
    private List<AudioSource> audioPlayList = new List<AudioSource>();

    #region 音量、播放控制

    [SerializeField] [Range(0, 1)] [OnValueChanged("UpdataAllAudioPlay")]
    private float globalVolume;

    public float GlobalVolume
    {
        get => globalVolume;
        set
        {
            if (globalVolume == value) return;
            globalVolume = value;
            UpdataAllAudioPlay();
        }
    }

    [SerializeField] [Range(0, 1)] [OnValueChanged("UpdataBGAudioPlay")]
    private float bgVolume;

    public float BGVolume
    {
        get => bgVolume;
        set
        {
            if (bgVolume == value) return;
            bgVolume = value;
            UpdataBGAudioPlay();
        }
    }

    [SerializeField] [Range(0, 1)] [OnValueChanged("UpdataEffectAudioPlay")]
    private float effectVolume;

    public float EffectVolume
    {
        get => effectVolume;
        set
        {
            if (effectVolume == value) return;
            effectVolume = value;
            UpdataEffectAudioPlay();
        }
    }
    
    [SerializeField]
    [OnValueChanged("UpdataMute")]
    private bool isMute = false;
    public bool IsMute
    {
        get => isMute;
        set
        {
            if (isMute == value) return;
            isMute = value;
            UpdataMute();
        }
    }
    
    [SerializeField]
    [OnValueChanged("UpdataLoop")]
    private bool isLoop = true;
    public bool IsLoop
    {
        get => isLoop;
        set
        {
            if (isLoop == value) return;
            isLoop = value;
            UpdataLoop();
        }
    }
    
    private bool isPause = false;

    public bool IsPause
    {
        get => isPause;
        set
        {
            if (isPause == value) return;
            isPause = value;
            if (isPause)
            {
                BGAudioSource.Pause();
            }
            else
            {
                BGAudioSource.UnPause();
            }
            UpdataEffectAudioPlay();
        }
    }

    /// <summary>
    /// 更新全部播放器类型
    /// </summary>
    private void UpdataAllAudioPlay()
    {
        UpdataBGAudioPlay();
        UpdataEffectAudioPlay();
    }

    /// <summary>
    /// 更新背景音乐
    /// </summary>
    private void UpdataBGAudioPlay()
    {
        BGAudioSource.volume = bgVolume * globalVolume;
    }

    /// <summary>
    /// 更新特效音乐
    /// </summary>
    private void UpdataEffectAudioPlay()
    {
        // 倒序遍历
        for (int i = audioPlayList.Count - 1; i >= 0; i--)
        {
            if (audioPlayList[i] != null)
            {
                SetEffectAudioPlay(audioPlayList[i]);
            }
            else
            {
                audioPlayList.RemoveAt(i);
            }
            
        }
    }
    
    /// <summary>
    /// 设置特效音乐播放器
    /// </summary>
    private void SetEffectAudioPlay(AudioSource audioPlay,float spatial = -1)
    {
        audioPlay.mute = isMute;
        audioPlay.volume = effectVolume * globalVolume;
        if (spatial != -1)
        {
            audioPlay.spatialBlend = spatial;
        }

        if (isPause)
        {
            audioPlay.Pause();
        }
        else
        {
            audioPlay.UnPause();
        }
    }
    
    /// <summary>
    /// 更新背景音乐静音情况
    /// </summary>
    private void UpdataMute()
    {
        BGAudioSource.mute = isMute;
        UpdataEffectAudioPlay();
    }
    
    /// <summary>
    /// 更新背景音乐循环
    /// </summary>
    private void UpdataLoop()
    {
        BGAudioSource.loop = isLoop;
    }

    #endregion
    public override void Init()
    {
        base.Init();
        UpdataAllAudioPlay();
    }
    #region 背景音乐

    public void PlayBGAudio(AudioClip clip,bool loop = true,float volume = -1)
    {
        BGAudioSource.clip = clip;
        IsLoop = loop;
        if (volume != -1)
        {
            BGVolume = volume;
        }
        BGAudioSource.Play();
    }

    public void PlayBGAudio(string clipPath, bool loop = true,float volume = -1)
    {
        AudioClip clip = ResManager.Instance.LoadAsset<AudioClip>(clipPath);
        PlayBGAudio(clip, loop, volume);
    }
    #endregion

    #region 特效音乐
    
    private Transform audioPlayRoot = null;
    
    /// <summary>
    /// 获取音乐播放器
    /// </summary>
    /// <returns></returns>
    private AudioSource GetAudioPlay(bool is3D = true)
    {
        if (audioPlayRoot == null)
        {
            audioPlayRoot = new GameObject("AudioPlayRoot").transform;
        }
        // 从对象池中获取播放器
        AudioSource audioSource = PoolManager.Instance.GetGameObject<AudioSource>(prefab_AudioPlay,audioPlayRoot);
        SetEffectAudioPlay(audioSource,is3D?1f:0f);
        audioPlayList.Add(audioSource);
        return audioSource;
    }

    /// <summary>
    /// 回收播放器
    /// </summary>
    private void RecycleAudioPlay(AudioSource audioSource, AudioClip clip, UnityAction callback, float time)
    {
        StartCoroutine(DoRecycleAudioPlay(audioSource, clip, callback, time));
    }

    private IEnumerator DoRecycleAudioPlay(AudioSource audioSource, AudioClip clip, UnityAction callback, float time)
    {
        // 延迟Clip的长度（秒）
        yield return new WaitForSeconds(clip.length);
        // 放回池子
        if (audioSource != null)
        {
            audioSource.CKGameObjectPushPool();
            // 回调 延迟time（秒）时间
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }
    }
    
    /// <summary>
    /// 播放一次特效音乐
    /// </summary>
    /// <param name="clip">音效片段</param>
    /// <param name="component">挂载组件</param>
    /// <param name="volumeScale">音量 0-1</param>
    /// <param name="is3d">是否3D</param>
    /// <param name="callback">回调函数-在音乐播放完成后执行</param>
    /// <param name="callBackTime">回调函数-在音乐播放完成后执行的延迟时间</param>
    public void PlayOnShot(AudioClip clip,Component component,float volumeScale = 1, bool is3d = true,UnityAction callback = null,float callBackTime = 0)
    {
        // 初始化音乐播放器
        AudioSource audioSource = GetAudioPlay(is3d);
        audioSource.transform.SetParent(component.transform);
        audioSource.transform.localPosition = Vector3.zero;
        
        // 播放一次音效
        audioSource.PlayOneShot(clip, volumeScale);
        
        //播放器回收以及回调函数
        RecycleAudioPlay(audioSource, clip, callback, callBackTime);
    }
    
    /// <summary>
    /// 播放一次特效音乐
    /// </summary>
    /// <param name="clip">音效片段</param>
    /// <param name="postion">播放的位置</param>
    /// <param name="volumeScale">音量 0-1</param>
    /// <param name="is3d">是否3D</param>
    /// <param name="callback">回调函数-在音乐播放完成后执行</param>
    /// <param name="callBackTime">回调函数-在音乐播放完成后执行的延迟时间</param>
    public void PlayOnShot(AudioClip clip,Vector3 postion,float volumeScale = 1, bool is3d = true,UnityAction callback = null,float callBackTime = 0)
    {
        // 初始化音乐播放器
        AudioSource audioSource = GetAudioPlay(is3d);
        audioSource.transform.position = postion;
        
        // 播放一次音效
        audioSource.PlayOneShot(clip, volumeScale);
        //播放器回收以及回调函数
        RecycleAudioPlay(audioSource, clip, callback, callBackTime);
    }
    
    /// <summary>
    /// 播放一次特效音乐
    /// </summary>
    /// <param name="clipPath">音效片段路径</param>
    /// <param name="component">挂载组件</param>
    /// <param name="volumeScale">音量 0-1</param>
    /// <param name="is3d">是否3D</param>
    /// <param name="callback">回调函数-在音乐播放完成后执行</param>
    /// <param name="callBackTime">回调函数-在音乐播放完成后执行的延迟时间</param>
    public void PlayOnShot(string clipPath,Component component,float volumeScale = 1, bool is3d = true,UnityAction callback = null,float callBackTime = 0)
    {
        AudioClip audioClip = ResManager.Instance.LoadAsset<AudioClip>(clipPath);
        if (audioClip != null) PlayOnShot(audioClip,component,volumeScale,is3d,callback,callBackTime);
    }
    
    /// <summary>
    /// 播放一次特效音乐
    /// </summary>
    /// <param name="clipPath">音效片段路径</param>
    /// <param name="postion">播放的位置</param>
    /// <param name="volumeScale">音量 0-1</param>
    /// <param name="is3d">是否3D</param>
    /// <param name="callback">回调函数-在音乐播放完成后执行</param>
    /// <param name="callBackTime">回调函数-在音乐播放完成后执行的延迟时间</param>
    public void PlayOnShot(string clipPath,Vector3 postion,float volumeScale = 1, bool is3d = true,UnityAction callback = null,float callBackTime = 0)
    {
        AudioClip audioClip = ResManager.Instance.LoadAsset<AudioClip>(clipPath);
        if (audioClip != null) PlayOnShot(audioClip,postion,volumeScale,is3d,callback,callBackTime);
    }
    
    #endregion
}