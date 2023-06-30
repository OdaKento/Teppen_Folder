using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource seAudioSource;

    //BGM�̉��ʐݒ�
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

    //SE�̉��ʐݒ�
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

    //BGM������΍Đ�
    public void PlayBgm(AudioClip clip)
    {
        bgmAudioSource.clip = clip;

        if (clip == null)
        {
            return;
        }

        bgmAudioSource.Play();
    }

    //SE������Έ�x�Đ�
    public void PlaySe(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        seAudioSource.PlayOneShot(clip);
    }

    //BGM�ꎞ��~
    public void bgmPause()
    {
        bgmAudioSource.Pause();
    }

    //SE�ꎞ��~
    public void sePause()
    {
        seAudioSource.Pause();
    }

    //BGM��~
    public void bgmStop()
    {
        bgmAudioSource.Stop();
    }

    //SE��~
    public void seStop()
    {
        seAudioSource.Stop();
    }
}