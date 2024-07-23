using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomMario.Map;
using SplashKitSDK;
using Rectangle = System.Drawing.Rectangle;

namespace CustomMario
{
    public class Menu
    {
        DrawMap map;
        Bitmap bgImage, logo;
        Point2D _point;
        SoundEffect menuMusic;
        bool loaded = false;
        Font font;
        public Menu()
        {
            font = SplashKit.LoadFont("Arial", "F:\\Projects\\repo\\CustomMario\\Resources\\fonts\\ARIALBD.TTF");

            bgImage = new Bitmap("Background", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\background2.png");
            logo = new Bitmap("Logo", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\gamelogo5.png");

            _point = new Point2D();

            menuMusic = SplashKit.LoadSoundEffect("Plains", "F:\\Projects\\repo\\CustomMario\\Resources\\music\\Plains2.mp3");

            map = new DrawMap();
        }
        public void Load()
        {

            SplashKit.PlaySoundEffect(menuMusic, -1);
            loaded= true;
        }
        public void Main_menu(Window MarioWindow)
        {
            if (!loaded) { Load(); }
            //ToWorld()
            _point.X = 0; _point.Y = -70;
            _point = SplashKit.ToWorld(_point);

            SplashKit.ClearWindow(MarioWindow, Color.Black);
    

            //Draw background
            bgImage.Draw(_point.X, _point.Y);
            map.Draw();
            logo.Draw(MarioWindow.Width / 2 + 550, 25);
            MarioWindow.DrawText("Press R to start", Color.Black,  MarioWindow.Width / 2 + 750, 300);
            SplashKit.MoveCameraTo(MarioWindow.Width / 2 + 175, MarioWindow.Height / 2 - 500);
            SplashKit.RefreshScreen(60);
        }

        public void Stop()
        {
            SplashKit.StopSoundEffect(menuMusic);
        }



    }
}
