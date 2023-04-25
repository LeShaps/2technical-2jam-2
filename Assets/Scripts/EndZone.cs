using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EndZone : MonoBehaviour
{
    public bool Activated = false;

    private void OnCollisionEnter(Collision collision) {
        Player player;

        if (collision.gameObject.Contains(out player)) {
            Activated = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        Player player;

        if (collision.gameObject.Contains(out player)) {
            Activated = false;
        }
    }
}
