using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoOver : MonoBehaviour
{
    private StartSound sound;
    public GameDirector game;
    public Animator fade;

    private void Start()
    {
        sound = gameObject.GetComponent<StartSound>();
    }

    private void OnMouseUp()
    {
        game.canControl = false;
        fade.gameObject.SetActive(true);
        sound.enabled = true;
        fade.SetTrigger("Start");
        StartCoroutine(End());
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
