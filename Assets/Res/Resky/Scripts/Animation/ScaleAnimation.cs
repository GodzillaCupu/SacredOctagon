using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DGE.Utils;
using DGE.Core;

public class ScaleAnimation : MonoBehaviour, IUpdatable
{
    [Header("Components")]
    [SerializeField] private Transform target = null;

    [Space]
    [Header("Settings")]
    [SerializeField] private AnimationCurve curve = null;
    [SerializeField] private float duration = 3;
    [SerializeField] private float speed = 1;

    private float time = 0;

    private void OnEnable()
    {
        UpdateManager.Add(this);
    }
    private void OnDisable()
    {
        UpdateManager.Remove(this);
    }

    void IUpdatable.OnUpdate()
    {
        time += Time.deltaTime * speed;
        if (time >= duration) time = 0;
        float scale = curve.Evaluate(time);
        target.localScale = new Vector3(scale, scale, scale);
    }
}
