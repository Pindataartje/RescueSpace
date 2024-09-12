using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRecal : MonoBehaviour
{
    [SerializeField] LayerMask _recalLayer;

    public bool canCheck;

    private void Update()
    {
        if (canCheck)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, _recalLayer);

            foreach (Collider collider in colliders)
            {
                collider.GetComponent<RobotAI>().ChangeState(RobotAI.State.FOLLOW);
            }
        }
    }
}
