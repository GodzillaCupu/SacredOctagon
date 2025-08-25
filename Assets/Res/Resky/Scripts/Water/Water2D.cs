using DGE.Core;
using DGE.Utils;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(LineRenderer))]
public class Water2D : MonoBehaviour, IUpdatable
{
    [Header("Main")]
    public int pointCount = 20; // Number of points for the LineRenderer
    public float waveAmplitude = 0.5f; // Height of the waves
    public float waveFrequency = 1f; // Frequency of the waves
    public float waveSpeed = 2f; // Speed of the wave movement
    public float waterLength = 10f; // Total length of the water
    public float waterWidth = 5;
    public bool hasEndPoint = false;

    private LineRenderer lineRenderer;

    [Space]
    [Header("Advanced")]
    [SerializeField] private WaveData[] waveDatas = new WaveData[0];

    void Start()
    {
        UpdateManager.Add(this);
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointCount;
        lineRenderer.widthMultiplier = waterWidth;

        foreach (WaveData data in waveDatas) data.Start();
    }
    private void OnDestroy()
    {
        UpdateManager.Remove(this);
    }

    void IUpdatable.OnUpdate()
    {
        float time = Time.time * waveSpeed;
        for (int i = 0; i < pointCount; i++)
        {
            bool isEndPoint = hasEndPoint ? (i <= 1) || (i >= pointCount - 2) : false;
            // Calculate the x position of each point
            float x = i / (float)(pointCount - 1) * waterLength;

            // Calculate the y position (wave height)
            float y = isEndPoint ? 0 : Mathf.Sin((x + time) * waveFrequency) * waveAmplitude;

            // Set the position of the point
            lineRenderer.SetPosition(i, new Vector3(x, y, 0) + transform.position);
        }

        foreach (WaveData data in waveDatas) data.Update();
    }

    [Button("TestApply")]
    private void TestApply()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.positionCount = pointCount;
        lr.widthMultiplier = waterWidth;
        for (int i = 0; i < pointCount; i++)
        {
            bool isEndPoint = hasEndPoint ? (i <= 1) || (i >= pointCount - 2) : false;
            // Calculate the x position of each point
            float x = i / (float)(pointCount - 1) * waterLength;

            // Calculate the y position (wave height)
            float y = isEndPoint ? 0 : Mathf.Sin((x + 0) * waveFrequency) * waveAmplitude;

            // Set the position of the point
            lr.SetPosition(i, new Vector3(x, y, 0) + transform.position);
        }
    }

    [System.Serializable]
    private class WaveData
    {
        public Material material = null;
        public float waveSpeed = 1.0f;

        public void Start()
        {
            material.mainTextureOffset = new Vector2(0, material.mainTextureOffset.y);
        }

        public void Update()
        {
            material.mainTextureOffset += Vector2.left * waveSpeed * Time.deltaTime;
        }
    }
}
