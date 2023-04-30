using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EndZone : MonoBehaviour
{
    public bool Activated = false;
    public bool IsYinZone;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        
        PlayerController player;
        if (collision.gameObject.Contains(out player))
        {
            if (IsYinZone && player.CompareTag("Yin") || !IsYinZone && player.CompareTag("Yang"))
            {
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
