using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer_20140222
{
    class Entity
    {
        
        public Texture2D texture;
        public Vector2 location, velocity = Vector2.Zero;
        public bool isRight = true;
        public bool grounded = false;
        public readonly Vector2 gravity = new Vector2(0, 0.981f);
        public Rectangle hitbox;
        public float boxY;

        public Entity(Texture2D texture, Vector2 location)
        {
            this.texture = texture;
            this.location = location;
        }

        public List<Box> boxList = new List<Box>();

        public Rectangle GetHitbox(Texture2D texture, Vector2 location)
        {
            return new Rectangle((int)location.X, (int)location.Y, texture.Width, texture.Height);
        }

        public bool GetGroundCollision(Rectangle rec, out float boxY)
        {
            foreach (Box box in boxList)
            {
                if (rec.Intersects(box.hitbox))
                {
                    boxY = box.location.Y;
                    return true;
                }
            }
            boxY = 0;
            return false;
        }

        public virtual void Update()
        {
            hitbox = GetHitbox(texture, location);
            //grounded = GetGroundCollision(hitbox, out boxY);
        }

        public void ApplyGravity()
        {
            if (GetGroundCollision(new Rectangle((int)hitbox.X, (int)hitbox.Y + hitbox.Height, hitbox.Width, 1), out boxY))
            {
                if ((velocity.Y >= 0) && (hitbox.Y + hitbox.Height - 20 < boxY))
                {
                    velocity.Y = 0;
                    grounded = true;
                    location.Y = boxY - hitbox.Height;
                }
            }
            else
            {
                if (velocity.Y < 15)
                {
                    velocity.Y += gravity.Y;
                }
                else
                {
                    velocity.Y = 15;
                }
            }

        }
    }
}
