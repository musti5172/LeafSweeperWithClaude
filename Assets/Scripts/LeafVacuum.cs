using UnityEngine;
using System.Collections.Generic;

public class LeafVacuum : MonoBehaviour
{
    [SerializeField] private Transform vacuumTip;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private BagCapacity bagCapacity;

    [Header("Vakum Ayarlarý")]
    [SerializeField] private float vacuumDistance = 1.5f;
    [SerializeField] private float vacuumRadius = 2f;
    [SerializeField] private float pullSpeed = 6f;
    [SerializeField] private float collectDistance = 0.6f;
    [SerializeField] private float coneAngle = 35f;
    [SerializeField] private int maxPulledAtOnce = 6;

    [Header("Çöp Kutusu")]
    [SerializeField] private float binReach = 3f;

    private int leafLayer;
    private int playerLayer;
    private bool vacuuming;

    private readonly List<Rigidbody> candidates = new List<Rigidbody>();
    private readonly HashSet<Rigidbody> pulledNow = new HashSet<Rigidbody>();
    private void Awake()
    {
        leafLayer = LayerMask.NameToLayer("Leaf");
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        vacuuming = Input.GetMouseButton(0) && !bagCapacity.IsFull();

        // Vakum açýkken yaprak-oyuncu çarpýþmasýný kapat, býrakýnca geri aç
        Physics.IgnoreLayerCollision(leafLayer, playerLayer, vacuuming);

        if (Input.GetMouseButtonDown(1))
            TryEmptyBag();
    }

    private void FixedUpdate()
    {
        if (vacuuming)
            Vacuum();
        else
            ReleasePulled();
    }

    private void ReleasePulled()
    {
        foreach (Rigidbody rb in pulledNow)
        {
            if (rb != null)
                rb.linearVelocity = Vector3.zero;
        }
        pulledNow.Clear();
    }

    private void Vacuum()
    {
        // 1) Uca yaklaþan yapraklarý topla (yapraðýn merkezine göre)
        Collider[] nearTip = Physics.OverlapSphere(vacuumTip.position, collectDistance + 0.5f);
        foreach (Collider col in nearTip)
        {
            if (!col.CompareTag("Leaf")) continue;

            Rigidbody leafRb = col.attachedRigidbody;
            Transform leafTf = leafRb != null ? leafRb.transform : col.transform;

            if (Vector3.Distance(leafTf.position, vacuumTip.position) > collectDistance) continue;
            if (bagCapacity.IsFull()) break;

            LeafValue value = leafTf.GetComponent<LeafValue>();
            bagCapacity.AddLeaf(value != null ? value.points : 1);
            pulledNow.Remove(leafRb);
            Destroy(leafTf.gameObject);
        }

        // 2) Koni içindeki aday yapraklarý bul
        candidates.Clear();
        float range = vacuumDistance + vacuumRadius;
        Vector3 center = vacuumTip.position + cameraTransform.forward * (range * 0.5f);
        Collider[] hits = Physics.OverlapSphere(center, range * 0.5f);

        foreach (Collider col in hits)
        {
            if (!col.CompareTag("Leaf")) continue;

            Rigidbody rb = col.attachedRigidbody;
            if (rb == null) continue;

            Vector3 toLeaf = rb.position - vacuumTip.position;
            if (toLeaf.magnitude > collectDistance &&
                Vector3.Angle(cameraTransform.forward, toLeaf) > coneAngle)
                continue;

            candidates.Add(rb);
        }

        // 3) En yakýn olanlarý seç: ayný anda en fazla maxPulledAtOnce ve boþ yer kadar
        Vector3 tip = vacuumTip.position;
        candidates.Sort((a, b) =>
            (a.position - tip).sqrMagnitude.CompareTo((b.position - tip).sqrMagnitude));

        int allowed = Mathf.Min(maxPulledAtOnce, bagCapacity.RemainingSpace);
        var selected = new HashSet<Rigidbody>();
        for (int i = 0; i < candidates.Count && i < allowed; i++)
            selected.Add(candidates[i]);

        // 4) Seçilmeyen ama önceden çekilen yapraklarý durdur
        foreach (Rigidbody rb in pulledNow)
        {
            if (rb != null && !selected.Contains(rb))
                rb.linearVelocity = Vector3.zero;
        }

        // 5) Seçilenleri çek
        foreach (Rigidbody rb in selected)
        {
            Vector3 toLeaf = rb.position - vacuumTip.position;
            float speed = Mathf.Min(pullSpeed, toLeaf.magnitude / Time.fixedDeltaTime);
            rb.linearVelocity = -toLeaf.normalized * speed;
        }

        pulledNow.Clear();
        foreach (Rigidbody rb in selected)
            pulledNow.Add(rb);
    }

    private void TryEmptyBag()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, binReach))
        {
            if (hit.collider.GetComponent<TrashBin>() != null)
                bagCapacity.EmptyBag();
        }
    }

    private void OnDisable()
    {
        Physics.IgnoreLayerCollision(leafLayer, playerLayer, false);
    }
}
