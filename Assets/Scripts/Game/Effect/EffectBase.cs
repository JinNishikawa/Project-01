using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class EffectBase : MonoBehaviour
    {
        private ParticleSystem[] _particles;

        // Start is called before the first frame update
        void Start()
        {
            _particles = transform.GetComponentsInChildren<ParticleSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            int finCount = 0;
            foreach (ParticleSystem particle in _particles)
            {
                if(particle.isStopped && particle.main.loop == false)
                {
                    finCount++;
                }
            }

            if(finCount >= _particles.Length)
            {
                Manager.EffectManager.Instance.DeleteList(gameObject);
            }
        }
    }
}