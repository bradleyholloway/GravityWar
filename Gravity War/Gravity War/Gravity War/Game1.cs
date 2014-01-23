using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BradleyXboxUtils;
using Microsoft.Xna.Framework.Net;

namespace Gravity_War
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont timesNewRoman;
        KeyboardInput keyboard = new KeyboardInput();
        ControlButton reset = new ControlButton();
        ControlButton bullets = new ControlButton();

        const int maxGamers = 16;
        const int maxLocalGamers = 4;
        NetworkSession networkSession;
        PacketWriter packetWriter = new PacketWriter();
        PacketReader packetReader = new PacketReader();

        string errorMessage;


        List<Player> players;

        PlanetGenerator planetGenerator;

        int windowX, windowY;
        Random r;
#endregion

        #region Initialize

        public Game1()
        {
            Bullet.radius = 10;
            Bullet.timeStep = .2;
            Player.radius = 20;
            Player.timeStep = .2;
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            //this.graphics.IsFullScreen = true;
            Components.Add(new GamerServicesComponent(this));
            

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            reset = new ControlButton();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            windowX = GraphicsDevice.Viewport.Width;
            windowY = GraphicsDevice.Viewport.Height;
            planetGenerator = new PlanetGenerator(windowX, windowY);
            planetGenerator.clearImages();
            planetGenerator.loadImage(Content.Load<Texture2D>("bluePlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("brownPlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("earthPlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("goldPlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("moonPlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("orangePlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("sunPlanet"));
            planetGenerator.loadImage(Content.Load<Texture2D>("yellowPlanet"));
            //planetGenerator.loadImage(Content.Load<Texture2D>("dollar"));
            Bullet.image = Content.Load<Texture2D>("bullet");
            Player.image = Content.Load<Texture2D>("ufo");
            timesNewRoman = Content.Load<SpriteFont>("TimesNewRoman");
            Bullets.clear();
            r = new Random();
            //addBullets();
            addPlanets();

            players = new List<Player>();


            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                players.Add(new Player(Planets.getPlanets().ElementAt<Planet>(0), 0f, new ControllerInput(PlayerIndex.One)));
                if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                    players.Add(new Player(Planets.getPlanets().ElementAt<Planet>(1), 0f, new ControllerInput(PlayerIndex.Two)));
                if (GamePad.GetState(PlayerIndex.Three).IsConnected)
                    players.Add(new Player(Planets.getPlanets().ElementAt<Planet>(2), 0f, new ControllerInput(PlayerIndex.Three)));
                if (GamePad.GetState(PlayerIndex.Four).IsConnected)
                    players.Add(new Player(Planets.getPlanets().ElementAt<Planet>(3), 0f, new ControllerInput(PlayerIndex.Four)));
            }
            else
                players.Add(new Player(Planets.getPlanets().ElementAt<Planet>(0), 0f, new KeyboardInput()));
            
            
            // TODO: use this.Content to load your game content here
        }
        #endregion

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (networkSession == null)
            {
                runMenu();
            }
            else
            {
                runGame();
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            foreach (Planet p in Planets.getPlanets())
            {
                spriteBatch.Draw(p.getImage(), p.getLocation(), null, Color.White, 0f, p.getOrigin(), p.getScale(), SpriteEffects.None, 0f);
            }
            foreach (Bullet b in Bullets.getBullets())
            {
                spriteBatch.Draw(Bullet.image, b.getLocation(), null, Color.White, b.getRotation(), Bullet.origin, Bullet.scale, SpriteEffects.None, 0f);
            }
            foreach (Player p in players)
            {
                spriteBatch.Draw(Player.image, p.getLocation(), null, Color.White, p.getRotation(), Player.origin, Player.scale, SpriteEffects.None, 0f);
            }
            //spriteBatch.DrawString(timesNewRoman, "" + Bullets.getBullets().Count, new Vector2(100, 100), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        #region game
        public void runGame()
        {
            for (int a = 0; a < Bullets.getBullets().Count; a++)
            {
                Bullets.getBullets().ElementAt<Bullet>(a).run(Planets.getGravityField(Bullets.getBullets().ElementAt<Bullet>(a).getLocation()));
                if (Planets.collides(Bullets.getBullets().ElementAt<Bullet>(a).getLocation()))
                {
                    Bullets.remove(a);
                    a--;
                }
            }
            for (int a = 0; a < Planets.getPlanets().Count; a++)
            {
                Planets.getPlanets().ElementAt<Planet>(a).move(Planets.getGravityField(Planets.getPlanets().ElementAt<Planet>(a).getLocation()));
            }
            for (int a = 0; a < players.Count; a++)
            {
                Player p = players.ElementAt<Player>(a);



                p.run();

                if (p.isDead())
                {
                    players.Remove(p);
                    a--;
                }
            }

            for (int a = 0; a < Planets.getPlanets().Count; a++)
            {
                Planets.getPlanets().ElementAt<Planet>(a).move();
            }
            Planets.colide();

            if (players.Count == 0 || reset.update(players.ElementAt<Player>(0).getReset()))
            {
                LoadContent();
            }

            //if (button.update(keyboard.getBottomActionButton()))
            //{
            //    LoadContent();
            //}
            if (bullets.update(keyboard.getUpDPad()))
            {
                addBullets();
            }
        }

        public void addBullets()
        {
            int n = 100;
            for (int a = 0; a < n; a++)
            {
                //Bullets.add(new Bullet(new Vector2(/*r.Next(windowX)*/0, ((float)windowY * a / n)/*r.Next(windowY)*/), new Vector2((float)r.NextDouble() * 0 + 0, (float)r.NextDouble() * 0)));
                //Bullets.add(new Bullet(new Vector2(/*r.Next(windowX)*/windowX, ((float)windowY * a / n)/*r.Next(windowY)*/), new Vector2((float)r.NextDouble() * 0 - 0, (float)r.NextDouble() * 0)));
                //Bullets.add(new Bullet(new Vector2(/*r.Next(windowX)*/ ((float)windowX * a / n),0/*r.Next(windowY)*/), new Vector2((float)r.NextDouble() * 0, (float)r.NextDouble() * 0 + 0)));
                //Bullets.add(new Bullet(new Vector2(/*r.Next(windowX)*/ ((float)windowX * a / n),windowY/*r.Next(windowY)*/), new Vector2((float)r.NextDouble() * 0, (float)r.NextDouble() * 0 - 0)));
            }
            for (int a = 0; a < n; a++)
            {
                for (int b = 0; b < n; b++)
                {
                    Bullets.add(new Bullet(new Vector2(/*r.Next(windowX)*/(float)windowX * b / n, ((float)windowY * a / n)/*r.Next(windowY)*/), new Vector2((float)r.NextDouble() * 0 + 0, (float)r.NextDouble() * 0)));
                }
            }
        }
        public void addPlanets()
        {
            Planets.clear();
            planetGenerator.generate(4 + r.Next(6), false, false);//r.Next(8) * 1 + 2, r.Next(2) == 0, r.Next(2) == 0);//r.Next(15)+5, r.Next(2) == 0);
        }
        #endregion

        #region menu

        public void runMenu()
        {
            if (IsActive)
            {
                if (Gamer.SignedInGamers.Count == 0)
                {
                    Guide.ShowSignIn(maxLocalGamers, false);
                }
                else if (isPressed(Keys.A, Buttons.A))
                {
                    createSession();
                }
                else if (isPressed(Keys.B, Buttons.B))
                {
                    joinSession();
                }
            }

        }

        void createSession()
        {
            try
            {
                networkSession = NetworkSession.Create(NetworkSessionType.SystemLink, maxLocalGamers, maxGamers);
                hookSessionEvents();
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
        }

        void joinSession()
        {
            try
            {
                using (AvailableNetworkSessionCollection avalibleSessions =
                    NetworkSession.Find(NetworkSessionType.SystemLink, maxLocalGamers, null))
                {
                    if (avalibleSessions.Count == 0)
                    {
                        return;
                    }
                    networkSession = NetworkSession.Join(avalibleSessions[0]);
                    hookSessionEvents();
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
        }

        #endregion

#region helpers
        public bool isPressed(Keys key, Buttons button)
        {
            return (Keyboard.GetState().IsKeyDown(key) || GamePad.GetState(PlayerIndex.One).IsButtonDown(button));
        }

#endregion
    }
}
