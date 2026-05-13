// ActivateObjectOnCollision.cs

using UnityEngine;

public class DisableObjectOnCollision : MonoBehaviour
{
    [Header("Object To Activate")]
    [SerializeField] private GameObject objectToActivate;

    [Header("Options")]
    [SerializeField] private bool DisableObjectOnlyOnce = true;
    [SerializeField] private bool disableThisObjectAfterActivation = false;

    private bool hasActivated;

    private void OnCollisionEnter(Collision collision)
    {
        if (DisableObjectOnlyOnce && hasActivated)
            return;

        hasActivated = true;

        if (objectToActivate != null)
            objectToActivate.SetActive(false);

        if (disableThisObjectAfterActivation)
            gameObject.SetActive(false);
    }
}