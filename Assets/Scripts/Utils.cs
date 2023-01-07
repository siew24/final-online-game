using UnityEngine;

public static class Utils
{
    public static void GetListener<T>(MonoBehaviour gameObject, out T listener)
    {
        listener = gameObject.GetComponent<T>();
    }
}