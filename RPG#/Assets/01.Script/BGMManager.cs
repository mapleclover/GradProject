using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;

    public AudioClip[] clips;

    public Slider BGM;

    private float SetVolume;

    private AudioSource BGMsource;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    void Start()
    {
        BGMsource = GetComponent<AudioSource>();
    }

    public void Play(int _playMusicTrack)
    {
        BGMsource.clip = clips[_playMusicTrack];
        BGMsource.Play();
    }

    public void Pause()
    {
        BGMsource.Pause();
    }

    public void UnPause()
    {
        BGMsource.UnPause();
    }
    
    public void Stop()
    {
        BGMsource.Stop();
    }

    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    public void FadeIntMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusicCoroutine());
    }

    public void changeVolume()
    {
        BGMsource.volume = BGM.value/10.0f;
        this.SetVolume = BGMsource.volume;
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        for(float i = SetVolume; i >= 0f; i -= SetVolume * 0.01f)
        {
            BGMsource.volume = i;
            yield return waitTime;
        }
    }

    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0f; i <= SetVolume; i += SetVolume * 0.01f)
        {
            BGMsource.volume = i;
            yield return waitTime;
        }
    }
}
