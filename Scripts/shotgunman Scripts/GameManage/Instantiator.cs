using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Instantiator : MonoBehaviour
{
    public Tilemap groundTilemap; // ����Tilemap,���ڹ滮���ɵ��˵�����
    [HideInInspector] public List<Vector3> groundTilePositions = new List<Vector3>();
    private GameObject player; // ��Ҷ���
    private float distanceToPlayer;

    //���ɵ������ʿ���
    [Header("���ɵ�������")]
    public float enemyhSpawnCoolDown; // ���ɵ��˵�����
    public float enemyMinSpawnDistanceToPlayer; // ���˾�����ҵ���С����
    private float enemySpawnTimer; // ��һ�����ɵ��˵�ʱ��
    public GameObject[] enemyPrefabs; // Ҫ���ɵ�Prefab
    public int enemyMaxCount; // ����������
    public int enemyCount; // ��ǰ��������

    [Header("���ɵ÷���Ʒ����")]
    public float scoreItemSpawnCoolDown; // ���ɵ÷���Ʒ������
    public float scoreItemMinSpawnDistanceToPlayer; // ������ҵ���С����
    private float scoreItemSpawnTimer; // ��һ�����ɵ�ʱ��
    public GameObject[] scoreItemPrefabs; // Ҫ���ɵ�Prefab
    public int scoreItemMaxCount; // �������

    [Header("���ɵ�ҩ��Ʒ����")]
    public float ammoItemSpawnCoolDown; // ���ɵ�ҩ��Ʒ������       
    public float ammoItemMinSpawnDistanceToPlayer; // ������ҵ���С����
    private float ammoItemSpawnTimer; // ��һ�����ɵ�ʱ��
    public GameObject[] ammoItemPrefabs; // Ҫ���ɵ�Prefab
    public int ammoItemMaxCount; // �������

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // ��ȡ��Ҷ���

        if (groundTilemap == null || enemyPrefabs == null || scoreItemPrefabs == null)
        {
            Debug.LogError("��ȷ��Tilemap��Prefab�ѷ��䣡");
            return;
        }

        GetGroundTilePositions();// ��ȡ�����ɵ��˵�λ������
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


    //��ȡ��������Ʒ��λ������
    public void GetGroundTilePositions()
    {
        // ��ȡTilemap�ı߽�
        BoundsInt bounds = groundTilemap.cellBounds;

        // �������е�Ԫ��
        foreach (var position in bounds.allPositionsWithin)
        {
            // ��鵱ǰλ���Ƿ���Tile
            TileBase tile = groundTilemap.GetTile(position);
            if (tile != null)
            {
                // ��Tile����ת��Ϊ��������
                Vector3 worldPosition = groundTilemap.CellToWorld(position) + groundTilemap.tileAnchor;
                groundTilePositions.Add(worldPosition);
            }
        }
    }


    // ���ɵ���
    public void SpawnEnemy()
    {
        if (groundTilePositions.Count == 0)
        {
            Debug.Log("û�п����ɵ��˵�λ�ã�");
            return;
        }

        // ���ѡ��һ��λ��
        int index = Random.Range(0, groundTilePositions.Count);
        Vector3 position = groundTilePositions[index];

        // �����������ҵľ���
        distanceToPlayer = Vector3.Distance(position, player.transform.position);

        // �������̫���������ɵ���
        if (distanceToPlayer < enemyMinSpawnDistanceToPlayer)
        {
            return;
        }
        else
        {
            // ���ѡ��һ��Prefab
            int prefabIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemy = Instantiate(enemyPrefabs[prefabIndex],
                position, Quaternion.identity);
            enemySpawnTimer = enemyhSpawnCoolDown; // �������ɵ�������
        }
    }

    //�����������
    public void GetEnemyCount()
    {
        // �������д���ָ����ǩ����Ϸ����
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");
        // ͳ������
        int count = objectsWithTag.Length;
        // ��ֵ
        enemyCount = count;
    }


    //���ɵ�ҩ��Ʒ
    public void SpawnAmmoItem()
    {
        if (groundTilePositions.Count == 0)
        {
            Debug.Log("û�п����ɵ��˵�λ�ã�");
            return;
        }

        // ���ѡ��һ��λ��
        int index = Random.Range(0, groundTilePositions.Count);
        Vector3 position = groundTilePositions[index];

        // �����������ҵľ���
        distanceToPlayer = Vector3.Distance(position, player.transform.position);

        // �������̫���������ɵ���
        if (distanceToPlayer < ammoItemMinSpawnDistanceToPlayer)
        {
            return;
        }
        else
        {
            // ���ѡ��һ��Prefab
            int prefabIndex = Random.Range(0, ammoItemPrefabs.Length);
            GameObject ammoItem = Instantiate(ammoItemPrefabs[prefabIndex],
                position, Quaternion.identity);
            ammoItemSpawnTimer = ammoItemSpawnCoolDown; // �������ɵ�ҩ��Ʒ����
        }
    }

}

