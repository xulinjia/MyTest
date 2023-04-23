using ErisGame;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RotateTest : MonoBehaviour
{
    public GameObject Cube_Test;
    private void Awake()
    {
        sengText = transform.Find("sengText").GetComponent<TMP_Text>();
        btn_test = transform.Find("btn_test").GetComponent<Button>();
        btn_test.onClick.AddListener(OnTestBtn);
        btn_switchScene = transform.Find("btn_testSend").GetComponent<Button>();
        btn_switchScene.onClick.AddListener(onTestSendBtn);
        btn_testLogin = transform.Find("btn_testLogin").GetComponent<Button>();
        btn_testLogin.onClick.AddListener(onTestLoginBtn);
    }

    private void OnTestBtn()
    {
        rotateFlag = !rotateFlag;
    }
    private async void onTestSendBtn()
    {
        string sendMsg = sengText.text;
        await Managers.GetNetManager().SendMessage(sendMsg);
    }
    private void onTestLoginBtn()
    {
        Managers.GetNetManager().TryConnect("",0);
    }
    private void Update()
    {
        if (rotateFlag)
        {
            Cube_Test.transform.Rotate(rotateSpeed, -rotateSpeed/2, rotateSpeed/3);
        }
    }

    private Button btn_test;
    private Button btn_switchScene;
    private Button btn_testLogin;
    private TMP_Text sengText;
    private bool rotateFlag = false;
    private int rotateSpeed = 2;
}
