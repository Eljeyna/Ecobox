using System.Collections;
using UnityEngine;

public class StartSound : MonoBehaviour
{
    public AudioDirector audioDirector;
    public string sound;
    public float time = 0.25f;

    private void Start()
    {
        StartCoroutine(MusicPlay());
    }

    IEnumerator MusicPlay()
    {
        yield return new WaitForSeconds(time);
        audioDirector.Play(sound);
    }
}
