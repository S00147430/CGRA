using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System.Timers;
using System.IO;

namespace MonoGameClient
{
    public class Game1 : Game
    {
        Rectangle rec;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static IHubProxy proxy;

        //SignalR
        HubConnection connection;

        //Menu
        public int currentLevel = 1;//make it the first level
        Menu MenuPart;
        public static string GameState = "Menu";
        public static SpriteFont font;
        public static Rectangle screen;
        private Texture2D background;

        //Data
        int score;
        string playerName = "";
        string outcome = "";
        int collectableInteraction;
        public List<PlayerData> leaderboardList = new List<PlayerData>();

        //Screen
        public static int ScreenWidth;
        public static int ScreenHeight;

        //GameVariables
        const float BALL_START_SPEED = 8f;
        const int PADDLE_OFFSET = 70;
        const float KEYBOARD_PADDLE_SPEED = 10f;
        const float SPIN = 2.5f;
        float comSpeed = 0.56f;

        //ClassObjects
        Player player1;
        Player player2;
        Ball ball;

        //Collectables/LevelUps
        SimpleSprite paddleSize, IncreaseScore, breakertime;
        List<SimpleSprite> IncreaseCollectables;
        Random r = new Random();
        Random rand = new Random();
        Timer colectableTimer = new Timer();
        Timer noteTimer = new Timer();
        Timer gameTimer = new Timer();
        TimeSpan previousSpawnTime, collectableSpawnTime;

        //10 Second Message
        Rectangle mesRec;
        Texture2D mesTex;
        bool writeMes = false;

        //Notification
        Rectangle noteRec;
        Texture2D noteTex;
        bool WriteNote = false;

        //Login
        public static bool loginBool = false;

        //Achievments
        public static bool AchievementsBool = false;

        //LeaderBoard
        Texture2D leaderboard;
        string path = @"D:\AFMTask2Server\PlayerData.txt";
        string readData;

        SpriteFont InGameFont;
        Texture2D middleTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IncreaseCollectables = new List<SimpleSprite>();

            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            //10 Sec Message.
            connection = new HubConnection("http://localhost:49727");
            proxy = connection.CreateHubProxy("MoveCharacterHub");

            connection.Start().Wait();

            Action<Point> SendMessagerecieved = recieved_a_message;
            proxy.On("BroadcastMessage", SendMessagerecieved);

            //Notification
            //previousSpawnTime = new TimeSpan(0);
            //collectableSpawnTime = new TimeSpan(15);

            //////Notification
            //proxy.Invoke<List<Check>>("getNote").ContinueWith((callback) =>
            //{
            //    foreach (Check c in callback.Result)
            //    {
            //        WriteNote = c.WriteNote;
            //        noteRec.Location = c.PosNote;
            //    }
            //}).Wait();
        }

        private void recieved_a_message(Point obj)
        {
            mesRec.Location = obj;
            writeMes = true;
        }

        private static void Connection_Received(string obj)
        {

        }


        protected override void Initialize()
        {
            ScreenWidth = GraphicsDevice.Viewport.Width;
            ScreenHeight = GraphicsDevice.Viewport.Height;

            //Menu
            MenuPart = new Menu();

            player1 = new Player();
            player2 = new Player();
            ball = new Ball();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            InGameFont = Content.Load<SpriteFont>("InGameFont");
            player1.Texture = Content.Load<Texture2D>("Textures/paddle1");
            player2.Texture = Content.Load<Texture2D>("Textures/paddle1");

            background = Content.Load<Texture2D>("Textures/background");
            MenuPart.LoadContent(Content);

            player1.Position = new Vector2(PADDLE_OFFSET, ScreenHeight / 2 - player1.Texture.Height / 2);
            player2.Position = new Vector2(ScreenWidth - player2.Texture.Width - PADDLE_OFFSET, ScreenHeight / 2 - player2.Texture.Height / 2);

            ball.Texture = Content.Load<Texture2D>("Textures/pongball");
            ball.Launch(BALL_START_SPEED);

            middleTexture = Content.Load<Texture2D>("Textures/middle");

            //LeaderBoard 
            leaderboard = Content.Load<Texture2D>("Textures/LeaderboardScreen");

            //Collectables and Level ups.
            Random rand = new Random();
            Random rno = new Random();
            int randno = rno.Next(7, 12);
            int maxx = GraphicsDevice.Viewport.Width - 20;
            int maxy = GraphicsDevice.Viewport.Height - 20;

            //10 Sec Messsage
            mesTex = Content.Load<Texture2D>("Textures/Thanks");
            mesRec = new Rectangle(100, 100, mesTex.Width, mesTex.Height);

            //Notification
            noteTex = Content.Load<Texture2D>("Textures/Note");
            noteRec = new Rectangle(100, 100, noteTex.Width, noteTex.Height);
            colectableTimer = new Timer(10000);
            noteTimer = new Timer(5000);

            for (int i = r.Next(1, 5); i > 0; i--)
            {
                IncreaseScore = new SimpleSprite(Content.Load<Texture2D>(@"Textures/Increase"), new Vector2(r.Next(maxx), r.Next(maxy)));
                IncreaseCollectables.Add(IncreaseScore);
            }

            SoundManager.LoadSounds(Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GameState != "Ended")
            {
                switch (GameState)
                {
                    case "Menu":
                        MenuPart.Update(gameTime, this);

                        //LeaderBoardData/Login
                        connection.Received += Connection_Received;
                        if (loginBool == true)
                        {
                            proxy.Invoke<List<PlayerData>>("getPlayers").ContinueWith((callback) =>
                            {
                                foreach (PlayerData p in callback.Result)
                                {
                                    playerName = p.PlayerID;
                                    score = p.score;
                                    outcome = p.outcome;
                                    collectableInteraction = p.collectableInteractions;
                                }
                            }).Wait();
                        }

                        else
                        {
                            playerName = "Guest";
                        }

                        //if (loginBool == true)
                        //{
                        //    proxy.Invoke<List<PlayerData>>("getPlayers").ContinueWith((callback) =>
                        //    {
                        //        foreach (PlayerData p in callback.Result)
                        //        {
                        //            p = new PlayerData { PlayerID = "Player User", score = 0, outcome = null, collectableInteractions = 0 },
                                    
                                    
                        //        };
                        //    }).Wait();
                        //}
                        break;
                }

                if (GameState == null)
                {
                    ScreenWidth = GraphicsDevice.Viewport.Width;
                    ScreenHeight = GraphicsDevice.Viewport.Height;

                    ball.Move(ball.Velocity);

                    Vector2 player1Velocity = Input.GetKeyboardInputDirection(PlayerIndex.One) * KEYBOARD_PADDLE_SPEED;
                    Vector2 player2Velocity = Input.GetKeyboardInputDirection(PlayerIndex.Two) * KEYBOARD_PADDLE_SPEED;

                    player1.Move(player1Velocity);
                    player2.Move(player2Velocity);

                    Vector2 player1TouchVelocity, player2TouchVelocity;
                    Input.ProcessTouchInput(out player1TouchVelocity, out player2TouchVelocity);

                    player1.Move(player1TouchVelocity);
                    player2.Move(player2TouchVelocity);

                    player2.Position.Y = ball.Position.Y * comSpeed;

                    if (player1TouchVelocity.Y > 0f)
                    {
                        player1Velocity = player1TouchVelocity;
                    }
                    if (player2TouchVelocity.Y > 0f)
                    {
                        player2Velocity = player2TouchVelocity;
                    }

                    if (player1Velocity.Y != 0)
                    {
                        player1Velocity.Normalize();
                    }
                    if (player2Velocity.Y != 0)
                    {
                        player2Velocity.Normalize();
                    }

                    if (GameObject.CheckPaddleBallCollision(player1, ball))
                    {
                        ball.Velocity.X = Math.Abs(ball.Velocity.X);
                        ball.Velocity += player1Velocity * SPIN;
                        SoundManager.PaddleBallCollisionSoundEffect.Play();
                    }

                    if (GameObject.CheckPaddleBallCollision(player2, ball))
                    {
                        ball.Velocity.X = -Math.Abs(ball.Velocity.X);
                        ball.Velocity += player2Velocity * SPIN;
                        SoundManager.PaddleBallCollisionSoundEffect.Play();
                    }

                    foreach (var collectable in IncreaseCollectables)
                    {
                        if (collectable.visible && collectable.BoundingRect.Intersects(player1.Bounds))
                        {
                            collectable.visible = false;
                            player1.Score++;
                            collectableInteraction += 1;
                        }

                        if (collectable.visible && collectable.BoundingRect.Intersects(player2.Bounds))
                        {
                            collectable.visible = false;
                            player2.Score++; ;
                        }
                    }

                    if (ball.Position.X + ball.Texture.Width < 0)
                    {
                        ball.Launch(BALL_START_SPEED);
                        player2.Score++;
                    }

                    if (ball.Position.X > ScreenWidth)
                    {
                        ball.Launch(BALL_START_SPEED);
                        player1.Score++;
                    }

                    if (player1.Score == 10)
                    {
                        outcome = "Win";
                        score = player1.Score;
                        GameState = "Score";

                        //if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        //{
                        //    GameState = "Menu";
                        //    MenuPart.Update(gameTime, this);
                        //}
                    }

                    if (player2.Score == 10)
                    {
                        outcome = "Lose";
                        score = player1.Score;
                        GameState = "Score";

                        //if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        //{
                        //    GameState = "Menu";
                        //    MenuPart.Update(gameTime, this);
                        //}
                    }

                    //Notification for Collectable
                    //if (gameTime.TotalGameTime - previousSpawnTime > collectableSpawnTime)
                    //{
                    //    previousSpawnTime = gameTime.TotalGameTime;
                    //    var spawnSeconds = (10);
                    //    collectableSpawnTime = TimeSpan.FromSeconds(spawnSeconds);
                    //if (WriteNote == false)
                    //{
                    //    proxy.Invoke<List<Check>>("getNote").ContinueWith((callback) =>
                    //    {
                    //        foreach (Check c in callback.Result)
                    //        {
                    //            WriteNote = c.WriteNote;
                    //            noteRec.Location = c.PosNote;
                    //        }
                    //    }).Wait();

                    //    for (int i = r.Next(1, 5); i > 0; i--)
                    //    {
                    //        IncreaseCollectables.Add(IncreaseScore);

                    //    }
                    //}
                        //}
                    //}


                    if (!File.Exists(path))
                    {
                        // Create a file to write to.                     
                        File.WriteAllText(path, playerName + " Score " + Convert.ToInt32(score) + " Outcome: " + outcome + " Collectable Interactions: " + Convert.ToInt32(collectableInteraction));
                        readData = File.ReadAllText(path);
                    }
                    base.Update(gameTime);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            if (GameState != "Ended")
            {
                switch (GameState)
                {
                    case "Menu":
                        spriteBatch.Begin();
                        spriteBatch.Draw(background, GraphicsDevice.Viewport.Bounds, Color.White);

                        //If Login is selected
                        if (loginBool == true)
                        {
                            spriteBatch.DrawString(InGameFont, playerName, new Vector2(150, 50), Color.White);
                        }

                        if (AchievementsBool == true)
                        {
                            spriteBatch.DrawString(InGameFont, "Username:" + playerName + "\nScore:" + Convert.ToInt32(score) + "\nLast Outcome: " + outcome + "\nCollectable Interactions: " + Convert.ToInt32(collectableInteraction), new Vector2(150, 150), Color.White);
                        }

                        //10 Sec Message
                        if (writeMes == true)
                        {
                            spriteBatch.Draw(mesTex, mesRec, Color.White);
                        }
                        spriteBatch.End();

                        MenuPart.Draw(spriteBatch, GraphicsDevice.Viewport);
                        break;
                }              

                if (GameState == null)
                {
                    GraphicsDevice.Clear(Color.Black);

                    spriteBatch.Begin();
                    spriteBatch.DrawString(InGameFont, player1.Score + " " + playerName + "   " + player2.Score + " Player 2/COM", new Vector2(ScreenWidth / 2 
                        - InGameFont.MeasureString(player1.Score + playerName + player2.Score + "      Player 2").X, 0), Color.White);
                    spriteBatch.Draw(middleTexture, new Rectangle(ScreenWidth / 2 - middleTexture.Width / 2, 0, middleTexture.Width, ScreenHeight), null, Color.White);
                    player1.Draw(spriteBatch);
                    player2.Draw(spriteBatch);
                    ball.Draw(spriteBatch);

                    while (WriteNote == true)
                    {
                        noteTimer.Start();
                        spriteBatch.Draw(noteTex, noteRec, Color.White);
                        colectableTimer.Start();
                        if (noteTimer == new Timer(3000))
                        {
                            WriteNote = false;
                        }
                    }

                    foreach (var collectable in IncreaseCollectables)
                    {
                        collectable.draw(spriteBatch);
                    }

                    spriteBatch.End();

                    base.Draw(gameTime);
                }

                switch (GameState)
                {
                    case "Score":
                        spriteBatch.Begin();
                        spriteBatch.Draw(leaderboard, GraphicsDevice.Viewport.Bounds, Color.White);
                        spriteBatch.DrawString(InGameFont,"Usermame:" + playerName + "  Score: " + Convert.ToInt32(score) + "  Outcome: " + outcome + "  Collectable Interactions: " + Convert.ToInt32(collectableInteraction), new Vector2(150, 150), Color.White);
                        spriteBatch.End();
                        break;
                }

            }
        }
    }
}

