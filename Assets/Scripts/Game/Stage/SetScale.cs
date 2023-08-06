using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    public class SetScale : MonoBehaviour
    {
        public void SetScaleRate(float rate)
        {
            transform.localScale = Vector3.one * rate;

            Destroy(this);
        }

        public void SetScaleY(float y)
        {
            Vector3 scale = transform.localScale;
            scale.y = y;
            transform.localScale = scale;

            Destroy(this);
        }
    }
}