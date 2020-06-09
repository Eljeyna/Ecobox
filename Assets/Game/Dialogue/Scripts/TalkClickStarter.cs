using UnityEngine;

public class TalkClickStarter : MonoBehaviour
{
    public GameDirector game;

    private void OnMouseDown()
    {
        game.joystick.enabled = false;
    }
    private void OnMouseUp()
    {
        if (game.canControl)
        {
            game.joystick.enabled = true;
            game.SetSpeaker(gameObject.transform.parent.gameObject, gameObject);
            game.StartDialogue();
        }
    }
}
