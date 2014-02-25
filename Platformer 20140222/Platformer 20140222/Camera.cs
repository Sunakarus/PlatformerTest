using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer_20140222
{
    class Camera
    {
        public Vector2 position = Vector2.Zero;
        GraphicsDeviceManager graphics;

        public Camera(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

        public Matrix TransformMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
            Matrix.CreateTranslation(new Vector3(graphics.PreferredBackBufferWidth * 0.5f, graphics.PreferredBackBufferHeight * 0.65f, 0));
        }

    }
}
