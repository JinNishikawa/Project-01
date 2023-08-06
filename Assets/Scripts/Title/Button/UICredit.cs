using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
    public class UICredit : UIButtonBase
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        public override void OnClick()
        {
            Debug.Log(GetType().Name + "ƒNƒŠƒbƒN");
        }
    }
}