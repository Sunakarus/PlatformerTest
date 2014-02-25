using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer_20140222
{
    class EnemyHopper : Enemy
    {
        public EnemyHopper(Texture2D texture, Vector2 location) : base(texture, location)
        {
            delay = Game1.random.Next(60,150);
        }

        public override void Update()
        {
            base.Update();
            if (delay>0)
            {
                delay--;
            }

            if (delay<=0)
            {
                velocity.Y = -20;
                delay = Game1.random.Next(60, 150);
            }

        }
        
        int delay;
    }
}
