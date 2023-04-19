using GreyFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ErisGame
{
    public class EventThreadManager : Manager
    {
        public override eManager Type => eManager.Event;
        /// <summary>
        /// 相当于Awake
        /// </summary>
        /// <returns></returns>
        public override eHRESULT OnInitialization()
        {
            return eHRESULT.Succeed;
        }
        /// <summary>
        /// 相当于Start
        /// </summary>
        /// <returns></returns>
        public override IEnumerator OnCreate()
        {
            return base.OnCreate();
        }

        protected override void OnUpdate()
        {
            if (notifyEvents.Count <= 0) return;
            while (notifyEvents.Count > 0)
            {
                NotifyData data = notifyEvents.Dequeue();
                var tor = listEvent.GetEnumerator();
                while (tor.MoveNext())
                {
                    if (tor.Current.Key == (int)data.eventName)
                    {
                        for (int i = 0; i < tor.Current.Value.Count; i++)
                        {
                            tor.Current.Value[i].handler(data.pars);
                        }
                    }
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public delegate void HandlerEvent(object[] obj);//参数数组
        public class EventObject
        {
            public HandlerEvent handler;
            public int eventName;
            //触发事件
            public void Notify(object[] obj)
            {
                if (handler != null) handler(obj);
            }

        }
        public class NotifyData
        {
            public int eventName;
            public object[] pars;
        }

        private readonly Dictionary<int, List<EventObject>> listEvent = new Dictionary<int, List<EventObject>>();
        private readonly Queue<EventObject> objectsPool = new Queue<EventObject>();

        private Queue<NotifyData> notifyEvents = new Queue<NotifyData>();
        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handlerEvent">委托事件</param>
        public void Registration(EnumHome.GameEvent eventName, HandlerEvent handlerEvent)
        {
            EventObject eventObj = null;
            if (objectsPool.Count == 0)
            {
                eventObj = new EventObject();
                eventObj.eventName = (int)eventName;
                eventObj.handler = handlerEvent;
            }
            else
            {
                eventObj = objectsPool.Dequeue();
                eventObj.eventName = (int)eventName;
                eventObj.handler = handlerEvent;
            }

            if (listEvent.ContainsKey((int)eventName))
            {
                List<EventObject> events = null;
                listEvent.TryGetValue((int)eventName, out events);
                events.Add(eventObj);
            }
            else
            {
                List<EventObject> events = new List<EventObject>();
                events.Add(eventObj);
                listEvent.Add((int)eventName, events);
            }
        }
        /// <summary>
        /// 注册事件 lua专用
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handlerEvent">委托事件</param>
        public void Registration(int eventId, HandlerEvent handlerEvent)
        {
            EventObject eventObj = null;
            if (objectsPool.Count == 0)
            {
                eventObj = new EventObject();
                eventObj.eventName = eventId;
                eventObj.handler = handlerEvent;
            }
            else
            {
                eventObj = objectsPool.Dequeue();
                eventObj.eventName = eventId;
                eventObj.handler = handlerEvent;
            }
            if (listEvent.ContainsKey(eventId))
            {
                List<EventObject> events = null;
                listEvent.TryGetValue(eventId, out events);
                events.Add(eventObj);
            }
            else
            {
                List<EventObject> events = new List<EventObject>();
                events.Add(eventObj);
                listEvent.Add(eventId, events);
            }

        }

        /// <summary>
        /// 注销事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handlerEvent">委托事件</param>
        public void Cancellation(EnumHome.GameEvent eventName, HandlerEvent handlerEvent)
        {
            if (listEvent.Count > 0)
            {
                if (listEvent.ContainsKey((int)eventName))
                {
                    List<EventObject> events = null;
                    listEvent.TryGetValue((int)eventName, out events);
                    for (int i = events.Count - 1; i >= 0; i--)
                    {
                        if (events[i].eventName == (int)eventName && IsEquals(events[i].handler, handlerEvent))
                        {
                            // UnityEngine.Debug.Log("移除:" + handlerEvent.Target +"."+handlerEvent.Method.Name);
                            events[i].handler = null;
                            objectsPool.Enqueue(events[i]);//回收对象
                            events.RemoveAt(i);
                        }
                    }
                }
                else
                {

                }
            }
        }
        /// <summary>
        /// 注销事件 lua专用
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handlerEvent">委托事件</param>
        public void Cancellation(int eventId, HandlerEvent handlerEvent)
        {
            if (listEvent.Count > 0)
            {
                if (listEvent.ContainsKey(eventId))
                {
                    List<EventObject> events = null;
                    listEvent.TryGetValue(eventId, out events);
                    for (int i = events.Count - 1; i >= 0; i--)
                    {
                        if (events[i].eventName == eventId && IsEquals(events[i].handler, handlerEvent))
                        {
                            // UnityEngine.Debug.Log("移除:" + handlerEvent.Target +"."+handlerEvent.Method.Name);
                            events[i].handler = null;
                            objectsPool.Enqueue(events[i]);//回收对象
                            events.RemoveAt(i);
                        }
                    }
                }
                else
                {

                }
            }
        }
        /// <summary>
        /// 通知事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="obj">参数数组</param>
        public void Notify(EnumHome.GameEvent eventName, params object[] obj)
        {
            NotifyData data = new NotifyData();
            data.eventName = (int)eventName;
            data.pars = obj;
            notifyEvents.Enqueue(data);
        }
        /// <summary>
        /// 通知事件 lua 专用
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="obj">参数数组</param>
        public void Notify(int eventId, params object[] obj)
        {
            NotifyData data = new NotifyData();
            data.eventName = eventId;
            data.pars = obj;
            notifyEvents.Enqueue(data);
        }
        private bool IsEquals(HandlerEvent l, HandlerEvent r)
        {
            if (l.Target.Equals(r.Target) && l.Method.Equals(r.Method))
                return true;
            else return false;
        }
    }
}

