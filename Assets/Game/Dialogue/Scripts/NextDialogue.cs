using System.Collections;
using UnityEngine;

public class NextDialogue : MonoBehaviour
{
    public GameDirector game;
    public GameObject next;
    public float time = 1.0f;
    private void Start()
    {
        StartCoroutine(Next());
    }

    IEnumerator Next()
    {
        yield return new WaitForSeconds(time);
        game.SetSpeaker(next, null);
        game.StartDialogue();
    }
}
