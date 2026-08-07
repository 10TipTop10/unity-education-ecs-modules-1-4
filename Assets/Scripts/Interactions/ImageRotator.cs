using UnityEngine;

public class ImageRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;

    private void Update()
    {
        transform.Rotate(0f, 0f, -_rotationSpeed * Time.deltaTime);
    }
}
