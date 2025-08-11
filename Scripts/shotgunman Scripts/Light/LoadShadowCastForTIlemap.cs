using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


//遍历所有需要添加shadow cast的tile, 在其位置生成相应大小的shadow cast
public class LoadShadowCastForTIlemap : MonoBehaviour
{
    public Tilemap tilemap; // 目标Tilemap
    public GameObject prefab; // 要生成的Prefab

    void Start()
    {
        if (tilemap == null || prefab == null)
        {
            Debug.LogError("请确保Tilemap和Prefab已分配！");
            return;
        }

        // 获取Tilemap的边界
        BoundsInt bounds = tilemap.cellBounds;

        // 遍历所有单元格
        foreach (var position in bounds.allPositionsWithin)
        {
            // 检查当前位置是否有Tile
            TileBase tile = tilemap.GetTile(position);
            if (tile != null)
            {
                // 将Tile坐标转换为世界坐标
                Vector3 worldPosition = tilemap.CellToWorld(position) + tilemap.tileAnchor;

                // 在Tile位置生成Prefab
                Instantiate(prefab, worldPosition, Quaternion.identity, transform);
            }
        }
    }
}
