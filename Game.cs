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
    public class Game
    {
        DrawMap map;
        Mario player;
        bool _loaded = false;
        List<Rectangle> _rects;
        Bitmap bgImage;
        Point2D Mario_location;
        

        public void Load()
        {
            map = new DrawMap();
            player = new Mario(50,200);
            bgImage = new Bitmap("Bg", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\background2.png");
            _loaded = true;
        }

        public void Main(Window MarioWindow)
        {
            if (!_loaded) { Load(); }

            SplashKit.ClearWindow(MarioWindow, Color.Black);

            Mario_location = player.getLocation();

            bgImage.Draw(0,0);

            //// Draw the map
            map.Draw();

            _rects = map.getRect();

            // Update player state
            player.HandleInput(_rects);

            // Update the window display
            MarioWindow.Refresh(60);

            // Process events
            SplashKit.ProcessEvents();


        }
    }
}
