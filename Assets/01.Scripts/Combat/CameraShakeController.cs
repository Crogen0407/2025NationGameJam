using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    private static CinemachineBasicMultiChannelPerlin _perlin;

    [ContextMenu("dfdf")]
    public void Shake()
    {
        Shake(1, 1);
    }
    
    public static  async void Shake(float duration = 1, float strength = 1)
    {
        _perlin ??= FindObjectOfType<CinemachineBasicMultiChannelPerlin>();
        
        _perlin.m_AmplitudeGain += strength;
        _perlin.m_FrequencyGain += strength;
        
        await Task.Delay((int)(duration * 1000));
        
        _perlin.m_AmplitudeGain -= strength;
        _perlin.m_FrequencyGain -= strength;
    }
}
