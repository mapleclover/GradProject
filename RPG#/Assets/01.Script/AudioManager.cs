using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    private AudioSource SFXsource;

    public float volume;
    public bool loop;

    public void SetSource(AudioSource _source)
    {
        SFXsource = _source;
        SFXsource.volume = 0.5f;
        SFXsource.clip = clip;
        SFXsource.loop = loop;
    }

    public void SetVolume()
    {
        SFXsource.volume = volume;
    }

    public void Play()
    {
        SFXsource.Play();
    }

    public void Stop()
    {
        SFXsource.Stop();
    }

    public void SetLoop()
    {
        SFXsource.loop = true;
    }

    public void SetLoopCancel()
    {
        SFXsource.loop = false;
    }
}

public class AudioManager : MonoBehaviour
{
    static public AudioManager instance;

    public Slider SFX;

    [SerializeField]
    public Sound[] sounds;

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
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("사운드 파일 이름 : " + i + " = " + sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }
    }

    public void TestPlay()
    {
        int i = Random.Range(0, 3);
        sounds[i].Play();
    }

    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if(_name == sounds[i].name)
            {
                sounds[i].Play();
                Debug.Log(_name);
                return;
            }
        }
    }

    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }

    public void SetLoopCancel(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoopCancel();
                return;
            }
        }
    }

    public void SetVolume()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].volume = SFX.value/10.0f;
            sounds[i].SetVolume();
        }
    }
}
