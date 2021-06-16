public class DialogueChangeMusic : DialogueScript
{
    public MusicList music;

    public override void Use()
    {
        MusicDirector.Instance.ChangeMusic((int)music);
    }
}
