using System.Collections;
using UnityEngine;

public class Synthea01After : MonoBehaviour
{
    public Animator fade;
    public AudioDirector audioDirector;
    public float time = 1f;

    private void Start()
    {
        fade.enabled = true;
        StartCoroutine(EndOfDialogue());
    }

    IEnumerator EndOfDialogue()
    {
        int random = Random.Range(0, 2);
        yield return new WaitForSeconds(time);
        fade.gameObject.SetActive(false);
        /*if (random == 0)
        {
            audioDirector.Play("City1");
        }
        else
        {
            audioDirector.Play("City2");
        }*/
    }
}
