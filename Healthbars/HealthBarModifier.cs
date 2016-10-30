using Dungeonator;
using SGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Healthbars
{
    class HealthBar : SModifier
    {
        private HealthHaver m_HealthHaver;
        private BraveBehaviour m_parent;
        private SRect m_interior;
        private SRect m_interiorbg;
        private Vector2 m_offset = new Vector2(0, 8);

        public HealthBar(BraveBehaviour parent)
        {
            m_parent = parent;
            m_HealthHaver = m_parent.healthHaver;
        }

        public override void Init()
        {
            m_interiorbg = new SRect(new Color(0.8f, 0.8f, 0.8f, 0.4f))
            {
                Parent = Elem,
                Size = Elem.InnerSize
            };

            m_interior = new SRect(new Color(0.9f, 0.1f, 0.1f, 0.7f))
            {
                Parent = Elem,
                Size = Elem.InnerSize,
                With =
                {
                    new HealthBarInterior(m_HealthHaver)
                }
            };

            m_offset -= Elem.Size / 2f;
        }

        public override void Update()
        {
            if (m_parent == null)
            {
                Elem.Remove();
                return;
            }

            RoomHandler room;
            try
            {
                room = m_parent.transform.position.GetAbsoluteRoom();
            }
            catch
            {
                room = null;
            }

            if (GameManager.Instance.IsPaused || room == null || (room.visibility == RoomHandler.VisibilityStatus.OBSCURED || room.visibility == RoomHandler.VisibilityStatus.REOBSCURED))
            {
                Elem.Visible = false;
                return;
            }
            else
                Elem.Visible = true;

            Vector2 pos = Camera.main.WorldToScreenPoint(m_parent.sprite.WorldBottomCenter).XY();
            pos.y = Screen.height - pos.y;
            Elem.Position = pos + m_offset;
        }

        private class HealthBarInterior : SModifier
        {
            private HealthHaver m_HealthHaver;
            private float healthPercent;

            public HealthBarInterior(HealthHaver health)
            {
                m_HealthHaver = health;
                m_HealthHaver.OnHealthChanged += OnHealthChanged;
                healthPercent = 1;
            }

            private void OnHealthChanged(float current, float max)
            {
                healthPercent = current / max;
            }

            public override void Update()
            {
                if (m_HealthHaver == null)
                {
                    Elem.Parent.Remove();
                    return;
                }

                Elem.Size.x = Elem.Parent.InnerSize.x * healthPercent;
            }

            public override void UpdateStyle()
            {
                Elem.Size.y = Elem.Parent.InnerSize.y;
            }
        }
    }
}
