using System.IO;
using System.Text;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    public void Use()
    {
        StringBuilder sb = new StringBuilder(Path.Combine(StaticGameVariables._SAVE_FOLDER, "save0.json"));

        if (File.Exists(sb.ToString()))
        {
            Settings.Instance.gameIsLoaded = true;
        }
    }
}
