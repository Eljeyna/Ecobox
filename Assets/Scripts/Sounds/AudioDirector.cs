using UnityEngine;

public class AudioDirector : MonoBehaviour
{
    public AudioClip[] sounds;

    [HideInInspector] public AudioSource[] sources;

    private void Awake()
    {
        sources = new AudioSource[sounds.Length];
        for (int i = 0; i < sounds.Length; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].clip = sounds[i];
        }
    }

    private void Start()
    {
        foreach (AudioSource sound in sources)
        {
            sound.volume = 1f;
        }
    }

    public void Play(int index)
    {
        sources[index].Play();
    }

    public void Stop(int index)
    {
        sources[index].Stop();
    }
}
