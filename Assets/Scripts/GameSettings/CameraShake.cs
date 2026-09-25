using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private CinemachineImpulseSource impulseSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    /// <summary>
    /// Panggil fungsi ini untuk memicu getaran kamera via Cinemachine.
    /// </summary>
    /// <param name="force">Kekuatan getaran (default: 1f)</param>
    public void Shake(float force = 1f)
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulseWithForce(force);
        }
        else
        {
            Debug.LogWarning("CinemachineImpulseSource tidak ditemukan di GameObject " + gameObject.name);
        }
    }
}