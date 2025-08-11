using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAviodanceBehavior : SteeringBehavoir
{
    [SerializeField]
    private float radius = 2f, agentColliderSize = 0.6f;

    [SerializeField]
    private bool showGizmos = true;

    //gizmos参数
    float[] dangerResultTemp = null;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aidata)
    {
        foreach(Collider2D obstacleCollider in aidata.obstacles)
        {
            Vector2 directionToObstacle 
                = obstacleCollider.ClosestPoint(aidata.transform.position) - (Vector2)aidata.transform.position;
            float distanceToObstacle = directionToObstacle.magnitude;

            //依据敌人与障碍远近判断权重
            float weight = distanceToObstacle < agentColliderSize ? 1f:(radius - distanceToObstacle)/radius;

            Vector2 directionToObstacleNormalized = directionToObstacle.normalized;

            //遍历八个方向，计算每一个方向的避障力
            for(int i = 0; i < Directions.eightDirections.Count; i++)
            {
                float result = Vector2.Dot(directionToObstacleNormalized, Directions.eightDirections[i]);

                float valueToInput = result * weight;

                //当结果大于原数组数据时，替换原数组数据将数据存入数组
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
