using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer_20140222
{
    class Box : Entity
    {

        public Box(Texture2D texture, Vector2 location) : base(texture, location)
        {
            hitbox = GetHitbox(texture, location);
        }
    }
}
