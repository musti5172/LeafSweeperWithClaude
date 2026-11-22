using UnityEngine;

public class PushLeaves : MonoBehaviour
{
    [SerializeField] private float pushRadius = 1f;
    [SerializeField] private float pushSpeed = 0.4f;
    [SerializeField] private float maxLeafSpeed = 2.5f;

    private CharacterController controller;
    private int leafMask;
    private readonly Collider[] buffer = new Collider[64];

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        leafMask = LayerMask.GetMask("Leaf");
    }

    private void FixedUpdate()
    {
        // Vakum çalýþýrken yaprak hýzýný vakum yönetiyor, karýþmasýn
        if (Input.GetMouseButton(0)) return;

        Vector3 vel = controller.velocity;
        vel.y = 0f;
        if (vel.sqrMagnitude < 0.1f) return;

        Vector3 moveDir = vel.normalized;
        Vector3 feet = transform.position + Vector3.down * 0.8f;
        Vector3 center = feet + moveDir * 0.4f;

        int count = Physics.OverlapSphereNonAlloc(center, pushRadius, buffer, leafMask);
        for (int i = 0; i < count; i++)
        {
            Rigidbody rb = buffer[i].attachedRigidbody;
            if (rb == null || rb.isKinematic) continue;
            if (rb.linearVelocity.magnitude > maxLeafSpeed) continue;

            Vector3 dir = rb.position - center;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f) dir = moveDir;

            rb.AddForce(dir.normalized * pushSpeed, ForceMode.VelocityChange);
        }
    }
}
