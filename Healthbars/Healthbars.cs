using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SGUI;
using Dungeonator;

namespace Healthbars
{
    class HealthBarBehavior : MonoBehaviour
    {
        BraveBehaviour parent;
        SGroup bar;

        private void OnDeath(Vector2 _)
        {
            bar.Remove();
        }

        public void Start()
        {
            parent = GetComponent<BraveBehaviour>();

            bar = new SGroup()
            {
                Size = new Vector2(50, 10),
                With =
                {
                    new HealthBar(parent)
                },
                Border = 2f
            };

            parent.healthHaver.OnDeath += OnDeath;
        }
    }

    public class Healthbars : ETGModule
    {
        private void OnNewEnemy(Component component)
        {
            if(component.GetComponent<HealthBarBehavior>() == null)
                component.gameObject.AddComponent<HealthBarBehavior>();
        }

        public override void Start()
        {
            ETGMod.Objects.AddHook<AIActor>(OnNewEnemy);
            ETGMod.Objects.AddHook<PlayerController>(OnNewEnemy);
        }

        public override void Init()
        {
        }

        public override void Exit()
        {
        }
    }
}
