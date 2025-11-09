using UnityEditorInternal;
using UnityEngine;

public class EnemyCatch : MonoBehaviour
{
    private float rotationVelocity;
    private bool CanR = false;
    public static EnemyCatch instance;
    public GameObject HealthUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Update()
    {

        if (CanR)
        {
            float SmoothingAngleZ = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.z,
            0f, ref rotationVelocity, 0.2f);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, SmoothingAngleZ);
        }

        if (transform.rotation.z < 0.01f) 
        {
            if (Vector3.Distance(transform.position, Player.instance.transform.position) > 1f)
                transform.position = Vector3.MoveTowards(transform.position, Player.instance.transform.position, Time.deltaTime * 2f);
            CanR = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            CanR = true;

            HealthUI.SetActive(true);
            gameObject.transform.SetParent(null);
        }
    }

    public void DamageEnd()
    {
        EnemyHealthContainer.instance.GetComponent<Animator>().SetBool("Damage", false);
    }
}
