using System;
using System.Collections;
using UnityEngine;

public class QuestAnimation : MonoBehaviour
{
    private GameObject objectAnim;
    private float startPos;
    private float curPos;
    private float newPos;
    public float needPos;
    public float speed;

    private void Start()
    {
        objectAnim = gameObject;
        startPos = objectAnim.transform.position.y;
        curPos = startPos;
        newPos = startPos - needPos;
        StartCoroutine(AnimationQuest());
    }

    IEnumerator AnimationQuest()
    {
        while (true)
        {
            while (curPos > newPos)
            {
                curPos -= Time.deltaTime / speed;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, curPos, gameObject.transform.position.z);

                if (curPos <= newPos)
                {
                    curPos = newPos;
                }
                yield return null;
            }
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, curPos, gameObject.transform.position.z);

            while (curPos < startPos)
            {
                curPos += Time.deltaTime / speed;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, curPos, gameObject.transform.position.z);

                if (curPos >= startPos)
                {
                    curPos = startPos;
                }
                yield return null;
            }
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, curPos, gameObject.transform.position.z);
        }
    }
}
