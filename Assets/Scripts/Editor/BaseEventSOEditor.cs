using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Delegate = System.Delegate;

[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor
{
    private BaseEventSO<T> baseEventSO;

    private void OnEnable()
    {
        if(baseEventSO == null)
            baseEventSO = (BaseEventSO<T>)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("订阅数量：" + GetListeners().Count.ToString(), EditorStyles.boldLabel);
        
        foreach (MonoBehaviour listener in GetListeners())
        {
            EditorGUILayout.LabelField(listener.ToString()); // 显示监听器的名称
        }
    }
    
    /// <summary>
    /// 获取所有监听器的列表
    /// </summary>
    /// <returns> 监听器列表 </returns>
    private List<MonoBehaviour> GetListeners()
    {
        List<MonoBehaviour> listeners = new List<MonoBehaviour>();
        
        if(baseEventSO == null || baseEventSO.OnEventRaised == null)
            return listeners; // 没有监听器，直接返回空列表
        
        Delegate[] subscribers =  baseEventSO.OnEventRaised.GetInvocationList();
        foreach (Delegate subscriber in subscribers)
        {
            MonoBehaviour obj = subscriber.Target as MonoBehaviour;
            if(!listeners.Contains(obj))
                listeners.Add(obj);
        }
        
        return listeners;
    }
}