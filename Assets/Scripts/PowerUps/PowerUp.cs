using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private PowerUpType type;
    private Pulpit sourcePulpit;

    public void Initialize(PowerUpType powerUpType, Pulpit pulpit)
    {
        type = powerUpType;
        sourcePulpit = pulpit;

        if (sourcePulpit != null)
        {
            sourcePulpit.OnPulpitDestroyed += OnPulpitDestroyed;
        }
    }

    private void OnPulpitDestroyed(Pulpit pulpit)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (sourcePulpit != null)
        {
            sourcePulpit.OnPulpitDestroyed -= OnPulpitDestroyed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PowerUpManager.Instance.ActivatePowerUp(type);
            Destroy(gameObject);
        }
    }
}
