using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MakeNonKinematicOnGrab : MonoBehaviour
{
    [SerializeField] private Rigidbody targetRigidbody;
    [SerializeField] private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        if (targetRigidbody == null)
            targetRigidbody = GetComponent<Rigidbody>();

        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
    }

    private void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        //Debug.Log("Grenade grabbed: setting Rigidbody to non-kinematic.");

        if (targetRigidbody == null)
            return;

        targetRigidbody.isKinematic = false;
        targetRigidbody.useGravity = true;
        targetRigidbody.WakeUp();
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        //Debug.Log("Grenade released. Rigidbody should stay non-kinematic for throwing.");

        if (targetRigidbody == null)
            return;

        targetRigidbody.isKinematic = false;
        targetRigidbody.useGravity = true;
        targetRigidbody.WakeUp();
    }
}