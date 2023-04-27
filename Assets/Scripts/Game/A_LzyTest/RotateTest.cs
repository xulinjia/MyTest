using CenterMsg;
using ErisGame;
using TMPro;
using UnityEngine;
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

        btn_A = transform.Find("btn_A").GetComponent<Button>();
        btn_A.onClick.AddListener(OnABtn);
        btn_B = transform.Find("btn_B").GetComponent<Button>();
        btn_B.onClick.AddListener(OnBBtn);
    }
    private void OnABtn()
    {
    }
    private void OnBBtn()
    {

    }
    private void OnTestBtn()
    {
        rotateFlag = !rotateFlag;
    }
    private async void onTestSendBtn()
    {
        string sendMsg = sengText.text;
        Managers.GetNetManager().SendMessageAsync(NetMsgID.C2G_Login, new C2G_Login() {AccountId = "1006", PassWd = "password"});
    }
    private void onTestLoginBtn()
    {
        var res = Managers.GetNetManager().TryConnectAsync("",0);
    }
    private void Update()
    {
        if (rotateFlag)
        {
            Cube_Test.transform.Rotate(rotateSpeed, -rotateSpeed/2, rotateSpeed/3);
        }
    }

    private Button btn_A;
    private Button btn_B;
    private Button btn_test;
    private Button btn_switchScene;
    private Button btn_testLogin;
    private TMP_Text sengText;
    private bool rotateFlag = false;
    private int rotateSpeed = 2;
}
