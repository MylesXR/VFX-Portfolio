// ActivateObjectOnCollision.cs

using UnityEngine;

public class ActivateObjectOnCollision : MonoBehaviour
{
    [Header("Object To Activate")]
    [SerializeField] private GameObject objectToActivate;

    [Header("Collision Filter")]
    [SerializeField] private string requiredCollisionTag = "Environment";

    [Header("Rigidbody Settings")]
    [SerializeField] private Rigidbody targetRigidbody;
    [SerializeField] private bool makeKinematicOnCollision = true;
    [SerializeField] private bool clearVelocityOnCollision = true;

    [Header("Options")]
    [SerializeField] private bool activateOnlyOnce = true;
    [SerializeField] private bool disableThisObjectAfterActivation = false;
    [SerializeField] private bool affectRigidbody = true;
    private bool hasActivated;

    private void Awake()
    {
        if (affectRigidbody && targetRigidbody == null)
            targetRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(requiredCollisionTag))
            return;

        if (activateOnlyOnce && hasActivated)
            return;

        hasActivated = true;

        if (affectRigidbody && targetRigidbody != null && makeKinematicOnCollision)
        {
            if (clearVelocityOnCollision)
            {
                targetRigidbody.velocity = Vector3.zero;
                targetRigidbody.angularVelocity = Vector3.zero;
            }

            targetRigidbody.isKinematic = true;
        }

        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        if (disableThisObjectAfterActivation)
            gameObject.SetActive(false);
    }
}