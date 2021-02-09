using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class DofFocus : MonoBehaviour
{
    private Camera cameraMain;
    public VolumeProfile postFxProfile;
    private DepthOfField dof;
    private MinFloatParameter dofDistanceParametar;

    private Ray ray;
    private RaycastHit hit;
    private Vector3 viewportCenter;
    public LayerMask mask;
    // private bool isHit = false;

    public float defaultDistance = 5f;
    public float minDistance = 0.5f;
    private float hitDistance;
    public float focusSpeed = 1f;
    public int updateFrequency = 2;

    private Transform thisTransform;

    private void Awake()
    {
        thisTransform = transform;
    }

    private void Start()
    {
        postFxProfile.TryGet<DepthOfField>(out dof);
        dofDistanceParametar = dof.focusDistance;
        viewportCenter = new Vector3(0.5f, 0.5f, 0);
        cameraMain = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Time.frameCount % updateFrequency == 0)
        {
            ray = cameraMain.ViewportPointToRay(viewportCenter);
            if (Physics.Raycast(ray, out hit, defaultDistance - 0.1f, mask))
            {
                // isHit = true;
                hitDistance = hit.distance;
                if (hitDistance < minDistance)
                {
                    hitDistance = minDistance;
                }
                dofDistanceParametar.value = Mathf.Lerp(dofDistanceParametar.value, hitDistance, focusSpeed);
            }
            else
            {
                // isHit = false;
                if (dofDistanceParametar.value < defaultDistance)
                {
                    dofDistanceParametar.value = Mathf.Lerp(dofDistanceParametar.value, defaultDistance, focusSpeed);
                }
            }
        }
    }

    // private void OnDrawGizmos()
    // {
    //  if (isHit)
    //  {
    //      Gizmos.DrawSphere(hit.point, 0.1f);
    //      Debug.DrawRay(thisTransform.position, thisTransform.forward * hitDistance);
    //  }
    //  else
    //  {
    //      Debug.DrawRay(thisTransform.position, thisTransform.forward * 1000f);
    //  }
    // }
}


