using System;
using System.Collections.Generic;

public class EventManager
{
    // 使用字典存储事件和对应的回调
    private Dictionary<string, Action<object[]>> eventDictionary;

    // 单例模式，确保全局只有一个事件管理器
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManager();
            }
            return instance;
        }
    }

    // 构造函数
    private EventManager()
    {
        eventDictionary = new Dictionary<string, Action<object[]>>();
    }

    // 订阅事件
    public void Subscribe(string eventName, Action<object[]> listener)
    {
        if (eventDictionary.TryGetValue(eventName, out Action<object[]> thisEvent))
        {
            thisEvent += listener;
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            eventDictionary.Add(eventName, thisEvent);
        }
    }

    // 取消订阅事件
    public void Unsubscribe(string eventName, Action<object[]> listener)
    {
        if (eventDictionary.TryGetValue(eventName, out Action<object[]> thisEvent))
        {
            thisEvent -= listener;
            if (thisEvent == null)
            {
                eventDictionary.Remove(eventName);
            }
            else
            {
                eventDictionary[eventName] = thisEvent;
            }
        }
    }

    // 触发事件
    public void TriggerEvent(string eventName, params object[] parameters)
    {
        if (eventDictionary.TryGetValue(eventName, out Action<object[]> thisEvent))
        {
            thisEvent.Invoke(parameters);
        }
    }
}