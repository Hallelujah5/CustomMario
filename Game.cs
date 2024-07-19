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
        Mario player    ;
        bool _loaded = false;
        List<Rectangle> _rects;
        Bitmap bgImage;
        Point2D Mario_location;
        Point2D _point, _mariohud, _time, coin;
        SoundEffect _bgMusic;
        Bitmap HUDmario, Time, Coin;
        Font font;
        int lives, countdownTimer;
        SplashKitSDK.Timer gameTimer;
        Goomba _goomba1, _goomba2, _goomba3, _goomba4, _goomba5, _goomba6, _goomba7, _goomba8, _goomba9;

        public void Load()
        {
            map = new DrawMap();
            player = new Mario(50,50);

            bgImage = new Bitmap("Bg", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\background2.png");
            _loaded = true;
            _point = new Point2D();
            _bgMusic = SplashKit.LoadSoundEffect("BackgroundMusic", "F:\\Projects\\repo\\CustomMario\\Resources\\music\\Overworld.mp3");
            HUDmario = new Bitmap("MarioHUD", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\HUDMario2.png");
            Time = new Bitmap("Time", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\TimeSprite2.png");
            Coin = new Bitmap("Coin", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\HUDCoinSprite4.png");


            SplashKit.PlaySoundEffect(_bgMusic, -1); // -1 loops indefinitely
            font = SplashKit.LoadFont("Arial", "F:\\Projects\\repo\\CustomMario\\Resources\\fonts\\ARIALBD.TTF");


            lives = 1;
            countdownTimer = 100;
            gameTimer = SplashKit.CreateTimer("Timer");
            SplashKit.StartTimer(gameTimer);


            //enemies initialization
            _goomba1 = new Goomba(10, 560);
            _goomba2 = new Goomba(17, 560);
            _goomba3 = new Goomba(160, 560);
            _goomba4 = new Goomba(44, 560);
            _goomba5 = new Goomba(52, 560);
            _goomba6 = new Goomba(86, 560);
            _goomba7 = new Goomba(112, 560);
            _goomba8 = new Goomba(135, 560);
            _goomba9 = new Goomba(73, 560);

        }

        public void Main(Window MarioWindow)
        {
            if (!_loaded) { Load(); }

            SplashKit.ClearWindow(MarioWindow, Color.Black);

            Mario_location = player.getLocation();

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
            SplashKit.DrawText("X    " + timeRemaining, Color.Black, font, 24, _time.X+ 20, _time.Y + 43);


            coin.X = 1000; coin.Y = 45;
            coin = SplashKit.ToWorld(coin);           //Coin hud
            SplashKit.DrawText("5", Color.Black, font, 30, coin.X + 86,coin.Y + 5);




            //Generate the map
            map.Draw();

            SplashKit.DrawBitmap(HUDmario, _mariohud.X, _mariohud.Y);       //draw HUDs
            SplashKit.DrawBitmap(Time, _time.X, _time.Y);
            SplashKit.DrawBitmap(Coin,coin.X,coin.Y);


            _rects = map.getRect();

            _goomba1.Moving(_rects);  
            _goomba2.Moving(_rects);
            _goomba3.Moving(_rects);
            _goomba4.Moving(_rects);
            _goomba5.Moving(_rects);
            _goomba6.Moving(_rects);
            _goomba7.Moving(_rects);
            _goomba8.Moving(_rects);
            if (Mario_location.X > 4275) { _goomba9.Moving(_rects); }




            // Update player states
            player.HandleInput(_rects);

            //Move camera corresponding to player
            if (Mario_location.Y < 4)
            {
                SplashKit.MoveCameraTo(Mario_location.X - MarioWindow.Width / 2 + 50, Mario_location.Y - MarioWindow.Height / 2 +300);
            }
            else if (Mario_location.X > 485) { SplashKit.MoveCameraTo(Mario_location.X - MarioWindow.Width / 2 + 50, MarioWindow.Height / 2 - 500); }
            else { SplashKit.MoveCameraTo(MarioWindow.Width / 2 - 675, MarioWindow.Height / 2 - 500); }

            // Update the window display
            MarioWindow.Refresh(60);

            

            // Process events
            SplashKit.ProcessEvents();


        }
    }
}
