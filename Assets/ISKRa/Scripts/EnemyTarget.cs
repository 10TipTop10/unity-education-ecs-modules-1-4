using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    [SerializeField] private Transform _aimPoint;
    [SerializeField] private Durability _durability;

    public Durability Durability => _durability;

    public Vector3 AimPosition =>   _aimPoint != null   ? _aimPoint.position : transform.position;
}
