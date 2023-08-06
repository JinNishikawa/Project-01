using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFieldPosition : ActionNode
{
    public bool _isMyRange;

    Card.Card _card = null;

    bool _isRange;
    Vector3 _fieldPosition;

    protected override void OnStart()
    {
        _card = blackboard._card;
        if (_card == null) return;

        int tileType = (int)Field.Battle.TileType.Select;

        int battleTileType = (int)Field.Battle.TileType.Battle;

        int checkTile = (int)Field.Battle.TileType.P1Check;
        if (_card._info._userType == Manager.UserType.Player2)
        {
            checkTile = (int)Field.Battle.TileType.P2Check;
        }

        _fieldPosition = TilemapFunction.RandomPosition(Field.Battle.Instance._tiles[checkTile]);
        if(_isMyRange)
        {
            _fieldPosition = TilemapFunction.RandomRangePosition(Field.Battle.Instance._tiles[checkTile]);
        }

        float tileScale = Field.Battle.Instance._tileScale;

        _isRange = true;
        // 判定
        foreach (GameObject obj in _card.GetSoldiersList())
        {
            Vector3 pos = _fieldPosition + obj.transform.localPosition * _card._info._moveDir * tileScale;

            // 設置フラグ
            bool isSetting = TilemapFunction.HasTile(Field.Battle.Instance._tiles[battleTileType], pos);
            // 範囲フラグ
            bool isRange = TilemapFunction.IsRange(Field.Battle.Instance._tiles[tileType], pos);
            // 設置範囲
            bool isMyRange = TilemapFunction.HasTile(Field.Battle.Instance._tiles[checkTile], pos);

            // 範囲内 & 未設置
            if (isRange && !isSetting && isMyRange) continue;

            _isRange = false;

            break;
        }


    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (_card == null)
        {
            return State.Failure;
        }

        if(!_isRange)
        {
            return State.Failure;
        }

        blackboard._fieldPosition = _fieldPosition;

        return State.Success;
    }
}
