using UnityEditorInternal;
using UnityEngine;

public class EnemyCatch : MonoBehaviour
{
    private float rotationVelocity;
    private bool CanR = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CanR)
        {
            float SmoothingAngleZ = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.z,
       0f, ref rotationVelocity, 0.2f);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, SmoothingAngleZ);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.transform.SetParent(null);
            CanR = true;

        }
    }
}
