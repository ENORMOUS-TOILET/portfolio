using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField]
    private float targetDetectionRadius = 5;

    [SerializeField]
    private LayerMask obstacleLayerMaskm, playerLayerMask;

    [SerializeField]
    private bool showGizmos = true;

    private List<Transform> colliders;

    public override void Detect(AIData aidata)
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, targetDetectionRadius, playerLayerMask);

        if (playerCollider != null)
        {
            //����Ƿ񿴼����
            Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, targetDetectionRadius, obstacleLayerMaskm);

            //ȷ���ڡ�Player��layer�Ͽ���collider
            if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.DrawRay(transform.position, direction * targetDetectionRadius, Color.magenta);
                colliders = new List<Transform>() { playerCollider.transform };
            }
            else
            {
                colliders = null;
            }
        }
        else
        {
            //����δ�������
            colliders = null;
        }
        aidata.targets = colliders;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos == false)
            return;

        Gizmos.DrawWireSphere(transform.position, targetDetectionRadius);

        if (colliders == null)
            return;
        Gizmos.color = Color.magenta;
        foreach(var item in colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }
}
