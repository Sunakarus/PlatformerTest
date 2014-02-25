#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
#endregion

namespace Platformer_20140222
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Menu menu;
        Platformer platformer;
        LevelEditor levelEditor;

        Texture2D tPlayer, tPlayerWalk, tBox, tEnemy, tEnemyIdle, tHopper, tButtonGame, tButtonEditor;
        SpriteFont tahoma;
        List<Box> boxList = new List<Box>();
        List<Enemy> enemyList = new List<Enemy>();
        List<string> wBoxList = new List<string>();
        List<string> wEnemyList = new List<string>();
        List<string> wHopperList = new List<string>();
        List<string> KEYWORDLIST;
        Camera camera;
        public static Random random = new Random();
        

        public enum CurrentScreen {Menu, Platformer, LevelEditor }
        public static CurrentScreen currentScreen = CurrentScreen.Menu;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            camera = new Camera(graphics);
            KEYWORDLIST = new List<string> { "BOX", "PLAYER", "HOPPER", "SMILEY" };
        }
        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tPlayer = Content.Load<Texture2D>("stickman_idle");
            tEnemyIdle = Content.Load<Texture2D>("Smiley_idle");
            tPlayerWalk = Content.Load<Texture2D>("stickman");
            tBox = Content.Load<Texture2D>("boxEmpty");
            tButtonGame = Content.Load<Texture2D>("buttonGame");
            tButtonEditor = Content.Load<Texture2D>("buttonEditor");
            tEnemy = Content.Load<Texture2D>("SmileyWalk");
            tHopper = Content.Load<Texture2D>("Hopper");
            tahoma = Content.Load<SpriteFont>("Tahoma");

            menu = new Menu(spriteBatch, graphics);
            menu.buttonList.Add(tButtonGame);
            menu.buttonList.Add(tButtonEditor);

            platformer = new Platformer(graphics, spriteBatch);
            platformer.tPlayer = tPlayer;
            platformer.tEnemyIdle = tEnemyIdle;
            platformer.tPlayerWalk = tPlayerWalk;
            platformer.tBox = tBox;
            platformer.tEnemy = tEnemy;
            platformer.tHopper = tHopper;
            platformer.tahoma = tahoma;
            platformer.LoadContent();

            levelEditor = new LevelEditor(graphics, spriteBatch);
            levelEditor.tBox = tBox;
            levelEditor.tPlayer = tPlayer;
            levelEditor.tEnemy = tEnemyIdle;
            levelEditor.tHopper = tHopper;
            levelEditor.tahoma = tahoma;
            levelEditor.LoadContent();
            }

        protected override void UnloadContent()
        {
            spriteBatch.Dispose();
            switch (currentScreen)
            {
                case CurrentScreen.Platformer:
                    {
                        platformer.UnloadContent();
                        break;
                    }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            switch (currentScreen)
            {
                case CurrentScreen.Menu:
                    {
                        menu.Update();
                        break;
                    }
                case CurrentScreen.Platformer:
                    {
                        platformer.Update();
                        break;
                    }
                case CurrentScreen.LevelEditor:
                    {
                        levelEditor.Update();
                        break;
                    }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);
            switch (currentScreen)
            {
                case CurrentScreen.Menu:
                    {
                        menu.Draw();
                        break;
                    }
                case CurrentScreen.Platformer:
                    {
                        platformer.Draw();
                        break;
                    }
                case CurrentScreen.LevelEditor:
                    {
                        levelEditor.Draw();
                        break;
                    }
            }
            base.Draw(gameTime);
        }
    }
}
