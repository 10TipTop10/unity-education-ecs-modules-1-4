using UnityEngine;

public class ObjectDeath : MonoBehaviour
{
    [SerializeField] private float _lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }
}
