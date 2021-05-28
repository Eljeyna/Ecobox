using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    public SwitchScene switchScene;
    
    public void Use()
    {
        StringBuilder sb = new StringBuilder(Path.Combine(Game._SAVE_FOLDER, "save0.json"));

        if (File.Exists(sb.ToString()))
        {
            Settings.Instance.gameIsLoaded = true;
            
            Saveable saveObject = JsonConvert.DeserializeObject<Saveable>(File.ReadAllText(sb.ToString()));
            
            switchScene.level = saveObject.scene;
            switchScene.Use();
        }
    }
}
