using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Instantiator : MonoBehaviour
{
    public Tilemap groundTilemap; // 地面Tilemap,用于规划生成敌人的坐标
    [HideInInspector] public List<Vector3> groundTilePositions = new List<Vector3>();
    private GameObject player; // 玩家对象
    private float distanceToPlayer;

    //生成敌人速率控制
    [Header("生成敌人属性")]
    public float enemyhSpawnCoolDown; // 生成敌人的速率
    public float enemyMinSpawnDistanceToPlayer; // 敌人距离玩家的最小距离
    private float enemySpawnTimer; // 下一次生成敌人的时间
    public GameObject[] enemyPrefabs; // 要生成的Prefab
    public int enemyMaxCount; // 最大敌人数量
    public int enemyCount; // 当前敌人数量

    [Header("生成得分物品属性")]
    public float scoreItemSpawnCoolDown; // 生成得分物品的速率
    public float scoreItemMinSpawnDistanceToPlayer; // 距离玩家的最小距离
    private float scoreItemSpawnTimer; // 下一次生成的时间
    public GameObject[] scoreItemPrefabs; // 要生成的Prefab
    public int scoreItemMaxCount; // 最大数量

    [Header("生成弹药物品属性")]
    public float ammoItemSpawnCoolDown; // 生成弹药物品的速率       
    public float ammoItemMinSpawnDistanceToPlayer; // 距离玩家的最小距离
    private float ammoItemSpawnTimer; // 下一次生成的时间
    public GameObject[] ammoItemPrefabs; // 要生成的Prefab
    public int ammoItemMaxCount; // 最大数量

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // 获取玩家对象

        if (groundTilemap == null || enemyPrefabs == null || scoreItemPrefabs == null)
        {
            Debug.LogError("请确保Tilemap和Prefab已分配！");
            return;
        }

        GetGroundTilePositions();// 获取可生成敌人的位置坐标
        enemySpawnTimer = 0;
    }


    private void Update()
    {
        enemySpawnTimer -= Time.deltaTime;
        ammoItemSpawnTimer -= Time.deltaTime;

        if (enemySpawnTimer <= 0 && enemyCount < enemyMaxCount)
        {
            SpawnEnemy();
            GetEnemyCount();
        }

        if(ammoItemSpawnTimer <= 0)
        {
            SpawnAmmoItem();
        }
    }


    //获取可生成物品的位置坐标
    public void GetGroundTilePositions()
    {
        // 获取Tilemap的边界
        BoundsInt bounds = groundTilemap.cellBounds;

        // 遍历所有单元格
        foreach (var position in bounds.allPositionsWithin)
        {
            // 检查当前位置是否有Tile
            TileBase tile = groundTilemap.GetTile(position);
            if (tile != null)
            {
                // 将Tile坐标转换为世界坐标
                Vector3 worldPosition = groundTilemap.CellToWorld(position) + groundTilemap.tileAnchor;
                groundTilePositions.Add(worldPosition);
            }
        }
    }


    // 生成敌人
    public void SpawnEnemy()
    {
        if (groundTilePositions.Count == 0)
        {
            Debug.Log("没有可生成敌人的位置！");
            return;
        }

        // 随机选择一个位置
        int index = Random.Range(0, groundTilePositions.Count);
        Vector3 position = groundTilePositions[index];

        // 计算敌人与玩家的距离
        distanceToPlayer = Vector3.Distance(position, player.transform.position);

        // 距离玩家太近，不生成敌人
        if (distanceToPlayer < enemyMinSpawnDistanceToPlayer)
        {
            return;
        }
        else
        {
            // 随机选择一个Prefab
            int prefabIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemy = Instantiate(enemyPrefabs[prefabIndex],
                position, Quaternion.identity);
            enemySpawnTimer = enemyhSpawnCoolDown; // 重置生成敌人速率
        }
    }

    //计算敌人数量
    public void GetEnemyCount()
    {
        // 查找所有带有指定标签的游戏物体
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");
        // 统计数量
        int count = objectsWithTag.Length;
        // 赋值
        enemyCount = count;
    }


    //生成弹药物品
    public void SpawnAmmoItem()
    {
        if (groundTilePositions.Count == 0)
        {
            Debug.Log("没有可生成敌人的位置！");
            return;
        }

        // 随机选择一个位置
        int index = Random.Range(0, groundTilePositions.Count);
        Vector3 position = groundTilePositions[index];

        // 计算敌人与玩家的距离
        distanceToPlayer = Vector3.Distance(position, player.transform.position);

        // 距离玩家太近，不生成敌人
        if (distanceToPlayer < ammoItemMinSpawnDistanceToPlayer)
        {
            return;
        }
        else
        {
            // 随机选择一个Prefab
            int prefabIndex = Random.Range(0, ammoItemPrefabs.Length);
            GameObject ammoItem = Instantiate(ammoItemPrefabs[prefabIndex],
                position, Quaternion.identity);
            ammoItemSpawnTimer = ammoItemSpawnCoolDown; // 重置生成弹药物品速率
        }
    }

}

