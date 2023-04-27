using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EndZone : MonoBehaviour
{
    public bool Activated = false;

    private void OnTriggerEnter(Collider collider)
    {
        PlayerController player;
        if (collider.gameObject.Contains(out player))
        {
            Activated = true;
            Debug.Log("Desactivated");
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        PlayerController player;
        if (collider.gameObject.Contains(out player))
        {
            Activated = false;
            Debug.Log("Activated");
        }
    }
}
