using UnityEngine;
using UnityEngine.Audio;

public enum MusicList
{
    MainMenu    = 0,
    Briefing    = 1,
    Tutorial01  = 2,
    Dungeon     = 3
}

public class MusicDirector : MonoBehaviour
{
    public static MusicDirector Instance { get; private set; }
    public AudioDirector audioDirector;

    public AudioMixerGroup soundsGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup guiGroup;

    private int prevMusic = -1;
    private int nextMusic = -1;

    private float nextMusicTime;

    private void Awake()
    {
        if (ReferenceEquals(Instance, null))
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        for (int i = 0; i < audioDirector.sources.Length; i++)
        {
            audioDirector.sources[i].outputAudioMixerGroup = musicGroup;
            audioDirector.sources[i].loop = true;
        }
    }

    private void Update()
    {
        if (nextMusicTime > Time.unscaledTime)
        {
            if (prevMusic >= 0)
            {
                audioDirector.sources[prevMusic].volume = nextMusicTime - Time.unscaledTime;
            }

            return;
        }

        if (prevMusic >= 0)
        {
            audioDirector.Stop(prevMusic);
            audioDirector.sources[prevMusic].volume = 1f;
        }

        if (nextMusic >= 0)
        {
            PlayMusic(nextMusic);
        }

        prevMusic = nextMusic;

        this.enabled = false;
    }

    public void PlayMusic(int music)
    {
        if (prevMusic != -1 && nextMusic != -1 && prevMusic == music)
        {
            return;
        }

        nextMusic = music;
        audioDirector.Play(music);
    }

    public void ChangeMusic(int music)
    {
        if (prevMusic != -1 && nextMusic != -1 && prevMusic == music)
        {
            return;
        }

        nextMusic = music;
        nextMusicTime = Time.unscaledTime + 1f;
        this.enabled = true;
    }

    public void StopMusic()
    {
        nextMusic = -1;
        nextMusicTime = Time.unscaledTime + 1f;
        this.enabled = true;
    }
}
