using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFunction
{
    static public bool IsRange(Tilemap tilemap ,Vector3 worldPos)
    {
        // セル座標取得
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);

        Vector3Int maxRange = new Vector3Int(tilemap.cellBounds.xMax, tilemap.cellBounds.yMax, tilemap.cellBounds.zMax);
        Vector3Int minRange = new Vector3Int(tilemap.cellBounds.xMin, tilemap.cellBounds.yMin, tilemap.cellBounds.zMin);

        // 範囲判定
        if ((minRange.x <= cellPos.x && cellPos.x < maxRange.x) && (minRange.y <= cellPos.y && cellPos.y < maxRange.y))
        {
            return true;
        }

        return false;
    }

    static public Vector3 GetCellsWorldPos(Tilemap tilemap, Vector3 pos)
    {
        // セル座標取得
        Vector3Int cellPos = tilemap.WorldToCell(pos);

        // セルのワールド座標取得
        pos = tilemap.GetCellCenterWorld(cellPos);

        return pos;
    }

    static public Vector3Int GetCellsPos(Tilemap tilemap, Vector3 worldPos)
    {
        return tilemap.WorldToCell(worldPos);
    }

    static public Vector3 GetWorldPos(Tilemap tilemap, Vector3Int cellPos)
    {
        // セルのワールド座標取得
        return tilemap.GetCellCenterWorld(cellPos);
    }

    static public void SetClearAllTile(Tilemap tilemap)
    {
        BoundsInt bound = tilemap.cellBounds;

        for (int y = bound.max.y - 1; y >= bound.min.y; --y)
        {
            for (int x = bound.min.x; x < bound.max.x; ++x)
            {
                var pos = new Vector3Int(x, y, 0);
                tilemap.SetTile(pos, null);

            }
        }
    }

    static public void SetTile(Tilemap tilemap, Vector3 worldPos, TileBase tile = null)
    {
        // セル座標取得
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        tilemap.SetTile(cellPos, tile);
    }

    static public void SetSpriteColor(Tilemap tilemap, Color color)
    {
        tilemap.color = color;
    }

    static public void SetTileColor(Tilemap tilemap, Vector3 worldPos, Color color)
    {
        Vector3Int cellPos = GetCellsPos(tilemap, worldPos);
        tilemap.SetColor(cellPos, color);
    }


    static public void SetTileFlagNone(Tilemap tilemap, Vector3 worldPos, TileFlags flag)
    {
        Vector3Int cellPos = GetCellsPos(tilemap, worldPos);
        tilemap.SetTileFlags(cellPos, flag);
    }

    static public bool HasTile(Tilemap tilemap, Vector3 worldPos)
    {
        Vector3Int cellPos = GetCellsPos(tilemap, worldPos);
        return tilemap.HasTile(cellPos);
    }

    static public Vector3 RandomPosition(Tilemap tilemap)
    {
        BoundsInt bound = tilemap.cellBounds;

        int x = Random.Range(bound.min.x, bound.max.x);
        int y = Random.Range(bound.min.y, bound.max.y);
        int z = Random.Range(bound.min.z, bound.max.z);

        Vector3Int cellPos = new Vector3Int(x, y, z);

        Vector3 worldPos = GetWorldPos(tilemap, cellPos);

        return worldPos;
    }

    static public Vector3 RandomRangePosition(Tilemap tilemap)
    {
        BoundsInt bound = tilemap.cellBounds;

        int maxX = bound.min.x;
        int minX = bound.max.x;
        for (int y = bound.max.y - 1; y >= bound.min.y; --y)
        {
            for (int x = bound.min.x; x < bound.max.x; ++x)
            {
                var pos = new Vector3Int(x, y, 0);
                if (!tilemap.HasTile(pos)) continue;
                if (pos.x > maxX)
                {
                    maxX = pos.x;
                }

                if (pos.x < minX)
                {
                    minX = pos.x;
                }
            }
        }

        int randX = Random.Range(minX, maxX);
        int randY = Random.Range(bound.min.y, bound.max.y);
        int randZ = Random.Range(bound.min.z, bound.max.z);

        Vector3Int cellPos = new Vector3Int(randX, randY, randZ);

        Vector3 worldPos = GetWorldPos(tilemap, cellPos);

        return worldPos;
    }
}
