using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekBehavior : SteeringBehavoir
{
    [SerializeField]
    private float targetReachedThreshold = 0.5f;

    [SerializeField]
    private bool showGizmo = true;

    bool reachedLastTarget = true;

    //gizmos����
    private Vector2 targetPositionCached;
    private float[] interestsTemp;
    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aidata)
    {
        if (reachedLastTarget)
        {
            if(aidata.targets == null || aidata.targets.Count <= 0)
            {
                aidata.currentTarget = null;
                return (danger, interest);
            }
            else
            {
                reachedLastTarget = false;
                aidata.currentTarget = aidata.targets.OrderBy
                    (target => Vector2.Distance(transform.position, target.position)).First();
            }
        }

        //����Ŀ��λ�û���
        if(aidata.currentTarget != null && aidata.targets != null&& aidata.targets.Contains(aidata.currentTarget))
            targetPositionCached = aidata.currentTarget.position;

        //����Ƿ񵽴�Ŀ��
        if (Vector2.Distance(transform.position, targetPositionCached) < targetReachedThreshold)
        {
            reachedLastTarget = true;
            aidata.currentTarget = null;
            return (danger, interest);
        }

        //���û�е���Ŀ�꣬�������Ȥ���򲢷���
        Vector2 directionToTarget = targetPositionCached - (Vector2)transform.position;
        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);

            if (result > 0)
            {
                float valueToPutIn = result;
                if(valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }
            }
        }
        interestsTemp = interest;
        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo)
            return;

        Gizmos.DrawSphere(targetPositionCached, 0.2f);

        if(Application.isPlaying && interestsTemp != null)
        {
            if(interestsTemp != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < interestsTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i]);
                }
                if(reachedLastTarget == false)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(targetPositionCached, 0.1f);
                }
            }
        }
    }


    public static class Directions
    {
        public static List<Vector2> eightDirections = new List<Vector2>()
        {
            new Vector2(1, 0).normalized,
            new Vector2(1, 1).normalized,
            new Vector2(0, 1).normalized,
            new Vector2(-1, 1).normalized,
            new Vector2(-1, 0).normalized,
            new Vector2(-1, -1).normalized,
            new Vector2(0, -1).normalized,
            new Vector2(1, -1).normalized
        };
    }
}
