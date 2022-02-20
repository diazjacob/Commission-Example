using UnityEngine;

public class LerpPos : MonoBehaviour
{
    [SerializeField] private GameObject _lerpTarget;
    [SerializeField] private float _lerpSpeed;
    [SerializeField] private float _lerpThreshold;
    
    private void Update()
    {
        PosLerp(Vector3.zero);
    }

    protected void PosLerp(Vector3 delta)
    {
        #if UNITY_EDITOR
        Debug.DrawLine(transform.position, _lerpTarget.transform.position, Color.cyan, .001f, true);
        #endif
        
        if (Vector3.Distance(_lerpTarget.transform.position + delta, transform.position) > _lerpThreshold)
            transform.position = Vector3.Slerp(transform.position, _lerpTarget.transform.position + delta, Time.deltaTime * _lerpSpeed);
    }
}
