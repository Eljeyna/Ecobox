using System.Threading.Tasks;
using UnityEngine;

public class StartSound : MonoBehaviour
{
    public AudioDirector audioDirector;
    public int sound;
    public int timeMilliseconds = 2500;

    private async Task Start()
    {
        await MusicPlay();
    }

    private async Task MusicPlay()
    {
        await Task.Delay(timeMilliseconds);
        audioDirector.Play(sound);
    }
}
