using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer_20140222
{
    class Player : Entity
    {
        KeyboardState state;
        public float movementSpeed = 4;
        public int maxHealth = 100;
        public int health;
        public int invincible = 90, invincibleDelay = 90;

        public Player(Texture2D texture, Vector2 location) : base(texture, location)
        {
            health = maxHealth;
        }

        public override void Update()
        {
            base.Update();
            hitbox.Inflate(-50, 0);
            base.ApplyGravity();
            grounded = GetGroundCollision(new Rectangle(hitbox.Location.X, hitbox.Location.Y, hitbox.Width, hitbox.Height + 10), out boxY);
            if (invincible > 0)
            {
                invincible--;
            }
            state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
            {
                if (!GetGroundCollision(new Rectangle(hitbox.Location.X - 5, hitbox.Location.Y, hitbox.Width, hitbox.Height-10), out boxY))
                {
                    velocity.X = -movementSpeed;
                }
                else
                {
                    velocity.X = 0;
                }

                isRight = false;
            }
            if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
            {
                if (!GetGroundCollision(new Rectangle(hitbox.Location.X + 5, hitbox.Location.Y, hitbox.Width, hitbox.Height-10), out boxY))
                {
                    velocity.X = movementSpeed;
                }
                else
                {
                    velocity.X = 0;
                }

                isRight = true;
            }

            if (state.IsKeyDown(Keys.W) || (state.IsKeyDown(Keys.Up)))
            {
                if (grounded && velocity.Y>=0)
                {
                    velocity.Y = -20;
                }

            }

            if (state.IsKeyUp(Keys.D) && state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.Left) && state.IsKeyUp(Keys.Right))
            {
                velocity.X = 0;
            }

            location += velocity;

        }


        public SpriteEffects GetSpriteEffects()
        {
            if (!isRight)
            {
                return SpriteEffects.FlipHorizontally;
            }
            else
            {
                return SpriteEffects.None;
            }
        }
        
    }
}
