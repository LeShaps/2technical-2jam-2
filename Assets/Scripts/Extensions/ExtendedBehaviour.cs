using UnityEngine;

public static class ExtendedBehaviour
{
    public static Component GetOrAddComponent<T>(this GameObject gameObject, string Name = null)
        where T : Component
    {
        if (gameObject.GetComponent<T>() is Component component && component != null)
            return component;

        return gameObject.AddComponent<T>();
    }

    public static T GetComponentInSibling<T>(this GameObject go)
        => go.transform.parent.GetComponentInChildren<T>();
    
    public static bool Contains<T>(this GameObject go, out T component) {
        if (go.GetComponent<T>() != null) {
            component = go.GetComponent<T>();
            return true;
        }

        component = default;
        return false;
    }

}
