using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Platformer_20140222
{
    class LevelEditor
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Texture2D tBox, tPlayer, tEnemy, currentTexture, tHopper;
        List<Vector2> boxList = new List<Vector2>();
        List<Vector2> enemyList = new List<Vector2>();
        List<Vector2> hopperList = new List<Vector2>();
        Vector2 player = Vector2.Zero;
        Vector2 tempVector, drawTempVector;
        public SpriteFont tahoma;
        MouseState mouse;
        StreamWriter streamWriter;
        bool isPressed = false, showVector = true;
        bool reallyReset = false;
        enum Select { Box, Player, Enemy, Hopper };
        Select select;
        KeyboardState state, prevState;
        EditorCamera camera = new EditorCamera();
        int closest1, closest2;
        int cameraSpeed = 8;
        string dirPath = "C:\\Users\\HanThi\\Disk Google\\Dropbox\\Projects\\Platformer 20140222\\level.txt";

        int savedTimer = 0;

        public LevelEditor(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
        }

        public void Reset()
        {
            reallyReset = false;
            select = Select.Box;
            currentTexture = tBox;
            state = Keyboard.GetState();
            isPressed = false;
            showVector = true;
            tempVector = Vector2.Zero;

            //camera = new Camera();
            player = Vector2.Zero;
            boxList.Clear();
            enemyList.Clear();
            hopperList.Clear();
        }


        public void LoadContent()
        {/*
            tBox = Content.Load<Texture2D>("box");
            tPlayer = Content.Load<Texture2D>("stickman_idle");
            tEnemy = Content.Load<Texture2D>("Smiley_idle");
            tHopper = Content.Load<Texture2D>("Hopper");
            tahoma = Content.Load<SpriteFont>("tahoma");*/

            //default
            select = Select.Box;
            currentTexture = tBox;
            state = Keyboard.GetState();
        }
        public void Update()
        {
            if (state.IsKeyDown(Keys.Escape))
            {
                Game1.currentScreen = Game1.CurrentScreen.Menu;
                Reset();
            }
            prevState = state;
            state = Keyboard.GetState();

            if (savedTimer > 0)
            {
                savedTimer--;
            }
            if (state.IsKeyDown(Keys.T) && prevState.IsKeyUp(Keys.T))
            {
                showVector = !showVector;
            }
            if (state.IsKeyDown(Keys.R) && prevState.IsKeyUp(Keys.R))
            {
                reallyReset = true;
            }
            if (state.IsKeyDown(Keys.Y) && prevState.IsKeyUp(Keys.Y) && reallyReset)
            {
                Reset();
            }
            if (state.IsKeyDown(Keys.N) && prevState.IsKeyUp(Keys.N) && reallyReset)
            {
                reallyReset = false;
            }

            //select item
            if (state.IsKeyDown(Keys.D1))
            {
                select = Select.Box;
                currentTexture = tBox;
            }

            if (state.IsKeyDown(Keys.D2))
            {
                select = Select.Enemy;
                currentTexture = tEnemy;
            }
            if (state.IsKeyDown(Keys.D3))
            {
                select = Select.Player;
                currentTexture = tPlayer;
            }
            if (state.IsKeyDown(Keys.D4))
            {
                select = Select.Hopper;
                currentTexture = tHopper;
            }
            mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && !isPressed)
            {
                isPressed = true;
            }

            if (isPressed)
            {
                if (state.IsKeyDown(Keys.Q))
                {
                    tempVector = new Vector2(mouse.X, tempVector.Y);
                }

                if (state.IsKeyDown(Keys.E))
                {
                    tempVector = new Vector2(tempVector.X, mouse.Y);
                }

                if (state.IsKeyDown(Keys.Space) && prevState.IsKeyUp(Keys.Space))
                {
                    tempVector.X = (int)tempVector.X;
                    tempVector.Y = (int)tempVector.Y;
                    closest1 = (int)tempVector.X + (int)camera.position.X;
                    closest2 = (int)tempVector.X + (int)camera.position.X;

                    while (closest1 % currentTexture.Width != 0)
                    {
                        closest1 -= 1;
                    }
                    while (closest2 % currentTexture.Width != 0)
                    {
                        closest2 += 1;
                    }

                    if ((int)tempVector.X + (int)camera.position.X != closest1 || (int)tempVector.X + (int)camera.position.X != closest1)
                    {
                        if ((Math.Abs(((int)tempVector.X + (int)camera.position.X) - closest1)) < (Math.Abs(((int)tempVector.X + (int)camera.position.X) - closest2)))
                        {
                            tempVector.X = closest1 - (int)camera.position.X;
                        }
                        else
                        {
                            tempVector.X = closest2 - (int)camera.position.X;
                        }
                    }

                    closest1 = (int)tempVector.Y + (int)camera.position.Y;
                    closest2 = (int)tempVector.Y + (int)camera.position.Y;

                    while (closest1 % currentTexture.Height != 0)
                    {
                        closest1 -= 1;
                    }
                    while (closest2 % currentTexture.Height != 0)
                    {
                        closest2 += 1;
                    }

                    if ((int)tempVector.Y + (int)camera.position.Y != closest1 || (int)tempVector.Y + (int)camera.position.Y != closest1)
                    {
                        if ((Math.Abs(((int)tempVector.Y + (int)camera.position.Y) - closest1)) < (Math.Abs(((int)tempVector.Y + (int)camera.position.Y) - closest2)))
                        {
                            tempVector.Y = closest1 - (int)camera.position.Y;
                        }
                        else
                        {
                            tempVector.Y = closest2 - (int)camera.position.Y;
                        }
                    }
                }

                if (state.IsKeyUp(Keys.E) && state.IsKeyUp(Keys.Q) && state.IsKeyUp(Keys.Space))
                {
                    tempVector = new Vector2(mouse.X, mouse.Y);
                }
            }

            if (mouse.LeftButton == ButtonState.Released && isPressed)
            {
                isPressed = false;
                //tempVector += camera.position;
                switch (select)
                {
                    case Select.Box:
                        {
                            boxList.Add(new Vector2(tempVector.X + camera.position.X, tempVector.Y + camera.position.Y));
                            break;
                        }
                    case Select.Enemy:
                        {
                            enemyList.Add(new Vector2(tempVector.X + camera.position.X, tempVector.Y + camera.position.Y));
                            break;
                        }
                    case Select.Player:
                        {
                            player = new Vector2(tempVector.X + camera.position.X, tempVector.Y + camera.position.Y);
                            break;
                        }
                    case Select.Hopper:
                        {
                            hopperList.Add(new Vector2(tempVector.X + camera.position.X, tempVector.Y + camera.position.Y));
                            break;
                        }
                }
            }

            if (mouse.RightButton == ButtonState.Pressed)
            {
                switch (select)
                {
                    case Select.Box:
                        {
                            for (int i = boxList.Count - 1; i > -1; i--)
                            {
                                Rectangle tempRectangle = new Rectangle((int)boxList[i].X, (int)boxList[i].Y, tBox.Width, tBox.Height);
                                if (tempRectangle.Contains((int)mouse.X + (int)camera.position.X, (int)mouse.Y + (int)camera.position.Y))
                                {
                                    boxList.RemoveAt(i);
                                }
                            }
                            break;
                        }
                    case Select.Enemy:
                        {
                            for (int i = enemyList.Count - 1; i > -1; i--)
                            {
                                Rectangle tempRectangle = new Rectangle((int)enemyList[i].X, (int)enemyList[i].Y, tEnemy.Width, tEnemy.Height);
                                if (tempRectangle.Contains((int)mouse.X + (int)camera.position.X, (int)mouse.Y + (int)camera.position.Y))
                                {
                                    enemyList.RemoveAt(i);
                                }
                            }
                            break;
                        }
                    case Select.Player:
                        {
                            player = Vector2.Zero;
                            break;
                        }
                    case Select.Hopper:
                        {
                            for (int i = hopperList.Count - 1; i > -1; i--)
                            {
                                Rectangle tempRectangle = new Rectangle((int)hopperList[i].X, (int)hopperList[i].Y, tHopper.Width, tHopper.Height);
                                if (tempRectangle.Contains((int)mouse.X + (int)camera.position.X, (int)mouse.Y + (int)camera.position.Y))
                                {
                                    hopperList.RemoveAt(i);
                                }
                            }
                            break;
                        }

                }
                //tempVector = tempVector + camera.position;
            }
            if (state.IsKeyDown(Keys.P))
            {
                streamWriter = new StreamWriter(dirPath, false);
                streamWriter.Write("BOX ");
                foreach (Vector2 v in boxList)
                {
                    streamWriter.Write((int)v.X + ";" + (int)v.Y + " ");
                }
                streamWriter.WriteLine();
                streamWriter.Write("SMILEY ");
                foreach (Vector2 v in enemyList)
                {
                    streamWriter.Write((int)v.X + ";" + (int)v.Y + " ");
                }
                streamWriter.WriteLine();
                streamWriter.Write("HOPPER ");
                foreach (Vector2 v in hopperList)
                {
                    streamWriter.Write((int)v.X + ";" + (int)v.Y + " ");
                }
                streamWriter.WriteLine();
                streamWriter.Write("PLAYER ");
                streamWriter.Write((int)player.X + ";" + (int)player.Y);
                streamWriter.Close();
                savedTimer = 60;
            }

            if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            {
                camera.position.Y -= cameraSpeed;
            }
            if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
            {
                camera.position.Y += cameraSpeed;
            }
            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
            {
                camera.position.X -= cameraSpeed;
            }
            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            {
                camera.position.X += cameraSpeed;
            }

        }

        public void Draw()
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix());
            drawTempVector = tempVector + camera.position;
            if (isPressed)
            {
                switch (select)
                {
                    case Select.Box:
                        {
                            spriteBatch.Draw(tBox, drawTempVector, Color.White);
                            break;
                        }
                    case Select.Enemy:
                        {
                            spriteBatch.Draw(tEnemy, drawTempVector, Color.White);
                            break;
                        }
                    case Select.Player:
                        {
                            spriteBatch.Draw(tPlayer, drawTempVector, Color.White);
                            break;
                        }
                    case Select.Hopper:
                        {
                            spriteBatch.Draw(tHopper, drawTempVector, Color.White);
                            break;
                        }
                }
                if (showVector)
                    spriteBatch.DrawString(tahoma, drawTempVector.ToString(), drawTempVector, Color.Black);
                        
            }
            foreach (Vector2 vector in boxList)
            {
                spriteBatch.Draw(tBox, vector, Color.White);
                if (showVector)
                    spriteBatch.DrawString(tahoma, vector.ToString(), vector, Color.Black);
            }
            foreach (Vector2 vector in enemyList)
            {
                spriteBatch.Draw(tEnemy, vector, Color.White);
                if (showVector)
                    spriteBatch.DrawString(tahoma, vector.ToString(), vector, Color.Black);
            }
            foreach (Vector2 vector in hopperList)
            {
                spriteBatch.Draw(tHopper, vector, Color.White);
                if (showVector)
                    spriteBatch.DrawString(tahoma, vector.ToString(), vector, Color.Black);
            }
            if (player!=Vector2.Zero)
            {
                spriteBatch.Draw(tPlayer, player, Color.White);
                if (showVector)
                    spriteBatch.DrawString(tahoma, player.ToString(), player, Color.Black);
            }
            spriteBatch.DrawString(tahoma, "P: save\nQ: snap to X\nE: snap to Y\nSelected: " + select + "\nT: toggle vectors\nR: clear all", camera.position, Color.Black);

            if (savedTimer > 0)
            {
                spriteBatch.DrawString(tahoma, "SAVED", camera.position + new Vector2(graphics.PreferredBackBufferWidth / 2 - 20, 30), Color.Black);
            }

            if (reallyReset)
            {
                spriteBatch.DrawString(tahoma, "Clear All? Y/N", camera.position + new Vector2(graphics.PreferredBackBufferWidth / 2 - 30, 60), Color.Black);
            }
            spriteBatch.End();
        }
    }
}
