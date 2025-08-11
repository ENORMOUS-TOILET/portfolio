using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


//����������Ҫ���shadow cast��tile, ����λ��������Ӧ��С��shadow cast
public class LoadShadowCastForTIlemap : MonoBehaviour
{
    public Tilemap tilemap; // Ŀ��Tilemap
    public GameObject prefab; // Ҫ���ɵ�Prefab

    void Start()
    {
        if (tilemap == null || prefab == null)
        {
            Debug.LogError("��ȷ��Tilemap��Prefab�ѷ��䣡");
            return;
        }

        // ��ȡTilemap�ı߽�
        BoundsInt bounds = tilemap.cellBounds;

        // �������е�Ԫ��
        foreach (var position in bounds.allPositionsWithin)
        {
            // ��鵱ǰλ���Ƿ���Tile
            TileBase tile = tilemap.GetTile(position);
            if (tile != null)
            {
                // ��Tile����ת��Ϊ��������
                Vector3 worldPosition = tilemap.CellToWorld(position) + tilemap.tileAnchor;

                // ��Tileλ������Prefab
                Instantiate(prefab, worldPosition, Quaternion.identity, transform);
            }
        }
    }
}
