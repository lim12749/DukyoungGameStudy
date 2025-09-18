// SoundManagerManual.cs
using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerManual : MonoBehaviour
{
    public enum Bus { Master, BGM, SFX, Enemy }
    public enum OverflowPolicy { DropNewest, StealOldest }

    public static SoundManagerManual Instance { get; private set; }

    [Header("Mixer & Groups (옵션)")]
    [SerializeField] AudioMixer mixer;
    [SerializeField] string masterVolParam = "MasterVol";
    [SerializeField] string bgmVolParam    = "BGMVol";
    [SerializeField] string sfxVolParam    = "SFXVol";
    [SerializeField] string enemyVolParam  = "EnemyVol";

    [Header("Audio Sources (직접 드래그)")]
    [SerializeField] AudioSource bgmSource;        // 2D, Output=BGM 그룹
    [SerializeField] AudioSource[] sfx2DSources;   // 2D, Output=SFX 그룹
    [SerializeField] AudioSource[] sfx3DSources;   // 3D, Output=SFX 그룹
    [SerializeField] AudioSource[] enemy3DSources; // 3D, Output=Enemy 그룹

    [Header("3D 기본 설정")]
    [SerializeField] float defaultMinDistance = 1f;
    [SerializeField] float defaultMaxDistance = 40f;

    [Header("동시재생 가득 찼을 때 정책")]
    [SerializeField] OverflowPolicy overflowPolicy = OverflowPolicy.StealOldest;

    int _idxSfx2D, _idxSfx3D, _idxEnemy3D;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 3D 소스 기본값 보정(인스펙터 수동 설정해도 무방)
        Apply3DDefaults(sfx3DSources);
        Apply3DDefaults(enemy3DSources);

        LoadVolumes();
    }

    void Apply3DDefaults(AudioSource[] arr)
    {
        if (arr == null) return;
        for (int i = 0; i < arr.Length; i++)
        {
            var a = arr[i];
            if (!a) continue;
            a.spatialBlend = 1f;
            a.rolloffMode = AudioRolloffMode.Logarithmic;
            if (a.minDistance <= 0f) a.minDistance = defaultMinDistance;
            if (a.maxDistance <= defaultMaxDistance) a.maxDistance = defaultMaxDistance;
        }
    }

    // ----------------- BGM -----------------
    public void PlayBGM(AudioClip clip, bool loop = true, float volume = 1f, float pitch = 1f)
    {
        if (!bgmSource || !clip) return;
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.volume = Mathf.Clamp01(volume);
        bgmSource.pitch = Mathf.Clamp(pitch, 0.1f, 3f);
        if (!bgmSource.isPlaying) bgmSource.Play();
        else { bgmSource.Stop(); bgmSource.Play(); }
    }
    public void StopBGM()
    {
        if (bgmSource) bgmSource.Stop();
    }

    // ----------------- SFX (2D) -----------------
    public void PlaySFX2D(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null || sfx2DSources == null || sfx2DSources.Length == 0) return;

        AudioSource src = GetFreeFromArray(sfx2DSources, ref _idxSfx2D);
        if (src == null) return; // DropNewest 정책일 때 모두 바쁘면 드롭

        ConfigureAndPlay(src, clip, volume, pitch, Vector3.zero, is3D:false);
    }

    // ----------------- SFX (3D, 월드 위치) -----------------
    public void PlaySFXAt(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        if (clip == null || sfx3DSources == null || sfx3DSources.Length == 0) return;

        AudioSource src = GetFreeFromArray(sfx3DSources, ref _idxSfx3D);
        if (src == null) return;

        ConfigureAndPlay(src, clip, volume, pitch, position, is3D:true);
    }

    // ----------------- Enemy 사운드 (3D) -----------------
    public void PlayEnemyAt(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        if (clip == null || enemy3DSources == null || enemy3DSources.Length == 0) return;

        AudioSource src = GetFreeFromArray(enemy3DSources, ref _idxEnemy3D);
        if (src == null) return;

        ConfigureAndPlay(src, clip, volume, pitch, position, is3D:true);
    }

    // ----------------- Mixer 볼륨 (0~1) -----------------
    public void SetVolume01(Bus bus, float value01, bool save = true)
    {
        if (!mixer) return;
        string p = GetParam(bus);
        float dB = ToDecibel(value01);
        mixer.SetFloat(p, dB);
        if (save) PlayerPrefs.SetFloat(GetKey(bus), Mathf.Clamp01(value01));
    }

    public float GetVolume01(Bus bus)
    {
        if (PlayerPrefs.HasKey(GetKey(bus)))
            return PlayerPrefs.GetFloat(GetKey(bus), 1f);

        if (mixer && mixer.GetFloat(GetParam(bus), out float dB))
            return FromDecibel(dB);

        return 1f;
    }

    public void LoadVolumes()
    {
        SetVolume01(Bus.Master, PlayerPrefs.GetFloat(GetKey(Bus.Master), 0.9f), false);
        SetVolume01(Bus.BGM,    PlayerPrefs.GetFloat(GetKey(Bus.BGM),    0.8f), false);
        SetVolume01(Bus.SFX,    PlayerPrefs.GetFloat(GetKey(Bus.SFX),    1.0f), false);
        SetVolume01(Bus.Enemy,  PlayerPrefs.GetFloat(GetKey(Bus.Enemy),  1.0f), false);
    }

    // ----------------- 내부 유틸 -----------------
    AudioSource GetFreeFromArray(AudioSource[] arr, ref int roundIndex)
    {
        int len = arr.Length;
        int i;

        // 1) 재생 중이 아닌 소스 먼저 찾기
        for (i = 0; i < len; i++)
        {
            if (arr[i] != null && !arr[i].isPlaying)
                return arr[i];
        }

        // 2) 모두 바쁘면 정책에 따름
        if (overflowPolicy == OverflowPolicy.DropNewest)
        {
            return null; // 새 소리 드롭
        }

        // 3) StealOldest: 라운드로빈으로 하나 뺏기
        if (len > 0)
        {
            roundIndex = (roundIndex + 1) % len;
            var src = arr[roundIndex];
            if (src != null) src.Stop(); // 기존 소리 중지 후 재사용
            return src;
        }
        return null;
    }

    void ConfigureAndPlay(AudioSource src, AudioClip clip, float volume, float pitch, Vector3 pos, bool is3D)
    {
        if (src == null) return;
        if (is3D) src.transform.position = pos;
        src.volume = Mathf.Clamp01(volume);
        src.pitch  = Mathf.Clamp(pitch, 0.1f, 3f);
        // PlayOneShot을 쓰면 같은 소스에 중첩 재생이 가능하지만,
        // 위치를 바꾸는 3D 사운드에선 이전 사운드 위치가 따라 움직일 수 있어 중지 후 Play를 사용.
        src.clip = clip;
        src.Play();
    }

    string GetParam(Bus bus)
    {
        switch (bus)
        {
            case Bus.BGM:   return bgmVolParam;
            case Bus.SFX:   return sfxVolParam;
            case Bus.Enemy: return enemyVolParam;
            default:        return masterVolParam;
        }
    }

    string GetKey(Bus bus) { return "vol." + bus; }

    static float ToDecibel(float v01) { return (v01 <= 0.0001f) ? -80f : Mathf.Log10(Mathf.Clamp01(v01)) * 20f; }
    static float FromDecibel(float dB) { return Mathf.Clamp01(Mathf.Pow(10f, dB / 20f)); }
}
