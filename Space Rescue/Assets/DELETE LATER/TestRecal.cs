using UnityEngine;

public class TestRecal : MonoBehaviour
{
    [SerializeField] LayerMask _recalLayer;

    public bool canCheck;

    [SerializeField] float _startScale;
    [SerializeField] float _maxScale;

    [SerializeField] float _scaleSpeed;

    [SerializeField] Vector3 _originalDetectionScale;

    private float _elapsedTime;  // New variable to track the time for scaling

    private void Start()
    {
        _originalDetectionScale = transform.localScale;
        _elapsedTime = 0f;  // Initialize elapsed time to 0
    }

    public void DetectionPulse()
    {
        _elapsedTime += Time.deltaTime * _scaleSpeed;  // Increment elapsed time based on scale speed

        // Calculate scale based on elapsed time and ensure it stays within bounds
        float scale = Mathf.Lerp(_startScale, _maxScale, _elapsedTime);
        scale = Mathf.Min(scale, _maxScale);  // Clamp the scale to the max value

        // Apply the calculated scale, keeping the y-axis fixed at 0.25f
        transform.localScale = _originalDetectionScale * scale;
        transform.localScale = new Vector3(transform.localScale.x, 0.25f, transform.localScale.z);
    }

    private void Update()
    {
        if (canCheck)
        {
            DetectionPulse();

            // Use the current X scale of the object as the detection radius for OverlapSphere
            float detectionRadius = transform.localScale.x / 2;

            // Use the dynamic detection radius for the OverlapSphere
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, _recalLayer);
            foreach (Collider collider in colliders)
            {
                collider.GetComponent<RobotAI>().ChangeState(RobotAI.State.FOLLOW);
            }
        }
        else
        {
            // Reset the scale and elapsed time when canCheck is false
            transform.localScale = _originalDetectionScale;
            _elapsedTime = 0f;  // Reset the elapsed time to start from _startScale next time
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 2);
    }
}