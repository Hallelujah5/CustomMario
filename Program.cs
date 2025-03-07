using System;
using System.Security.Cryptography.X509Certificates;
using CustomMario.Map;
using SplashKitSDK;
using System.Threading;
using Rectangle = System.Drawing.Rectangle;

namespace CustomMario
{
    public class Program
    {
        public static void Main()
        {
            Window TestWin = new Window("Mario Window", 1200, 800);
            bool choice = false;
            Game newGame = new Game();
            Menu newMenu = new Menu();  
            do
            {
                SplashKit.ProcessEvents();

                switch (choice)
                {
                    case false:
                        newMenu.Main_menu(TestWin);
                        if (SplashKit.KeyDown(KeyCode.RKey))
                        {
                            choice = true;
                            newMenu.Stop();
                        }
                        break;
                    case true:
                        newGame.Main(TestWin);
                        if (newGame.HasCourseEnded)
                        {
                            choice = false;
                            Thread.Sleep(7350);
                        }
                        if (newGame.PlayerDies)
                        {
                            choice = false;
                            Thread.Sleep(3600);
                        }
                        break;
                }
            

            } while (!TestWin.CloseRequested);


            SplashKit.CloseWindow(TestWin);



        }
    }
    }

