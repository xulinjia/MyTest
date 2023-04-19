using ErisGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RotateTest : MonoBehaviour
{
    public GameObject Cube_Test;
    private void Awake()
    {
        btn_test = transform.Find("btn_test").GetComponent<Button>();
        btn_test.onClick.AddListener(OnRotateBtn);
        btn_switchScene = transform.Find("btn_switchScene").GetComponent<Button>();
        btn_switchScene.onClick.AddListener(onSwitchSceneBtn);
        btn_testLogin = transform.Find("btn_testLogin").GetComponent<Button>();
        btn_testLogin.onClick.AddListener(onTestLoginBtn);
    }

    private void OnRotateBtn()
    {
        rotateFlag = !rotateFlag;
    }
    private void onSwitchSceneBtn()
    {
        SceneManager.LoadScene("SceneTest");
    }
    private void onTestLoginBtn()
    {
        Managers.GetNetManager();
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
    private bool rotateFlag = false;
    private int rotateSpeed = 2;
}
