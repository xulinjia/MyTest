using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class EventTriggerListener : EventTrigger, IDisposable
{
    public static float clickDragThreshold = 0.1f;
    public static float clickPressThreshold = 0.2f;//长按超过一定时间 就不再认为是点击

    public delegate void NoArgsVoidDelegate();
    public delegate void VoidDelegate(GameObject go);
    public delegate void BoolDelegate(GameObject go, bool state);
    public delegate void OnValueChangedDelegate(bool isToggle);
    public delegate void OnScrollRectValueChangedDelegate(Vector2 delta);
    public delegate void OnSliderValueChangedDelegate(float delta);
    public delegate void FloatDelegate(GameObject go, float value);
    public delegate void VectorDelegate(GameObject go, Vector3 value);
    public delegate void PointerEventDataDelegate(GameObject go, PointerEventData value);

    public VoidDelegate onClick;
    public VoidDelegate onClickShot;
    public VectorDelegate onClickEx; //会返回点击坐标点
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
    public BoolDelegate onPress;
    public BoolDelegate onLongPress; //0.3秒钟后认为是 按压状态第一帧
    public BoolDelegate onHover;
    public VoidDelegate onDrag;
    public VoidDelegate onDrop;
    public VoidDelegate onBeginDrag;
    public VoidDelegate onEndDrag;
    public OnValueChangedDelegate onValueChanged;
    public OnScrollRectValueChangedDelegate onScrollRectValueChanged;
    public OnSliderValueChangedDelegate onSliderValueChanged;


    public PointerEventDataDelegate onMouseDown;
    public PointerEventDataDelegate onMouseUp;

    //扩展 
    public PointerEventDataDelegate onDragDk;
    public PointerEventDataDelegate onBeginDragDk;
    public PointerEventDataDelegate onEndDragDk;
    public PointerEventDataDelegate onClickDk;
    public PointerEventDataDelegate onDropDk;

    private bool m_BeginDrag = false;
    private bool m_Pressed = false;
    private bool m_IsOver = false;
    private bool m_Interactable = true;
    private Selectable m_Selectable = null;
    private Toggle m_Toggle = null;
    private ScrollRect m_ScrollRect = null;
    private Slider m_Slider = null;
    private Button m_Button = null;

    private float m_PressDragLength = 0;

    public bool pressed { get { return m_Pressed; } }
    public bool isOver { get { return m_IsOver; } }
    public bool interactable { get { return m_Interactable; } set { m_Interactable = value; if (m_Selectable != null) m_Selectable.interactable = value; } }

    private float pressTime = 0;
    private bool isDoPress = false;
    private bool isTriggerLongPress = false;

    private GraphicRaycaster gr = null;
    void OnDisable()
    {

        m_IsOver = false;
        m_Pressed = false;
        isDoPress = false;
        isTriggerLongPress = false;
        m_BeginDrag = false;
        pressTime = 0;
    }
    private void Update()
    {
        if (m_Pressed && !m_BeginDrag)
        {
            pressTime += Time.deltaTime / Mathf.Max(0.0001f, Time.timeScale);
            if (isDoPress == false)
            {
                if (pressTime > 0.2f)
                {
                    isDoPress = true;
                    isTriggerLongPress = true;
                    if (onLongPress != null)
                    {
                        onLongPress(gameObject, true);
                    }
                }
            }

        }
        else
        {
            //pressTime = 0;
            //isDoPress = false;
            //if (onLongPress != null)
            //{
            //    onLongPress(gameObject, false);
            //}

        }
    }
    public static EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null)
        {
            listener = go.AddComponent<EventTriggerListener>();
            listener.m_Selectable = go.GetComponent<Selectable>();
            listener.m_Toggle = go.GetComponent<Toggle>();
            listener.m_ScrollRect = go.GetComponent<ScrollRect>();
            listener.m_Slider = go.GetComponent<Slider>();
            listener.m_Button = go.GetComponent<Button>();

        }

        return listener;
    }

    public void SetToggleOnValueChangedCallback(OnValueChangedDelegate callback)
    {
        var toggle = m_Toggle as UnityEngine.UI.Toggle;
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(ifselect => { callback(ifselect); });
    }
    public void SetScrollRectOnValueChangedCallback(OnScrollRectValueChangedDelegate callback)
    {
        this.m_ScrollRect.onValueChanged.RemoveAllListeners();
        this.m_ScrollRect.onValueChanged.AddListener(ifselect => { callback(ifselect); });
    }
    public void SetSliderOnValueChangedCallback(OnSliderValueChangedDelegate callback)
    {
        this.m_Slider.onValueChanged.RemoveAllListeners();
        this.m_Slider.onValueChanged.AddListener(delta => { callback(delta); });
    }
    public void SetButtonOnClickCallback(NoArgsVoidDelegate callback)
    {
        this.m_Button.onClick.RemoveAllListeners();
        this.m_Button.onClick.AddListener(delegate { callback(); });
    }

    public void AddEventOnPress(BoolDelegate callback)
    {
        this.onPress = callback;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClickShot != null) onClickShot(gameObject);
        else if (onClickEx != null) onClickEx(gameObject, eventData.position);
        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, ExecuteEvents.ValidateEventData<BaseEventData>(eventData), ExecuteEvents.pointerClickHandler);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        m_Pressed = true;
        m_PressDragLength = 0;
        if (m_Interactable && onDown != null) onDown(gameObject);
        if (m_Interactable && onMouseDown != null)
            onMouseDown(gameObject, eventData);
        if (m_Interactable && onPress != null) onPress(gameObject, true);
        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, ExecuteEvents.ValidateEventData<BaseEventData>(eventData), ExecuteEvents.pointerDownHandler);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        m_Pressed = false;
        if (m_Interactable && onClick != null && m_PressDragLength < clickDragThreshold)
        {
            onClick(gameObject);
        }
        if (m_Interactable && onClickDk != null && m_PressDragLength < clickDragThreshold && pressTime < clickPressThreshold)
        {
            onClickDk(gameObject, eventData);
        }
        if (m_Interactable && onUp != null) onUp(gameObject);
        if (m_Interactable && onMouseUp != null)
            onMouseUp(gameObject, eventData);
        if (m_Interactable && onPress != null) onPress(gameObject, false);

        pressTime = 0;
        isDoPress = false;
        if (onLongPress != null && isTriggerLongPress)
        {
            onLongPress(gameObject, false);
        }
        isTriggerLongPress = false;
        //ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, ExecuteEvents.ValidateEventData<BaseEventData>(eventData), ExecuteEvents.pointerUpHandler);
    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        m_IsOver = true;
        if (m_Interactable && onEnter != null) onEnter(gameObject);
        if (m_Interactable && onHover != null) onHover(gameObject, true);
        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, ExecuteEvents.ValidateEventData<BaseEventData>(eventData), ExecuteEvents.pointerEnterHandler);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        m_IsOver = false;
        if (m_Interactable && onExit != null) onExit(gameObject);
        if (m_Interactable && onHover != null) onHover(gameObject, false);
        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, ExecuteEvents.ValidateEventData<BaseEventData>(eventData), ExecuteEvents.pointerExitHandler);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (m_Interactable && onSelect != null) onSelect(gameObject);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (m_Interactable && onUpdateSelect != null) onUpdateSelect(gameObject);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (m_Pressed)
        {
            m_PressDragLength += eventData.delta.magnitude;
        }
        if (m_Interactable && onDrag != null) onDrag(gameObject);
        OnDragDk(eventData);
        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, ExecuteEvents.ValidateEventData<BaseEventData>(eventData), ExecuteEvents.dragHandler);
    }
    public override void OnDrop(PointerEventData eventData)
    {
        if (m_Interactable && onDrag != null) onDrag(gameObject);
        OnDropDk(eventData);
        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, ExecuteEvents.ValidateEventData<BaseEventData>(eventData), ExecuteEvents.dropHandler);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (m_Interactable && onBeginDrag != null) onBeginDrag(gameObject);
        OnBeginDragDk(eventData);
        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, ExecuteEvents.ValidateEventData<BaseEventData>(eventData), ExecuteEvents.beginDragHandler);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (m_Interactable && onEndDrag != null) onEndDrag(gameObject);
        OnEndDragDk(eventData);
        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, ExecuteEvents.ValidateEventData<BaseEventData>(eventData), ExecuteEvents.endDragHandler);
    }

    //扩展
    public void OnDragDk(PointerEventData eventData)
    {
        if (m_Interactable && onDragDk != null) onDragDk(gameObject, eventData);
    }
    public void OnBeginDragDk(PointerEventData eventData)
    {
        m_BeginDrag = true;
        if (m_Interactable && onBeginDragDk != null) onBeginDragDk(gameObject, eventData);
    }
    public void OnEndDragDk(PointerEventData eventData)
    {
        m_BeginDrag = false;
        if (m_Interactable && onEndDragDk != null) onEndDragDk(gameObject, eventData);
    }
    public void OnDropDk(PointerEventData eventData)
    {
        if (m_Interactable && onDropDk != null) onDropDk(gameObject, eventData);
    }
    public void GetPointPressGameObject(PointerEventData eventData)
    {
        GraphicRaycaster gr = transform.GetComponentInParent<GraphicRaycaster>();
        if (gr != null)
        {
            System.Collections.Generic.List<RaycastResult> rs = new System.Collections.Generic.List<RaycastResult>();
            gr.Raycast(eventData, rs);

            foreach (var item in rs)
            {
                Debug.Log(item.gameObject.name);
            }
        }
    }
    public bool IsPointPressGameObject(PointerEventData eventData, string gameobjName)
    {

        System.Collections.Generic.List<RaycastResult> rs = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, rs);
        bool isPress = false;
        foreach (var item in rs)
        {
            if (item.gameObject.name == gameobjName)
            {
                isPress = true;
                break;
            }
        }
        return isPress;

    }

    public void UnregisterAllEvents()
    {
        if (this.m_Button)
        {
            if (this.m_Button.onClick != null)
            {
                this.m_Button.onClick.AddListener(null);
                this.m_Button.onClick.RemoveAllListeners();
            }
            m_Button = null;
        }
        onClick = null;
        onClickShot = null;
        onClickEx = null;
        onDown = null;
        onEnter = null;
        onExit = null;
        onUp = null;
        onSelect = null;
        onUpdateSelect = null;
        onPress = null;
        onLongPress = null;
        onHover = null;
        onDrag = null;
        onDrop = null;
        onBeginDrag = null;
        onEndDrag = null;
        onValueChanged = null;

        onBeginDragDk = null;
        onEndDragDk = null;
        onDragDk = null;
        onClickDk = null;
        onMouseDown = null;
        onMouseUp = null;

        if (m_Toggle)
        {
            if (m_Toggle.onValueChanged != null)
            {
                m_Toggle.onValueChanged.AddListener(null);
                m_Toggle.onValueChanged.RemoveAllListeners();
            }
        }
        m_Toggle = null;

        if (m_ScrollRect)
        {
            if (m_ScrollRect.onValueChanged != null)
            {
                m_ScrollRect.onValueChanged.AddListener(null);
                m_ScrollRect.onValueChanged.RemoveAllListeners();
            }
        }
        m_ScrollRect = null;

        if (m_Slider)
        {
            if (m_Slider.onValueChanged != null)
            {
                m_Slider.onValueChanged.AddListener(null);
                m_Slider.onValueChanged.RemoveAllListeners();
            }
        }
        m_Slider = null;
    }

    public void UnloadFunction()
    {
        UnregisterAllEvents();
    }

    private void OnDestroy()
    {
        UnloadFunction();
    }

    public void Dispose()
    {
        UnloadFunction();
    }
}