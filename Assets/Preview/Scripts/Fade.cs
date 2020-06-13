using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    private Animator fade;
    public float time = 4f;

    private void Start()
    {
        fade = gameObject.GetComponent<Animator>();
        StartCoroutine(End());
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Escape))
        {
            StopCoroutine(End());
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        }
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(time);
        fade.SetTrigger("Start");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
