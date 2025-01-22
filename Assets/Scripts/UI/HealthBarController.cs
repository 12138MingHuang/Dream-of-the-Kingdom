using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    [Header("Elements")]
    public Transform healthBarTrans;
    private UIDocument healthBarDoc;
    private ProgressBar healthBar;

    private void Awake()
    {
        healthBarDoc = GetComponent<UIDocument>();
        healthBar = healthBarDoc.rootVisualElement.Q<ProgressBar>();
        MoveToWorldPosition(healthBar, healthBarTrans.position, Vector2.zero);
    }
    private void MoveToWorldPosition(VisualElement element, Vector3 worldPos, Vector2 size)
    {
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPos, size, Camera.main);
        element.transform.position = rect.position;
    }
    
    [ContextMenu("Test")]
    public void Test()
    {
        healthBarDoc = GetComponent<UIDocument>();
        healthBar = healthBarDoc.rootVisualElement.Q<ProgressBar>();
        MoveToWorldPosition(healthBar, healthBarTrans.position, Vector2.zero);
    }
}