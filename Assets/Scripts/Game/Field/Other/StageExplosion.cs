using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    public class StageExplosion : MonoBehaviour
    {
        private float _timer = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
            _timer = 0.5f;

            GameObject[] cardObj = GameObject.FindGameObjectsWithTag("Card");
            foreach (GameObject obj in cardObj)
            {
                Card.Card card = obj.GetComponent<Card.Card>();
                if (card)
                {
                    card.DestroyState();
                    card._isGame = false;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            float time = Time.deltaTime * Manager.GameMgr.Instance._gameSpeed;
            _timer -= time;
            if(_timer < 0.0f)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Explosion(other.gameObject);
        }

        private void Explosion(GameObject target)
        {
            ExplosionStage(target);

            ExplosionSoldier(target);
        }

        private void ExplosionStage(GameObject target)
        {
            if (target.tag != "StageBlock") return;

            target.AddComponent<RemoveLine>();

            Rigidbody targetRb = target.GetComponentInParent<Rigidbody>();
            if (targetRb == null) return;

            float pow = 1000.0f;
            float radius = transform.localScale.x;

            targetRb.AddExplosionForce(pow, transform.position, radius, 1.0f);
        }

        private void ExplosionSoldier(GameObject target)
        {
            if (target.tag != "Soldier") return;

            Rigidbody targetRb = target.GetComponentInParent<Rigidbody>();
            if (targetRb == null) return;

            float pow = 1000.0f;
            float radius = transform.localScale.x;

            target.GetComponentInParent<Soldier.Soldier>()?.DeleteStorePos();

            targetRb.AddExplosionForce(pow, transform.position, radius, 1.0f);
        }
    }
}