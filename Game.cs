using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomMario.Map;
using SplashKitSDK;
using Rectangle = System.Drawing.Rectangle;
using System.Threading;
using System.Diagnostics;
using System.Collections;

namespace CustomMario
{
    public class Game
    {
        DrawMap map;
        Mario player;
        bool _loaded = false;
        List<Rectangle> _rects;
        List<Rectangle> _mapObj;
        List<MapObject> QB_obj;
        Bitmap bgImage;
        Point2D Mario_location;
        Rectangle Mario_hitbox, Mario_rectDown;
        Point2D _point, _mariohud, _time, coin;
        SoundEffect _bgMusic;
        Bitmap HUDmario, Time, Coin;
        Font font;
        int lives, countdownTimer, _lifeUpdate, totalCoins, _coinAdd;
        SplashKitSDK.Timer gameTimer;
        Goomba _goomba9;
        bool m1, m2, m3, m4;
        GC_List _gcList;
        bool _pause;
        bool _victory;
        bool _courseEnded;
        SoundEffect VictorySFX, LifeLostSFX, powerUpAppearSFX;
        bool _dies;
        coinList _coinList;
        GoombaList _goombaList;
        double[,] coinPositions, goombaPositions;


        public void Load()
        {
            map = new DrawMap();
            player = new Mario(50, 50);
            bgImage = new Bitmap("Bg", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\background2.png");

            _point = new Point2D();
            _bgMusic = SplashKit.LoadSoundEffect("BackgroundMusic", "F:\\Projects\\repo\\CustomMario\\Resources\\music\\Overworld.mp3");
            HUDmario = new Bitmap("MarioHUD", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\HUDMario2.png");
            Time = new Bitmap("Time", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\TimeSprite2.png");
            Coin = new Bitmap("Coin", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\HUDCoinSprite4.png");

            _coinList = new coinList();
            _goombaList = new GoombaList();

            SplashKit.PlaySoundEffect(_bgMusic, -1); // -1 loops indefinitely
            font = SplashKit.LoadFont("Arial", "F:\\Projects\\repo\\CustomMario\\Resources\\fonts\\ARIALBD.TTF");


            //SFX
            VictorySFX = SplashKit.LoadSoundEffect("Victory", "F:\\Projects\\repo\\CustomMario\\Resources\\SFX\\clearcourse.mp3");
            LifeLostSFX = SplashKit.LoadSoundEffect("GameOver", "F:\\Projects\\repo\\CustomMario\\Resources\\SFX\\lifelost.mp3");
            powerUpAppearSFX = SplashKit.LoadSoundEffect("PowerUpAppear", "F:\\Projects\\repo\\CustomMario\\Resources\\SFX\\SFX1upAppear.mp3");

            lives = 1;
            totalCoins = 0;
            countdownTimer = 100;
            gameTimer = SplashKit.CreateTimer("Timer");
            SplashKit.StartTimer(gameTimer);
            _gcList = new GC_List();


            coinPositions = new double[,]
            {
                {32, 560}, {29, 560}, {30, 560}, {31, 560}, {35, 150},
                {36, 75}, {37, 0}, {38, -20}, {59, 560}, {56, 560},
                {57, 560}, {58, 560}, {73, 560}, {71, 560}, {72, 560},
                {73, -6}, {72, -6}
            };
            for (int i = 0; i < coinPositions.GetLength(0); i++)
                {
                    double x = coinPositions[i, 0];
                    double y = coinPositions[i, 1];
                    _coinList.Add(new Coin(x, y));
                }

            //enemies initialization
            goombaPositions = new double[,]
            {
                {10, 560}, {17, 560}, {160, 560}, {44, 560}, {52, 560},
                {86, 560}, {112, 560}, {135, 560}, {73, 560}
            };

            for (int i = 0; i < goombaPositions.GetLength(0); i++)
            {
                double x = goombaPositions[i, 0];
                double y = goombaPositions[i, 1];
                _gcList.Add(new Goomba(x, y));
            }


            _goombaList.InitializeGoombas(goombaPositions);
            _gcList.AddGoombas(_goombaList.GetGoombas());

            _goomba9 = new Goomba(73, 560);

            m1 = false;
            m2 = false;
            m3 = false;
            m4 = false;

            _loaded = true;
            _courseEnded = false;
            _victory = false;
            _dies = false;
            _pause = false;
        }

        public void Main(Window MarioWindow)
        {

            if (!_loaded) { Load(); }


            if (SplashKit.KeyTyped(KeyCode.PKey))
            {
                _pause = !_pause; // Toggle pause state
                Console.WriteLine(_pause ? "paused" : "resumed");
            }


            if (!_pause && !_courseEnded)
            {

                SplashKit.ClearWindow(MarioWindow, Color.White);

                Mario_location = player.getLocation();
                Mario_hitbox = player.getHitbox();
                Mario_rectDown = player.getRectdown();



                int elapsedTime = (int)(SplashKit.TimerTicks(gameTimer) / 1000);
                int timeRemaining = countdownTimer - elapsedTime;


                //ToWorld()
                _point.X = 0; _point.Y = -70;
                _point = SplashKit.ToWorld(_point);

                //Draw background
                bgImage.Draw(_point.X, _point.Y);



                _mariohud.X = 50; _mariohud.Y = 45;
                _mariohud = SplashKit.ToWorld(_mariohud);       //Mario hud
                SplashKit.DrawText("X    " + lives, Color.Black, font, 24, _mariohud.X + 20, _mariohud.Y + 43);


                _time.X = 800; _time.Y = 45;
                _time = SplashKit.ToWorld(_time);           //time hud
                SplashKit.DrawText("X    " + timeRemaining, Color.Black, font, 24, _time.X + 20, _time.Y + 43);


                coin.X = 1000; coin.Y = 45;
                coin = SplashKit.ToWorld(coin);           //Coin hud
                SplashKit.DrawText(" " + totalCoins, Color.Black, font, 30, coin.X + 86, coin.Y + 5);



                //Generate the map
                map.Draw();



                SplashKit.DrawBitmap(HUDmario, _mariohud.X, _mariohud.Y);       //draw HUDs
                SplashKit.DrawBitmap(Time, _time.X, _time.Y);
                SplashKit.DrawBitmap(Coin, coin.X, coin.Y);


                _rects = map.getRect();             //get rects from map objects
                _mapObj = map.getQB_rects();
                QB_obj = map.getQB_list();

                _gcList.Draw(_rects, Mario_hitbox, Mario_rectDown, ref lives);         //Game character AI
                _lifeUpdate = _gcList.Lives(_rects, Mario_hitbox, Mario_rectDown, ref lives);
                _coinAdd = _coinList.Draw(Mario_hitbox);

                if (Mario_location.X > 4275) { _goomba9.Moving(_rects, Mario_hitbox, Mario_rectDown, lives); }
                lives += _lifeUpdate;
                totalCoins += _coinAdd;


                // Update player states
                if (!_courseEnded && !_victory && !_dies)
                {
                    player.HandleInput(_rects, _mapObj, QB_obj, ref lives);            //Player movements
                }

                //Move camera corresponding to player
                if (Mario_location.Y < 4)
                {
                    SplashKit.MoveCameraTo(Mario_location.X - MarioWindow.Width / 2 + 50, Mario_location.Y - MarioWindow.Height / 2 + 300);
                }
                else if (Mario_location.X > 485) { SplashKit.MoveCameraTo(Mario_location.X - MarioWindow.Width / 2 + 50, MarioWindow.Height / 2 - 500); }
                else { SplashKit.MoveCameraTo(MarioWindow.Width / 2 - 675, MarioWindow.Height / 2 - 500); }

                if (Mario_location.X > 13500 && Mario_location.X < 13625 && Mario_location.Y > 500)
                {
                    _courseEnded = true;
                }
              
                // Winning state
                if (!_victory && Mario_location.X > 13500 && Mario_location.X < 13625 && Mario_location.Y > 500)
                {
                    _victory = true; // Set victory flag
                    player.Winning();
                    SplashKit.StopSoundEffect(_bgMusic);
                    SplashKit.PlaySoundEffect(VictorySFX);
                    Console.WriteLine("Victory!");
                    MarioWindow.Refresh(60);
                }

                // Death state
                if (lives == 0 || timeRemaining == 0)
                {
                    _dies = true;
                    player.Death();
                    SplashKit.StopSoundEffect(_bgMusic);
                    SplashKit.PlaySoundEffect(LifeLostSFX);
                    Console.WriteLine("Failed!");
                    MarioWindow.Refresh(60);
                }

                //Spawning mushrooms

                QuestionBlock collidedBlock = player.collideActiveBlocks_block(QB_obj, Mario_hitbox);
                Mushroom mushroom = null;
                if (collidedBlock != null)
                {
                    Console.WriteLine("Collision with an active block detected.");

                    usedBlock newBlock = new usedBlock(collidedBlock.Rect().X, collidedBlock.Rect().Y);
                    int index = player.CollideActiveBlocks_index(_mapObj, Mario_hitbox);
                    Console.WriteLine(index);
                    mushroom = new Mushroom(collidedBlock.Rect().X, collidedBlock.Rect().Y - newBlock.Rect().Height);
                    if (index == 0 && !m1)
                    {
                        m1 = true;
                        SplashKit.PlaySoundEffect(powerUpAppearSFX);
                        _gcList.Add(mushroom);
                    }
                    else if (index == 3 && !m2)
                    {
                        m2 = true;
                        SplashKit.PlaySoundEffect(powerUpAppearSFX);
                        _gcList.Add(mushroom);
                    }
                    else if (index == 9 && !m3)
                    {
                        m3 = true;
                        SplashKit.PlaySoundEffect(powerUpAppearSFX);
                        _gcList.Add(mushroom);
                    }
                    else if (index == 10 && !m4)
                    {
                        m4 = true;
                        SplashKit.PlaySoundEffect(powerUpAppearSFX);
                        _gcList.Add(mushroom);
                    }

                    //for (int i = 0; i < _gcList.Count; i++)
                    //{
                    //    if (_gcList[i] is Mushroom && mushroom.CheckCollision(Mario_hitbox))
                    //    {
                    //        Console.WriteLine("Absorbed");
                    //        _gcList.Remove(mushroom);
                    //        i--; // Adjust index after removal to continue iteration correctly
                    //    }
                    //}
                }
               


                // Update the window display
                MarioWindow.Refresh(60);
                // Process events
                SplashKit.ProcessEvents();





            }

        }

        public bool HasCourseEnded
        {
            get { return _victory; }

        }

        public bool PlayerDies
        {
            get { return _dies; }
        }
    }
}
