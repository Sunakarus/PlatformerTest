using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer_20140222
{
    class Menu
    {
        public Menu(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
        }

        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;
        public List<Texture2D> buttonList = new List<Texture2D>();
        MouseState mouseState;
        Vector2 mousePos;


        public void Update()
        {
            mouseState = Mouse.GetState();
            mousePos = new Vector2(mouseState.X, mouseState.Y);
            for (int i = 0; i<buttonList.Count;i++)
            {
                Texture2D tempBut = buttonList[i];
                Rectangle hitbox = new Rectangle(graphics.PreferredBackBufferWidth/2 - tempBut.Width/2, (i+1)*100, tempBut.Width, tempBut.Height);
                if (mouseState.LeftButton == ButtonState.Pressed && hitbox.Contains((int)mousePos.X,(int)mousePos.Y))
                {
                    switch (i)
                    {
                        case 0:  //PLATFORMER
                            {
                                Game1.currentScreen = Game1.CurrentScreen.Platformer;
                                break;
                            }
                        case 1:  //EDITOR
                            {
                                Game1.currentScreen = Game1.CurrentScreen.LevelEditor;
                                break;
                            }

                    }

                }

            }

        }

        public void Draw()
        {
            spriteBatch.Begin();
            for (int i = 0; i < buttonList.Count; i++ )
            {
                spriteBatch.Draw(buttonList[i], new Vector2(graphics.PreferredBackBufferWidth/2 - buttonList[i].Width/2, (i+1)*100), Color.White);
            }
            spriteBatch.End();
        }
    }
}
