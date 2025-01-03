using System;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _lineRenderer;
    [SerializeField]
    private float _lineSpeed = 0.1f;

    private void Update()
    {
        if (_lineRenderer != null)
        {
            Vector2 offset = _lineRenderer.material.mainTextureOffset;
            offset.x += _lineSpeed * Time.deltaTime;
            _lineRenderer.material.mainTextureOffset = offset;
        }
    }
}
