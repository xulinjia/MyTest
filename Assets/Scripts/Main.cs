using ErisGame;
using UnityEngine;

public class Main : MonoBehaviour
{
    [System.ComponentModel.DefaultValue(false)]
    public static bool Active { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        if (!enabled || !gameObject.activeSelf) return;

        DontDestroyOnLoad(gameObject);
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Active = true;

        GameClient.InitGame(null);
    }

    // Update is called once per frame
    void Update()
    {
        GameClient.Update();
    }

    void LateUpdate()
    {
        GameClient.LateUpdate();
    }

    private void OnApplicationQuit()
    {
        if (!enabled || !gameObject.activeSelf) return;
        GameClient.Destroy();
        Active = false;
    }
}
