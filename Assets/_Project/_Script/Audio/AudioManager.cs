using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum SoundID
{
    Title_BG = 0,
    Home_BG = 1,
    Gameplay_BG = 2,
    Battle_BG = 3,
}

[Serializable]
public enum FxID
{
    Button = 0,
    Level_Load = 1,
    Level_Select = 2,
    Tile_Select = 3,
    Tile_Connect = 4,
    Game_Win = 5,
    Game_Lose = 6,
    Time_Warning = 7,
}


public class SoundManager : Singleton<SoundManager>
{
    private AudioSource soundSource;
    private AudioSource[] fxSource = new AudioSource[Enum.GetNames(typeof(FxID)).Length];

    [SerializeField] private AudioClip[] soundAus;
    [SerializeField] private AudioClip[] fxAus;

    private bool isLoaded = false;
    private int indexSound;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);

        soundSource = gameObject.AddComponent<AudioSource>();
        soundSource.loop = true;
    }

    private void Start()
    {
        Invoke(nameof(OnLoad), 0.1f);
    }

    private void OnLoad()
    {
        soundSource.volume = SaveSystem.userData.settingConfig.BackgroundVolume;
        if (soundAus.Length > 0)
        {
            isLoaded = true;
            PlaySound(SoundID.Title_BG);
        }
    }

    public void UpdateSoundVolume()
    {
        soundSource.volume = SaveSystem.userData.settingConfig.BackgroundVolume;
    }

    private void PlaySound(SoundID ID)
    {
        soundSource.clip = soundAus[(int)ID];
        soundSource.Play();
    }

    public void PlayFx(FxID ID)
    {
        if (SaveSystem.userData.settingConfig.FxVolume > 0 && isLoaded)
        {
            if (fxSource[(int)ID] == null)
            {
                fxSource[(int)ID] = new GameObject().AddComponent<AudioSource>();
                fxSource[(int)ID].clip = fxAus[(int)ID];
                fxSource[(int)ID].loop = false;
                fxSource[(int)ID].transform.SetParent(transform);
            }
            fxSource[(int)ID].volume = SaveSystem.userData.settingConfig.FxVolume;
            fxSource[(int)ID].PlayOneShot(fxAus[(int)ID]);

            //Debug.Log(ID);
        }
    }

    public void ChangeSound(SoundID ID, float time)
    {
        soundSource.Play();
        StartCoroutine(ChangeSoundCoroutine(ID, time));
    }

    private IEnumerator ChangeSoundCoroutine(SoundID ID, float time)
    {
        float startVolume = SaveSystem.userData.settingConfig.BackgroundVolume;
        float elapsed = 0f;

        // Fade out
        while (elapsed < time)
        {
            soundSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

        soundSource.volume = 0f;
        PlaySound(ID);

        // Fade in
        elapsed = 0f;
        while (elapsed < time)
        {
            soundSource.volume = Mathf.Lerp(0f, startVolume, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

        soundSource.volume = startVolume;
    }

    public void StopSound()
    {
        StartCoroutine(StopSoundCoroutine());
    }

    private IEnumerator StopSoundCoroutine(float duration = 1f)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            soundSource.volume = Mathf.Lerp(soundSource.volume, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        soundSource.Stop();
    }
    

}