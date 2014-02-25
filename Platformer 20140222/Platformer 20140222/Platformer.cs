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
    class Platformer
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Texture2D tPlayer, tPlayerWalk, tBox, tEnemy, tEnemyIdle, tHopper;
        Player player;
        Animation playerAnimation;
        public SpriteFont tahoma;
        List<Box> boxList = new List<Box>();
        List<Enemy> enemyList = new List<Enemy>();
        List<string> wBoxList = new List<string>();
        List<string> wEnemyList = new List<string>();
        List<string> wHopperList = new List<string>();
        List<string> KEYWORDLIST;
        string sPlayer;
        string dirPath = "C:\\Users\\HanThi\\Disk Google\\Dropbox\\Projects\\Platformer 20140222\\level.txt";
        Camera camera;
        StreamReader stream;
        Texture2D pinkRec;
        KeyboardState state, prevState;

        public Platformer(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            //camera = new Camera(graphics);
            KEYWORDLIST = new List<string> { "BOX", "PLAYER", "HOPPER", "SMILEY" };
        }

        public void ParseLevelInfo(string keyword, string[] words, List<string> list)
        {
            if (words[0].Equals(keyword))
            {
                for (int i = 1; i < words.Length; i++)
                {
                    list.Add(words[i]);
                }
            }
        }
        public void ParseVectorCoord(string vector, out string x, out string y)
        {
            x = null;
            y = null;
            bool currX = true;
            for (int i = 0; i < vector.Length; i++)
            {
                if (vector[i] == ';')
                {
                    currX = false;
                    continue;
                }
                else if (currX == true)
                {
                    x += vector[i];
                }
                else
                {
                    y += vector[i];
                }
            }
        }
        public void LoadContent()
        {
            camera = new Camera(graphics);
            state = Keyboard.GetState();
            prevState = state;
            pinkRec = new Texture2D(graphics.GraphicsDevice, 1, 1);
            pinkRec.SetData(new[] { Color.Pink });

            if (File.Exists(dirPath))
            {
                stream = new StreamReader(dirPath);
                string s;

                s = stream.ReadLine();
                while (s != null)
                {
                    string[] words = s.Split(' ');

                    ParseLevelInfo("BOX", words, wBoxList);
                    ParseLevelInfo("SMILEY", words, wEnemyList);
                    ParseLevelInfo("HOPPER", words, wHopperList);

                    if (words[0].Equals("PLAYER"))
                    {
                        sPlayer = words[1];
                    }
                    s = stream.ReadLine();
                }


                stream.Close();

                foreach (string vector in wBoxList)
                {
                    if (!KEYWORDLIST.Contains(vector))
                    {
                        string x, y;
                        ParseVectorCoord(vector, out x, out y);
                        if (x != null && y != null)
                        {
                            boxList.Add(new Box(tBox, new Vector2(int.Parse(x), int.Parse(y))));
                        }
                    }
                }

                foreach (string vector in wEnemyList)
                {
                    if (!KEYWORDLIST.Contains(vector))
                    {
                        string x, y;
                        ParseVectorCoord(vector, out x, out y);
                        if (x != null && y != null)
                        {
                            enemyList.Add(new EnemySmiley(tEnemyIdle, new Vector2(int.Parse(x), int.Parse(y))));
                        }
                    }


                }

                foreach (string vector in wHopperList)
                {
                    if (!KEYWORDLIST.Contains(vector))
                    {
                        string x, y;
                        ParseVectorCoord(vector, out x, out y);
                        if (x != null && y != null)
                        {
                            enemyList.Add(new EnemyHopper(tHopper, new Vector2(int.Parse(x), int.Parse(y))));
                        }
                    }


                }
                string[] xyPlayer = sPlayer.Split(';');
                player = new Player(tPlayer, new Vector2(int.Parse(xyPlayer[0]), int.Parse(xyPlayer[1])));
                playerAnimation = new Animation(spriteBatch, tPlayerWalk, 8, 4, 2, new Vector2(int.Parse(xyPlayer[0]), int.Parse(xyPlayer[1])));
            }

        }
        public void UnloadContent()
        {
            pinkRec.Dispose();
        }

        public void Reset()
        {
            boxList.Clear();
            enemyList.Clear();
            wBoxList.Clear();
            wEnemyList.Clear();
            wHopperList.Clear();
            UnloadContent();
            player = null;
            camera = null;
            LoadContent();

        }
        public void Update()
        {
            prevState = state;
            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
            {
                Game1.currentScreen = Game1.CurrentScreen.Menu;
                Reset();
            }

            if (state.IsKeyDown(Keys.R) && prevState.IsKeyUp(Keys.R))
            {
                Reset();
            }


            player.boxList = boxList;
            foreach (Enemy e in enemyList)
            {
                //KILL();
                if (player.hitbox.Intersects(e.hitbox))
                {
                    Vector2 tempVelocity = player.velocity;
                    tempVelocity.Normalize();
                    if ((((player.hitbox.X + player.hitbox.Width) > e.hitbox.X)
                        && (player.hitbox.X < (e.hitbox.X + e.hitbox.Width)))
                        && (Vector2.Dot(tempVelocity, new Vector2(0, 1)) > 0.75)
                        && (player.hitbox.Y + player.hitbox.Height - 10) > e.hitbox.Y+10)
                    {
                        e.Death();
                    }
                    else if (player.invincible <= 0)
                    {
                        player.health -= e.damage;
                        player.invincible = player.invincibleDelay;
                    }
                }

                //ENDKILL();

                e.boxList = boxList;
                e.Update();
            }

            for (int i = enemyList.Count - 1; i > -1; i--)
            {
                if (enemyList[i].dead)
                {
                    enemyList.RemoveAt(i);
                }
            }

            playerAnimation.Update();
            player.Update();


            camera.position.X = player.location.X;
            camera.position.Y = graphics.PreferredBackBufferHeight / 2 + player.location.Y - player.texture.Height;
        }
        public void Draw()
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix());
            //player.hitbox
            Color tempColor = Color.White;
            if (player.invincible > 0)
            {
                tempColor = Color.Red;
            }
            else
            {
                tempColor = Color.White;
            }
            spriteBatch.Draw(pinkRec, new Vector2(player.hitbox.Location.X, player.hitbox.Location.Y), player.hitbox, tempColor, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

            //level
            foreach (Box box in boxList)
            {
                spriteBatch.Draw(box.texture, box.location, Color.White);
                spriteBatch.Draw(pinkRec, box.location, box.hitbox, Color.Orange, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }
            foreach (Enemy en in enemyList)
            {
                spriteBatch.Draw(en.texture, en.location, Color.White);
                spriteBatch.Draw(pinkRec, en.location, en.hitbox, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }
            //player
            if (player.velocity.X == 0)
            {
                spriteBatch.Draw(player.texture, player.location, new Rectangle(0, 0, player.texture.Width, player.texture.Height), Color.White, 0f, Vector2.Zero, 1, player.GetSpriteEffects(), 0);
            }

            if (player.velocity.X != 0)
            {
                playerAnimation.location = player.location;
                playerAnimation.spriteEffect = player.GetSpriteEffects();
                playerAnimation.Draw();
            }
            //debugging info
            spriteBatch.DrawString(tahoma, "HEALTH: " + player.health + "\nplayer: " + player.location.ToString() + " camera: " + camera.position.ToString() + "\nmouse: " + new Vector2(Mouse.GetState().X, Mouse.GetState().Y) + "\nEnemy count: " + enemyList.Count + "\nplayer.velocity: " + player.velocity.ToString(), new Vector2(camera.position.X - 400, camera.position.Y - 300), Color.Black);

            int a = 0;
            foreach (Enemy e in enemyList)
            {
                spriteBatch.Draw(e.texture, e.location, Color.White);
                spriteBatch.DrawString(tahoma, Math.Abs(enemyList.Count - a) + ": " + e.location.ToString() + (e.dead ? " dead " : ""), new Vector2(camera.position.X - 400, camera.position.Y -
                    (30 * a)), Color.Black);
                a++;
            }
            spriteBatch.End();

        }

    }
}
