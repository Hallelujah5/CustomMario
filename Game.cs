using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomMario.Map;
using SplashKitSDK;
using Rectangle = System.Drawing.Rectangle;
using System.Threading;

namespace CustomMario
{
    public class Game
    {
        DrawMap map;
        Mario player;
        bool _loaded = false;
        List<Rectangle> _rects;
        Bitmap bgImage;
        Point2D Mario_location;
        Rectangle Mario_hitbox, Mario_rectDown;
        Point2D _point, _mariohud, _time, coin;
        SoundEffect _bgMusic;
        Bitmap HUDmario, Time, Coin;
        Font font;
        int lives, countdownTimer;
        SplashKitSDK.Timer gameTimer;
        Goomba _goomba1, _goomba2, _goomba3, _goomba4, _goomba5, _goomba6, _goomba7, _goomba8, _goomba9;
        Enemies enemyList;
        bool _pause = false;
        bool _victory = false;
        bool _courseEnded = false;
        SoundEffect VictorySFX;

        public void Load()
        {
            map = new DrawMap();
            player = new Mario(50,50);
            bgImage = new Bitmap("Bg", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\background2.png");
            
            _point = new Point2D();
            _bgMusic = SplashKit.LoadSoundEffect("BackgroundMusic", "F:\\Projects\\repo\\CustomMario\\Resources\\music\\Overworld.mp3");
            HUDmario = new Bitmap("MarioHUD", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\HUDMario2.png");
            Time = new Bitmap("Time", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\TimeSprite2.png");
            Coin = new Bitmap("Coin", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\HUDCoinSprite4.png");


            SplashKit.PlaySoundEffect(_bgMusic, -1); // -1 loops indefinitely
            font = SplashKit.LoadFont("Arial", "F:\\Projects\\repo\\CustomMario\\Resources\\fonts\\ARIALBD.TTF");


            //SFX
            VictorySFX = SplashKit.LoadSoundEffect("Victory", "F:\\Projects\\repo\\CustomMario\\Resources\\SFX\\clearcourse.mp3");


            lives = 1;
            countdownTimer = 100;
            gameTimer = SplashKit.CreateTimer("Timer");
            SplashKit.StartTimer(gameTimer);


            //enemies initialization

            enemyList = new Enemies();

            _goomba1 = new Goomba(10, 560);
            _goomba2 = new Goomba(17, 560);
            _goomba3 = new Goomba(160, 560);
            _goomba4 = new Goomba(44, 560);
            _goomba5 = new Goomba(52, 560);
            _goomba6 = new Goomba(86, 560);
            _goomba7 = new Goomba(112, 560);
            _goomba8 = new Goomba(135, 560);
            _goomba9 = new Goomba(73, 560);

            enemyList.Add(_goomba1);
            enemyList.Add(_goomba2);
            enemyList.Add(_goomba3);
            enemyList.Add(_goomba4);
            enemyList.Add(_goomba5);
            enemyList.Add(_goomba6);
            enemyList.Add(_goomba7);
            enemyList.Add(_goomba8);


            _loaded = true;
        }

        public void Main(Window MarioWindow)
        {
   
            if (!_loaded) { Load(); }
            if(lives == 0) { }

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

                if (timeRemaining == 0)
                {
                    //stops the game
                    //minus one Mario life
                }

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
                SplashKit.DrawText("5", Color.Black, font, 30, coin.X + 86, coin.Y + 5);




                //Generate the map
                map.Draw();

                SplashKit.DrawBitmap(HUDmario, _mariohud.X, _mariohud.Y);       //draw HUDs
                SplashKit.DrawBitmap(Time, _time.X, _time.Y);
                SplashKit.DrawBitmap(Coin, coin.X, coin.Y);


                _rects = map.getRect();


                enemyList.Draw(_rects, Mario_hitbox);   
                if (Mario_location.X > 4275) { _goomba9.Moving(_rects, Mario_hitbox); }


                // Update player states
                if (!_courseEnded && !_victory)
                {
                    player.HandleInput(_rects);
                }

                //Move camera corresponding to player
                if (Mario_location.Y < 4)
                {
                    SplashKit.MoveCameraTo(Mario_location.X - MarioWindow.Width / 2 + 50, Mario_location.Y - MarioWindow.Height / 2 + 300);
                }
                else if (Mario_location.X > 485) { SplashKit.MoveCameraTo(Mario_location.X - MarioWindow.Width / 2 + 50, MarioWindow.Height / 2 - 500); }
                else { SplashKit.MoveCameraTo(MarioWindow.Width / 2 - 675, MarioWindow.Height / 2 - 500); }

                if(Mario_location.X > 13500 && Mario_location.X < 13725 && Mario_location.Y > 500)
                {
                    _courseEnded = true;
                }

                // Winning state
                if (!_victory && Mario_location.X > 13500 && Mario_location.X < 13725 && Mario_location.Y > 500)
                {
                  
                    _victory = true; // Set victory flag
                    player.Winning();
                    SplashKit.StopSoundEffect(_bgMusic);
                    SplashKit.PlaySoundEffect(VictorySFX);
                    Console.WriteLine("Victory!");
                    MarioWindow.Refresh(60);

                }








                // Update the window display
                MarioWindow.Refresh(60);
                // Process events
                SplashKit.ProcessEvents();

                




            }

        }
    }
}
