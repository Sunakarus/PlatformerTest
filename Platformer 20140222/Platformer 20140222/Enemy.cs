using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer_20140222
{
    class Enemy : Entity
    {
        public const int MAXSCREEN = 5000;
        public float speed = 1f;
        public bool dead = false;
        public int damage = 10;
        
        public Enemy(Texture2D texture, Vector2 location) : base(texture, location)
        {
        }

        public override void Update()
        {
            base.Update();
            base.ApplyGravity();
            grounded = GetGroundCollision(new Rectangle(hitbox.Location.X, hitbox.Location.Y, hitbox.Width, hitbox.Height + 10), out boxY);

            if (location.X<-MAXSCREEN || location.X>MAXSCREEN || location.Y<-MAXSCREEN || location.Y>MAXSCREEN)
            {
                Death();
            }

            if (!isRight)
            {
                if (!GetGroundCollision(new Rectangle(hitbox.X - 5, hitbox.Y, hitbox.Width, hitbox.Height - 10), out boxY))
                {
                    velocity.X = -speed;
                }
                else
                {
                    isRight = !isRight;
                }
            }
            else
            {
                if (!GetGroundCollision(new Rectangle(hitbox.X + 5, hitbox.Y, hitbox.Width, hitbox.Height - 10), out boxY))
                {
                    velocity.X = speed;
                }
                else
                {
                    isRight = !isRight;
                }
            }
            location += velocity;

        }

        public void Death()
        {
            dead = true;
        }

        public override string ToString()
        {
            return "velocity " + velocity.ToString() + " location " + location.ToString()
                + "\ntouching left " + GetGroundCollision(new Rectangle((int)location.X - 5, (int)location.Y, texture.Width, texture.Height - 5), out boxY)
                + "\ntouching right " + GetGroundCollision(new Rectangle((int)location.X + 5, (int)location.Y, texture.Width, texture.Height - 5), out boxY);
        }

    }
}
