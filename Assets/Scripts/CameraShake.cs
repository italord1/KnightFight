using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CinemachineCamera cam;
    public static CameraShake instance;

    private float shakeTime;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTime > 0f)
        {
            shakeTime -= Time.deltaTime;
            if (shakeTime <= 0f)
            {
                CinemachineBasicMultiChannelPerlin perlin = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();

                perlin.AmplitudeGain = 0f;
            }
        }
    }

    public void Shake(float intensity, float duration)
    {
        shakeTime = duration;

        CinemachineBasicMultiChannelPerlin perlin = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.AmplitudeGain = intensity;
    }
}
