using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    public class Deck : State
    {
        public override void Awake()
        {
            base.Awake();

            GetComponent<BoxCollider>().enabled = true;

            transform.rotation = Quaternion.Euler(-90, 0, 0);

            _card.InitFlag();
        }

        public override void Start()
        {
            base.Start();

            // ƒŠƒXƒg‚Ö’Ç‰Á
            _characterInfo._deck.AddCardList(gameObject);
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

        public override void Fin()
        {
            base.Fin();
        }
    }
}