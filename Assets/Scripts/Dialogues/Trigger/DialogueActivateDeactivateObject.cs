using UnityEngine;

public class DialogueActivateDeactivateObject : DialogueScript
{
    public string rootName;
    public string nameObject;
    public bool activate;

    public override void Use()
    {
        GameObject rootObject = GameObject.Find(rootName);

        if (rootObject)
        {
            GameObject objectToActivateDeactivate = rootObject.transform.Find(nameObject).gameObject;

            if (objectToActivateDeactivate)
            {
                objectToActivateDeactivate.SetActive(activate);
            }
        }
    }
}
