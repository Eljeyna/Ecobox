using UnityEngine;

public class TalkClickStarter : MonoBehaviour
{
    public GameDirector game;

    private void OnMouseUp()
    {
        if (game.canControl)
        {
            game.SetSpeaker(gameObject.transform.parent.gameObject, gameObject);
            game.StartDialogue();
        }
    }
}
