using Unity.Cinemachine;
using UnityEngine;

public class Cameracontol : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smooth;

    void FixedUpdate() {
        Follow();
    }

    private void Follow() {
        Vector3 campos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, campos, smooth * Time.fixedDeltaTime);
    }
}
