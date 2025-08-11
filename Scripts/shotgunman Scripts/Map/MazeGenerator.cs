using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    public Tilemap tilemap; // Tilemap���
    public TileBase groundTile; // ������Ƭ
    public TileBase wallTile; // ǽ��Ƭ
    public GameObject monsterPrefab; // ����Ԥ����
    public GameObject collectiblePrefab; // ���ռ�����Ԥ����

    public Vector2Int mazeSize = new Vector2Int(10, 10); // �Թ��Ĵ�С
    public int numberOfCollectibles = 5; // ���ռ���������

    private bool[,] maze; // �洢�Թ��Ĳ���

    private void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        maze = new bool[mazeSize.x, mazeSize.y]; // ��ʼ���Թ�����

        // �Ȱ�����λ�ö�����Ϊǽ
        for (int x = 0; x < mazeSize.x; x++)
        {
            for (int y = 0; y < mazeSize.y; y++)
            {
                maze[x, y] = false; // false��ʾǽ
            }
        }

        // ʹ��DFS�㷨�����Թ�
        GenerateMazeDFS(1, 1);

        // �����ɵ��Թ�ת����Tilemap
        DrawMaze();

        // �����λ�����ɹ���
        SpawnMonsters();

        // �����λ�����ɿ��ռ�����
        SpawnCollectibles();
    }

    // ʹ��DFS�㷨�����Թ�
    void GenerateMazeDFS(int x, int y)
    {
        maze[x, y] = true; // �ѵ�ǰ������Ϊͨ��

        // �����ĸ�����
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1), // ��
            new Vector2Int(1, 0), // ��
            new Vector2Int(0, -1), // ��
            new Vector2Int(-1, 0) // ��
        };

        // ��������ĸ������˳��
        Shuffle(directions);

        // ���Դ�ÿ������ʼ��
        foreach (var dir in directions)
        {
            int nx = x + dir.x * 2; // ����һ������
            int ny = y + dir.y * 2;

            // �ж���λ���Ƿ����Թ��ڲ��һ�δ������
            if (nx > 0 && ny > 0 && nx < mazeSize.x - 1 && ny < mazeSize.y - 1 && !maze[nx, ny])
            {
                maze[nx, ny] = true; // �����µ�ͨ��
                maze[x + dir.x, y + dir.y] = true; // ����ǽ��ͨ��֮�������
                GenerateMazeDFS(nx, ny); // �ݹ�����
            }
        }
    }

    // ���ҷ�������
    void Shuffle(Vector2Int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int j = Random.Range(i, array.Length);
            var temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    // �����ɵ��Թ�ת��ΪTilemap
    void DrawMaze()
    {
        for (int x = 0; x < mazeSize.x; x++)
        {
            for (int y = 0; y < mazeSize.y; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                if (maze[x, y]) // true��ʾͨ��������
                {
                    tilemap.SetTile(position, groundTile);
                }
                else // false��ʾǽ
                {
                    tilemap.SetTile(position, wallTile);
                }
            }
        }
    }

    void SpawnMonsters()
    {
        // ���ɹ����λ��
        int monsterCount = Random.Range(1, 4); // �������1��3������
        for (int i = 0; i < monsterCount; i++)
        {
            int randomX = Random.Range(1, mazeSize.x - 1); // �ų��߽�
            int randomY = Random.Range(1, mazeSize.y - 1); // �ų��߽�

            Vector3Int position = new Vector3Int(randomX, randomY, 0);
            if (tilemap.GetTile(position) == groundTile) // ȷ������ֻ�����ڵ�����
            {
                Instantiate(monsterPrefab, tilemap.CellToWorld(position), Quaternion.identity);
            }
            else
            {
                i--; // �������λ�ò����ʣ����³���
            }
        }
    }

    void SpawnCollectibles()
    {
        // ���ɿ��ռ������λ��
        for (int i = 0; i < numberOfCollectibles; i++)
        {
            int randomX = Random.Range(1, mazeSize.x - 1); // �ų��߽�
            int randomY = Random.Range(1, mazeSize.y - 1); // �ų��߽�

            Vector3Int position = new Vector3Int(randomX, randomY, 0);
            if (tilemap.GetTile(position) == groundTile) // ȷ�����������ڵ�����
            {
                Instantiate(collectiblePrefab, tilemap.CellToWorld(position), Quaternion.identity);
            }
            else
            {
                i--; // �������λ�ò����ʣ����³���
            }
        }
    }
}
