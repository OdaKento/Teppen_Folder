using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource seAudioSource;

    //BGMÇÃâπó ê›íË
    public float BgmVolume
    {
        get
        {
            return bgmAudioSource.volume;
        }
        set
        {
            bgmAudioSource.volume = Mathf.Clamp01(value);
        }
    }

    //SEÇÃâπó ê›íË
    public float SeVolume
    {
        get
        {
            return seAudioSource.volume;
        }
        set
        {
            seAudioSource.volume = Mathf.Clamp01(value);
        }
    }

    void Start()
    {
        GameObject soundManager = CheckOtherSoundManager();
        bool checkResult = soundManager != null && soundManager != gameObject;

        if (checkResult)
        {
            //Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundManager");
    }

    //BGMÇ™Ç†ÇÍÇŒçƒê∂
    public void PlayBgm(AudioClip clip)
    {
        bgmAudioSource.clip = clip;

        if (clip == null)
        {
            return;
        }

        bgmAudioSource.Play();
    }

    //SEÇ™Ç†ÇÍÇŒàÍìxçƒê∂
    public void PlaySe(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        seAudioSource.PlayOneShot(clip);
    }

    //BGMàÍéûí‚é~
    public void bgmPause()
    {
        bgmAudioSource.Pause();
    }

    //SEàÍéûí‚é~
    public void sePause()
    {
        seAudioSource.Pause();
    }

    //BGMí‚é~
    public void bgmStop()
    {
        bgmAudioSource.Stop();
    }

    //SEí‚é~
    public void seStop()
    {
        seAudioSource.Stop();
    }
}