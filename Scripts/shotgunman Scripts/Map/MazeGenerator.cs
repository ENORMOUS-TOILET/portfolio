using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    public Tilemap tilemap; // Tilemap组件
    public TileBase groundTile; // 地面瓦片
    public TileBase wallTile; // 墙瓦片
    public GameObject monsterPrefab; // 怪物预制体
    public GameObject collectiblePrefab; // 可收集物体预制体

    public Vector2Int mazeSize = new Vector2Int(10, 10); // 迷宫的大小
    public int numberOfCollectibles = 5; // 可收集物体数量

    private bool[,] maze; // 存储迷宫的布局

    private void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        maze = new bool[mazeSize.x, mazeSize.y]; // 初始化迷宫布局

        // 先把所有位置都设置为墙
        for (int x = 0; x < mazeSize.x; x++)
        {
            for (int y = 0; y < mazeSize.y; y++)
            {
                maze[x, y] = false; // false表示墙
            }
        }

        // 使用DFS算法生成迷宫
        GenerateMazeDFS(1, 1);

        // 把生成的迷宫转换成Tilemap
        DrawMaze();

        // 在随机位置生成怪物
        SpawnMonsters();

        // 在随机位置生成可收集物体
        SpawnCollectibles();
    }

    // 使用DFS算法生成迷宫
    void GenerateMazeDFS(int x, int y)
    {
        maze[x, y] = true; // 把当前格子设为通道

        // 定义四个方向
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1), // 上
            new Vector2Int(1, 0), // 右
            new Vector2Int(0, -1), // 下
            new Vector2Int(-1, 0) // 左
        };

        // 随机打乱四个方向的顺序
        Shuffle(directions);

        // 尝试从每个方向开始走
        foreach (var dir in directions)
        {
            int nx = x + dir.x * 2; // 跳过一个格子
            int ny = y + dir.y * 2;

            // 判断新位置是否在迷宫内并且还未被访问
            if (nx > 0 && ny > 0 && nx < mazeSize.x - 1 && ny < mazeSize.y - 1 && !maze[nx, ny])
            {
                maze[nx, ny] = true; // 设置新的通道
                maze[x + dir.x, y + dir.y] = true; // 创建墙和通道之间的连接
                GenerateMazeDFS(nx, ny); // 递归生成
            }
        }
    }

    // 打乱方向数组
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

    // 将生成的迷宫转换为Tilemap
    void DrawMaze()
    {
        for (int x = 0; x < mazeSize.x; x++)
        {
            for (int y = 0; y < mazeSize.y; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                if (maze[x, y]) // true表示通道，地面
                {
                    tilemap.SetTile(position, groundTile);
                }
                else // false表示墙
                {
                    tilemap.SetTile(position, wallTile);
                }
            }
        }
    }

    void SpawnMonsters()
    {
        // 生成怪物的位置
        int monsterCount = Random.Range(1, 4); // 随机生成1到3个怪物
        for (int i = 0; i < monsterCount; i++)
        {
            int randomX = Random.Range(1, mazeSize.x - 1); // 排除边界
            int randomY = Random.Range(1, mazeSize.y - 1); // 排除边界

            Vector3Int position = new Vector3Int(randomX, randomY, 0);
            if (tilemap.GetTile(position) == groundTile) // 确保怪物只生成在地面上
            {
                Instantiate(monsterPrefab, tilemap.CellToWorld(position), Quaternion.identity);
            }
            else
            {
                i--; // 如果生成位置不合适，重新尝试
            }
        }
    }

    void SpawnCollectibles()
    {
        // 生成可收集物体的位置
        for (int i = 0; i < numberOfCollectibles; i++)
        {
            int randomX = Random.Range(1, mazeSize.x - 1); // 排除边界
            int randomY = Random.Range(1, mazeSize.y - 1); // 排除边界

            Vector3Int position = new Vector3Int(randomX, randomY, 0);
            if (tilemap.GetTile(position) == groundTile) // 确保物体生成在地面上
            {
                Instantiate(collectiblePrefab, tilemap.CellToWorld(position), Quaternion.identity);
            }
            else
            {
                i--; // 如果生成位置不合适，重新尝试
            }
        }
    }
}
