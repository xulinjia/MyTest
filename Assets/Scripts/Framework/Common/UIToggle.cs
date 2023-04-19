using UnityEngine;
using UnityEngine.UI;
using System;
using GreyFramework;

public class UIToggle : MonoBehaviour
{
    [SerializeField]
    bool _isOn = false;
    public MaskableGraphic targetGraphics;
    public Transform checkedTransform;
    public Transform unCheckedTransform;
    public UIToggleGroup UIToggleGroup;
    private bool isCanUse = true;
    private Action clickEvent;
    private Action<bool> onValueChanged;
    private Func<bool, bool> onPressCheck;

    private bool inited = false;
    private void Awake()
    {
        if (targetGraphics == null)
        {
            Debug.LogError("targetGraphics is null");
            return;
        }

        SetSwitch();
        initEvent();
    }

    private void initEvent()
    {
        if (inited == true)
        {
            return;
        }

        inited = true;
        EventTriggerListener.Get(targetGraphics.gameObject).onClick = OnClick;
        if (UIToggleGroup != null)
        {
            UIToggleGroup.AddUIToggle(this);
        }

    }
    public void SetEnable()
    {
        isCanUse = true;
    }
    public void SetDisable()
    {
        isCanUse = false;
    }
    public void SetClickCallback(Action action)
    {
        initEvent();
        clickEvent = action;
    }
    public void SetOnValueChanged(Action<bool> action)
    {
        initEvent();
        onValueChanged = action;
    }

    public void SetOnPressCheck(Func<bool, bool> action)
    {
        initEvent();
        onPressCheck = action;
    }

    public void SetSelected(bool isSelected)
    {

        if (onPressCheck != null && onPressCheck(isSelected) == false)
        {
            return;
        }

        if (UIToggleGroup != null)
        {
            UIToggleGroup.ToggleClickEvent(this);
        }
        else
        {
            _isOn = isSelected;
            SetSwitch();
            if (onValueChanged != null)
            {
                onValueChanged.Invoke(_isOn);
            }
        }
    }
    public void SetSelectedByTg(bool isSelected)
    {
        if (onPressCheck != null && onPressCheck(isSelected) == false)
        {
            return;
        }

        _isOn = isSelected;
        SetSwitch();
        if (onValueChanged != null)
        {
            onValueChanged.Invoke(_isOn);
        }
    }
    private void OnClick(GameObject go)
    {
        if (clickEvent != null)
        {
            clickEvent();
        }

        if (onPressCheck != null && onPressCheck(!_isOn) == false)
        {
            return;
        }

        if (isCanUse)
        {
            if (UIToggleGroup != null)
            {
                UIToggleGroup.ToggleClickEvent(this);
            }
            else
            {
                _isOn = !_isOn;
                SetSwitch();
                if (onValueChanged != null)
                {
                    onValueChanged.Invoke(_isOn);
                }
            }
        }
    }
    private void SetSwitch()
    {
        if (checkedTransform)
        {
            checkedTransform.gameObject.SetActive(_isOn);
        }

        if (unCheckedTransform)
        {
            unCheckedTransform.gameObject.SetActive(!_isOn);
        }
    }
    private void OnDestroy()
    {
        if (onValueChanged != null)
        {
            onValueChanged = null;
        }
        clickEvent = null;
    }

    public void Dispose()
    {
        if (onValueChanged != null)
        {
            onValueChanged = null;
        }
        clickEvent = null;
    }
}
