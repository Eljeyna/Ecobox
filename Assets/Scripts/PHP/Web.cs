using UnityEngine;

public class Web : MonoBehaviour
{
    void Start()
    {
        //Debug.Log(StaticGameVariables.GetRequest("http://eztix/GetUsers.php"));
        Debug.Log(StaticGameVariables.UserLogin("__test__testunity@gmail.com", "121212"));
        Debug.Log(StaticGameVariables.UserRegister("TestUnity", "mironov20002000@gmail.com", "121212"));
    }
}
