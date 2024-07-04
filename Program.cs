using System;
using System.Security.Cryptography.X509Certificates;
using CustomMario.Map;
using SplashKitSDK;

using Rectangle = System.Drawing.Rectangle;

namespace CustomMario
{
    public class Program
    {
        public static void Main()
        {
            Window TestWin = new Window("Mario Testing", 1100, 700);
            bool choice = false;
            Game newGame = new Game();

            do
            {
                SplashKit.ProcessEvents();

                switch (choice)
                {
                    case false:
                        //newGame.Menu_main(TestWin, opts);
                        if (SplashKit.KeyDown(KeyCode.RKey))
                        {
                            choice = true;
                        }
                        break;
                    case true:
                        newGame.Main(TestWin);
                        break;
                }
            

            } while (!TestWin.CloseRequested);


            SplashKit.CloseWindow(TestWin);



        }
    }
    }

