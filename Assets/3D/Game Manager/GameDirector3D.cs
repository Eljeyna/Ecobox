using UnityEngine;

public class GameDirector3D : MonoBehaviour
{
    public static int maxWave = 8;
    public static int maxEntities = 40;

    private static GameObject player;
    private static AudioDirector audioDirector;
    private static GameObject spawner;

    public static Transform GetPlayer()
    {
        if (player == null)
            player = GameObject.Find("Synthea");

        return player.transform;
    }

    public static AudioDirector GetAudioDirector()
    {
        if (audioDirector == null)
        {
            GameObject audio = GameObject.Find("Audio Director");
            audioDirector = audio.GetComponent<AudioDirector>();
        }

        return audioDirector;
    }

    public static int PlayRandomSound(AudioDirector sounds, string[] soundsList)
    {
        if (soundsList.Length > 0)
        {
            if (soundsList.Length == 1)
            {
                sounds.Play(soundsList[0]);
                return 0;
            }

            int randomSound = Random.Range(0, soundsList.Length);
            sounds.Play(soundsList[randomSound]);
            return randomSound;
        }

        return -1;
    }

    public static void StartGame()
    {
        spawner = GameObject.Find("Spawner");
        spawner.GetComponent<Spawner3D>().spawn = true;
    }

    public static void VictoryGame()
    {
        spawner.GetComponent<Spawner3D>().spawn = false;
        spawner = GameObject.Find("End");
        spawner.GetComponent<Animator>().Play("VictoryDefeat");
    }

    public static void DefeatGame()
    {
        spawner.GetComponent<Spawner3D>().spawn = false;
        spawner = GameObject.Find("End");
        spawner.GetComponent<Animator>().Play("VictoryDefeat");
    }
}
