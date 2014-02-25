using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer_20140222
{
    class Animation
    {
        Texture2D texture;
        public Vector2 location;
        public int columns, rows, currentColumn, currentRow, currentFrame = 1, totalFrames;
        int width, height;
        Rectangle sourceRectangle, destinationRectangle;
        SpriteBatch spriteBatch;
        public SpriteEffects spriteEffect = SpriteEffects.None;
        public int scale = 1;

        public int delay;
        public int maxDelay;

        public Animation(SpriteBatch spriteBatch, Texture2D texture, int totalFrames, int columns, int rows, Vector2 location)
        {
            this.texture = texture;
            this.totalFrames = totalFrames;
            this.columns = columns;
            this.rows = rows;
            this.location = location;
            this.spriteBatch = spriteBatch;
            maxDelay = 5;
            delay = maxDelay;
        }

        public void Update()
        {
            if (delay <= 0)
            {
                if (currentFrame < totalFrames)
                {
                    currentFrame++;
                }
                else
                {
                    currentFrame = 1;
                }
                delay = maxDelay;
            }
            else if (delay>0)
            {
                delay--;
            }
        }

        public void Draw()
        {
            
            currentRow = (int)(Math.Ceiling((float)currentFrame/(float)columns));

            if (currentFrame % columns == 0)
            {
                currentColumn = columns;
            }
            else
            {
                currentColumn = currentFrame % columns;
            }

            width = texture.Width/columns;
            height = texture.Height/rows;

            sourceRectangle = new Rectangle((currentColumn-1) * width, (currentRow-1) * height, width, height);
            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle , Color.White, 0f, Vector2.Zero, spriteEffect, 0);

        }

        public override string ToString()
        {
            return ("currentFrame " + currentFrame + " totalFrames" + totalFrames + " width " + width + "\n height " + height + " sourceRec " + sourceRectangle + "\n destinationRec " + destinationRectangle + " currentColumn " + currentColumn + "\n currentRow " + currentRow + (currentRow == rows) + (currentColumn == columns));
        }
    }
}
