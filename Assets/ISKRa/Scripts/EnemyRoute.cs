using UnityEngine;

public class EnemyRoute : MonoBehaviour
{
    [SerializeField] private Transform[] _points;

    public int Count => _points.Length;

    public Transform GetPoint(int index)
    {
        return _points[index];
    }
}
