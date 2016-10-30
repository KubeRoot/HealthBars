using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SGUI;

namespace Healthbars
{
    class DamageLabelModifier : SModifier
    {
        private string text;
        private Color outlineCol;

        public DamageLabelModifier(string v, Color col)
        {
            text = v;
            outlineCol = col;
        }

        public override void Init()
        {
            base.Init();

            for (float x = 0; x <= 2; x += 1)
            {
                for (float y = 0; y <= 2; y += 1)
                {
                    new SLabel(text)
                    {
                        Parent = Elem,
                        Position = new Vector2(x, y),
                        Foreground = outlineCol,

                        With = {
                            new SFadeOutAnimation(1.3f)
                        }
                    };
                }
            }

            new SLabel(text)
            {
                Parent = Elem,
                Position = new Vector2(1, 1),

                With = {
                    new SFadeOutAnimation(1.3f)
                }
            };

            Elem.Foreground = new Color(0, 0, 0, 0);
            Elem.Background = new Color(0, 0, 0, 0);
        }
    }

    class FloatAwayAnimation : SAnimation
    {
        Vector2 Position, Direction;

        public FloatAwayAnimation(float duration, Vector2 pos, Vector2 dir) : base(duration)
        {
            Position = pos;
            Direction = dir;
        }

        public override void Animate(float t)
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(Position).XY();
            pos.y = Screen.height - pos.y + 10;

            Elem.Position = pos - Elem.Size / 2 + Direction * t;
        }

        public override void OnStart()
        {
            Elem.Position = Position - Elem.Size/2;
        }

        public override void OnEnd()
        {
            base.OnEnd();

            Elem.Remove();
        }
    }
}
