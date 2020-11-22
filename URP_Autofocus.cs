using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class DofFocus : MonoBehaviour
{
    private Camera cameraMain;
    public VolumeProfile postFxProfile;
    private DepthOfField dof;

    private Ray ray;
    private RaycastHit hit;
    private Vector3 viewportCenter;
    public LayerMask mask;
    // private bool isHit = false;

    public float defaultDistance = 100f;
    private float hitDistance;
    public float focusSpeed = 1f;
    public int updateFrequency = 2;

    private void Start()
    {
        postFxProfile.TryGet<DepthOfField>(out dof);
        viewportCenter = new Vector3(0.5f, 0.5f, 0);
        cameraMain = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Time.frameCount % updateFrequency == 0)
        {
            ray = cameraMain.ViewportPointToRay(viewportCenter);
            if (Physics.Raycast(ray, out hit, 100f, mask))
            {
                // isHit = true;
                hitDistance = Vector3.Distance(transform.position, hit.point);
            }
            else
            {
                // isHit = false;
                hitDistance = defaultDistance;
            }
            dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, hitDistance, focusSpeed);
        }
    }

    // private void OnDrawGizmos()
    // {
    //  if (isHit)
    //  {
    //      Gizmos.DrawSphere(hit.point, 0.1f);
    //      Debug.DrawRay(transform.position, transform.forward * hitDistance);
    //  }
    //  else
    //  {
    //      Debug.DrawRay(transform.position, transform.forward * 1000f);
    //  }
    // }
}


