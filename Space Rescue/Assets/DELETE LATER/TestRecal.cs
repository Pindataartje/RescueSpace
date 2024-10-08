using UnityEngine;

public class TestRecal : MonoBehaviour
{
    [SerializeField] LayerMask _recalLayer;

    public bool canCheck;

    [SerializeField] float _startScale;
    [SerializeField] float _maxScale;

    [SerializeField] float _scaleSpeed;

    [SerializeField] Vector3 _originalDetectionScale;

    private float _elapsedTime;

    private void Start()
    {
        _originalDetectionScale = transform.localScale;
        _elapsedTime = 0f;
    }

    public void DetectionPulse()
    {
        _elapsedTime += Time.deltaTime * _scaleSpeed;

        float scale = Mathf.Lerp(_startScale, _maxScale, _elapsedTime);
        scale = Mathf.Min(scale, _maxScale);

        transform.localScale = _originalDetectionScale * scale;
        transform.localScale = new Vector3(transform.localScale.x, 0.25f, transform.localScale.z);
    }

    private void Update()
    {
        if (canCheck)
        {
            DetectionPulse();

            float detectionRadius = transform.localScale.x / 2;

            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, _recalLayer);
            foreach (Collider collider in colliders)
            {
                collider.GetComponent<RobotAI>().Recal();
            }
        }
        else
        {
            transform.localScale = _originalDetectionScale;
            _elapsedTime = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 2);
    }
}