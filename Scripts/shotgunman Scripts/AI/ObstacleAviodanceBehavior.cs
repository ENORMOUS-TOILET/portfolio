using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAviodanceBehavior : SteeringBehavoir
{
    [SerializeField]
    private float radius = 2f, agentColliderSize = 0.6f;

    [SerializeField]
    private bool showGizmos = true;

    //gizmos����
    float[] dangerResultTemp = null;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aidata)
    {
        foreach(Collider2D obstacleCollider in aidata.obstacles)
        {
            Vector2 directionToObstacle 
                = obstacleCollider.ClosestPoint(aidata.transform.position) - (Vector2)aidata.transform.position;
            float distanceToObstacle = directionToObstacle.magnitude;

            //���ݵ������ϰ�Զ���ж�Ȩ��
            float weight = distanceToObstacle < agentColliderSize ? 1f:(radius - distanceToObstacle)/radius;

            Vector2 directionToObstacleNormalized = directionToObstacle.normalized;

            //�����˸����򣬼���ÿһ������ı�����
            for(int i = 0; i < Directions.eightDirections.Count; i++)
            {
                float result = Vector2.Dot(directionToObstacleNormalized, Directions.eightDirections[i]);

                float valueToInput = result * weight;

                //���������ԭ��������ʱ���滻ԭ�������ݽ����ݴ�������
                if(danger[i] < valueToInput)
                {
                    danger[i] = valueToInput;
                }
            }
        }
        dangerResultTemp = danger;



        return (danger, interest);


    }


    private void OnDrawGizmos()
    {
        if(!showGizmos)
            return;

        if (Application.isPlaying && dangerResultTemp != null)
        {
            if (dangerResultTemp != null)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < dangerResultTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * dangerResultTemp[i]);
                }
            }
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
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
