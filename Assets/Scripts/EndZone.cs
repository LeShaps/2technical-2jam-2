using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EndZone : MonoBehaviour
{
    public bool Activated = false;
    public bool IsYinZone;

    private void OnTriggerEnter(Collider collider)
    {
        // if (!UltimateLoad.Instance.UltimateReady)
        //     return;
        
        PlayerController player;
        if (collider.gameObject.Contains(out player))
        {
            if (IsYinZone && player.CompareTag("Yin") || !IsYinZone && player.CompareTag("Yang"))
            {
                Debug.Log(IsYinZone ? "Yin ready" : "Yang ready");
                Activated = true;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        PlayerController player;
        if (collider.gameObject.Contains(out player))
        {
            Activated = false;
        }
    }
}
