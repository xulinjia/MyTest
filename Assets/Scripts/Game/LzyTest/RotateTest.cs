using System;
using UnityEngine;
using UnityEngine.UI;

public class RotateTest : MonoBehaviour
{
    public GameObject Cube_Test;
    private void Awake()
    {
        btn_test = transform.GetComponent<Button>();
        btn_test.onClick.AddListener(OnStartButtonClick);
    }

    private void OnStartButtonClick()
    {
        rotateFlag = !rotateFlag;
    }
    private void Update()
    {
        if (rotateFlag)
        {
            Cube_Test.transform.Rotate(rotateSpeed, -rotateSpeed/2, rotateSpeed/3);
        }
    }

    private Button btn_test;
    private bool rotateFlag = false;
    private int rotateSpeed = 2;
}
