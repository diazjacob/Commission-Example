using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _lerpTarget;
    [SerializeField] private float _lerpSpeed;
    [SerializeField] private float _occludedLerpSpeed;
    [SerializeField] private float _lerpThreshold;
    
    [SerializeField] private GameObject _detector;
    [SerializeField] private GameObject _lookTarget;
    private Vector3 _lerpTargetPos;
    
    private void Update()
    {
        _lerpTargetPos = CheckCameraOccclusion();
        
        PosLerp();
        LookAtLerp();
    }
    
    private void LookAtLerp()
    {
        transform.rotation = Quaternion.LookRotation(_lookTarget.transform.position - transform.position, Vector3.up);
    }
    
    private void PosLerp()
    {
        #if UNITY_EDITOR
        Debug.DrawLine(transform.position, _lerpTargetPos, Color.cyan, .001f, true);
        #endif

        float trueLerpSpeed = _lerpSpeed;
        if (Vector3.SqrMagnitude(_lerpTargetPos - _lerpTarget.transform.position) > 0.001f) trueLerpSpeed = _occludedLerpSpeed;
        
        if (Vector3.Distance(_lerpTargetPos, transform.position) > _lerpThreshold)
            transform.position = Vector3.Slerp(transform.position, _lerpTargetPos, Time.deltaTime * trueLerpSpeed);
    }

    private Vector3 CheckCameraOccclusion()
    {   
        RaycastHit hit;
        var ray = new Ray(_lookTarget.transform.position, _lerpTarget.transform.position - _lookTarget.transform.position);
        var rayHit = Physics.Raycast(ray, out hit, Mathf.Infinity);

        if (rayHit && hit.collider.gameObject != _detector.gameObject)
        {
            return hit.point;
        }
        
        return _lerpTarget.transform.position;
    }
}
