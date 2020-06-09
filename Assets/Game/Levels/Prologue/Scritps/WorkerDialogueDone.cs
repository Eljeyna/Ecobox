using System.Collections;
using UnityEngine;

public class WorkerDialogueDone : MonoBehaviour
{
    private Fadeout fade;
    private SpriteRenderer rend;
    public GameObject talk;
    public GameObject talkName;
    public GameObject masterDoor;
    public float animSpeed;

    public void Start()
    {
        talk.SetActive(false);
        talkName.SetActive(false);
        fade = GetComponent<Fadeout>();
        rend = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(fade.Fade(rend, animSpeed));
        StartCoroutine(Done());
    }

    IEnumerator Done()
    {
        yield return new WaitForSeconds(animSpeed);
        masterDoor.SetActive(true);
        Destroy(gameObject);
    }
}
