using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public class Ready : State
    {
        public override void Awake()
        {
            base.Awake();
            if (_soldier._info._userType == Manager.UserType.Player1)
            {
                if (_soldier is SoldierVisual)
                {
                    SoldierVisual sv = (SoldierVisual)_soldier;
                    sv.SetVisibleSprite(false);
                }
                else
                {
                    _soldier.SetVisibleSprite(SoldierMesh.Character, false);
                    _soldier.SetVisibleSprite(SoldierMesh.Field, false);
                }

                _soldier.SetRotateCharacter(180.0f);
            }
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

        public override void Fin()
        {
            base.Fin();
            if (_soldier._info._userType == Manager.UserType.Player1)
            {
                if (!_soldier._info._isOnlyVisual)
                {
                    _soldier.SetVisibleSprite(SoldierMesh.Character, true);
                    _soldier.SetVisibleSprite(SoldierMesh.Field, false);
                }
                else
                {
                    if (_soldier is SoldierVisual)
                    {
                        SoldierVisual sv = (SoldierVisual)_soldier;
                        sv.SetVisibleSprite(true);
                    }
                    else
                    {
                        _soldier.SetVisibleSprite(SoldierMesh.Character, false);
                        _soldier.SetVisibleSprite(SoldierMesh.Field, true);
                    }
                }
            }
            else
            {
                if (!_soldier._info._isOnlyVisual)
                {
                    _soldier.SetVisibleSprite(SoldierMesh.Field, false);
                }
                else
                {
                    if (_soldier is SoldierVisual)
                    {
                        SoldierVisual sv = (SoldierVisual)_soldier;
                        sv.SetVisibleSprite(true);
                    }
                    else
                    {
                        _soldier.SetVisibleSprite(SoldierMesh.Character, false);
                        _soldier.SetVisibleSprite(SoldierMesh.Field, false);
                    }
                }
            }
        }
    }

}