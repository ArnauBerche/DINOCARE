using UnityEngine;

public class Shake : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Vector3 moveDirection = Vector3.right;
    public bool useLocalPosition = true;

    public bool active = true;

    public Vector3 shakeAmplitude = new Vector3(0.1f, 0.1f, 0f);
    public float shakeFrequency = 10f;
    public bool usePerlin = true;

    Vector3 _basePosition;
    float _seed;

    void Awake()
    {
        _seed = Random.Range(0f, 1000f);
        _basePosition = useLocalPosition ? transform.localPosition : transform.position;
    }

    void OnEnable()
    {
        _basePosition = useLocalPosition ? transform.localPosition : transform.position;
    }

    void Update()
    {
        if (!active) return;

        Vector3 dir = moveDirection.normalized;
        _basePosition += dir * moveSpeed * Time.deltaTime;

        Vector3 offset;
        if (usePerlin)
        {
            float t = Time.time * shakeFrequency;
            float x = (Mathf.PerlinNoise(t + _seed, 0f) - 0.5f) * 2f * shakeAmplitude.x;
            float y = (Mathf.PerlinNoise(t + _seed + 37.13f, 0f) - 0.5f) * 2f * shakeAmplitude.y;
            float z = (Mathf.PerlinNoise(t + _seed + 73.31f, 0f) - 0.5f) * 2f * shakeAmplitude.z;
            offset = new Vector3(x, y, z);
        }
        else
        {
            float x = Mathf.Sin(Time.time * shakeFrequency + _seed) * shakeAmplitude.x;
            float y = Mathf.Sin(Time.time * shakeFrequency * 1.23f + _seed) * shakeAmplitude.y;
            float z = Mathf.Sin(Time.time * shakeFrequency * 1.7f + _seed) * shakeAmplitude.z;
            offset = new Vector3(x, y, z);
        }

        if (useLocalPosition) transform.localPosition = _basePosition + offset;
        else transform.position = _basePosition + offset;
    }

    public void SetActive(bool on)
    {
        active = on;
        if (!active)
        {
            if (useLocalPosition) transform.localPosition = _basePosition;
            else transform.position = _basePosition;
        }
    }

    public void ResetBasePosition()
    {
        _basePosition = useLocalPosition ? transform.localPosition : transform.position;
    }
}
