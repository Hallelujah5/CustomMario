using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = System.Drawing.Rectangle;
using CustomMario.Map;
using System.Reflection;
using System.Xml;
using System.Net.WebSockets;

namespace CustomMario
{

    public class GC_List
    {
        List<GameCharacter> _gcList;
        SoundEffect SFXGoomba;
        SoundEffect SFXGoombaHit;
        public GC_List()
        {
            _gcList = new List<GameCharacter>();
            SFXGoomba = SplashKit.LoadSoundEffect("GoombaDies", "F:\\Projects\\repo\\CustomMario\\Resources\\SFX\\SFXgoomba.mp3");
            SFXGoombaHit = SplashKit.LoadSoundEffect("GoombaHit", "F:\\Projects\\repo\\CustomMario\\Resources\\SFX\\SFXGoombaHit.mp3");
        }

        public void Add(GameCharacter _entity)
        {
            _gcList.Add(_entity); // add all the enemies into a list and Draw all of them
        }
        public void Remove(GameCharacter _entity)
        {
            _gcList.Remove(_entity);
        }

        public void AddGoombas(List<Goomba> goombaList)
        {
            foreach (var goomba in goombaList)
            {
                Add(goomba);
            }
        }

        public void AddCoins(List<Coin> coinList)
        {
            foreach (var coin in coinList)
            {
                Add(coin);
            }
        }


        public List<GameCharacter> Get_gcList()
        {
            return _gcList;
        }
        public void Draw(List<Rectangle> rects, Rectangle Mario_hitbox, Rectangle Mario_rectDown, ref int lives)
        {
            for (int i = 0; i < _gcList.Count; i++)
            {
                GameCharacter _entity = _gcList[i];
                _entity.Moving(rects, Mario_hitbox, Mario_rectDown, lives);
            }
   
        }
        public int Lives(List<Rectangle> rects, Rectangle Mario_hitbox, Rectangle Mario_rectDown, ref int lives)        //uses collision checks to minus or add a life
        {
            for (int i = 0; i < _gcList.Count; i++)
            {
                GameCharacter _entity = _gcList[i];
                if (_entity is Mushroom)
                {
                    if (_entity.CheckCollision(Mario_hitbox))          //also removes it from the screen after being absorbed
                    {
                        _gcList.Remove(_entity);
                        return _entity.Lives(lives);        //+1 life
                    }
                }


                if (_entity is Goomba)
                {
                    if (_entity.CheckCollision(Mario_rectDown))         //if Mario jumps on it, it will get squished and dies
                    {
                        SplashKit.PlaySoundEffect(SFXGoomba);
                        _gcList.Remove(_entity);
                    }
                    if (_entity.CheckCollision(Mario_hitbox))           //Other wise damage him
                    {
                        SplashKit.PlaySoundEffect(SFXGoombaHit);
                        return _entity.Lives(lives);        //-1 life
                    }
                }
            }
            return 0;

        }

        public int Coins(Rectangle Mario_hitbox)
        {
            for (int i = 0; i < _gcList.Count; i++)
            {
                GameCharacter _entity = _gcList[i];
                if (_entity is Coin)
                {
                    if (_entity.CheckCollision(Mario_hitbox))
                    {
                        _gcList.Remove(_entity);
                        return 1;
                    }
                }
            }
            return 0;
        }

    }

    public abstract class GameCharacter
    {
        public abstract void Draw();
        public abstract void setHitbox();
        public abstract void Moving(List<Rectangle> rects, Rectangle Mario_hitbox, Rectangle mario_rectDown, int lives);
        public Boolean Collision(List<Rectangle> rects, Rectangle RECT)     //rects is the list<> rects of the map, RECT is the directional hitbox of the character.
        {

            foreach (Rectangle rect in rects)
            {

                if (RECT.IntersectsWith(rect))
                {
                    return true;                       //returns true if collide 
                }

            }
            return false;                //else return false
        }

        public double AntiGlitch(List<Rectangle> rects, Rectangle RECT, double speedX, double speedY)   //calculates a safe speed value to apply to the player's position.
        {
            double length;
            if (speedX == 0)
            {
                length = speedY;
            }
            else { length = speedX; }

            Boolean negative;
            if (speedX < 0 || speedY < 0)       //if speedX is negative, means player is going left, negative speedY means jumping/going up.
            {
                negative = true;        //left, up
            }
            else { negative = false; }      //right, down

            Rectangle c_rect = RECT;         //placeholder for the position (top, bottom, left, right) of the 4 rect hitbox of the player.

            for (double i = 0; i < Math.Abs(length) - 1; i++)       //Absolute value
            {
                if (speedX != 0)
                {
                    if (speedX > 0) { c_rect.X++; }     //calculates the X movable distance before colliding
                    else { c_rect.X--; }
                }
                if (speedY != 0)
                {
                    if (speedY > 0) { c_rect.Y++; }     //calc for Y factor
                    else { c_rect.Y--; }
                }
                foreach (Rectangle rect in rects)
                {
                    if (c_rect.IntersectsWith(rect))
                    {
                        if (!negative) { return i; }
                        else { return i * -1; }       //returns the movable safe distance before colliding
                    }
                }
            }
            return length;
        }
        public abstract bool CheckCollision(Rectangle Mario_hitbox);
        public abstract int Lives(int lives);
    }



    public class GoombaList
    {
         List<Goomba> _goombaList;
         SoundEffect _sfxGoomba;
         SoundEffect _sfxGoombaHit;

        public GoombaList()
        {
            _goombaList = new List<Goomba>();
            _sfxGoomba = SplashKit.LoadSoundEffect("GoombaDies", "F:\\Projects\\repo\\CustomMario\\Resources\\SFX\\SFXgoomba.mp3");
            _sfxGoombaHit = SplashKit.LoadSoundEffect("GoombaHit", "F:\\Projects\\repo\\CustomMario\\Resources\\SFX\\SFXGoombaHit.mp3");
        }
        
        public void InitializeGoombas(double[,] positions)
        {
            for (int i = 0; i < positions.GetLength(0); i++)
            {
                double x = positions[i, 0];
                double y = positions[i, 1];
                _goombaList.Add(new Goomba(x, y));
            }
        }

        public List<Goomba> GetGoombas()
        {
            return _goombaList;
        }
    }



    public class Goomba : GameCharacter
    {
        public Bitmap _moving;
        const int speed = 6;

        Point2D _location;
        private Rectangle _rectUp;
        private Rectangle _rectDown;
        private Rectangle _rectLeft;
        private Rectangle _rectRight;

        private Rectangle _hitbox;

        public Goomba(double x, double y)
        {
            _moving = new Bitmap("goombaMove", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\goomba.png");

            _location.X = x * 75;
            _location.Y = y;



        }

        public Boolean onAir(List<Rectangle> rects)
        {

            foreach (Rectangle rect in rects)
            {

                if (_rectDown.IntersectsWith(rect))         //if bottom hitbox intersects with the terrain tiles, it means is on the ground.
                {
                    return false;
                }

            }
            return true;            //else is on air
        }

        //Goomba has a hitbox.
        //When it touches a player, the player dies.
        //Moves backwards when collide with a wall/a cliff


        public override void setHitbox()
        {
            _rectUp = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y), 60, 1);
            _rectDown = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y) + 60, 60, 1);
            _rectLeft = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y), 1, 60);
            _rectRight = new Rectangle(Convert.ToInt32(_location.X) + 60, Convert.ToInt32(_location.Y), 1, 60);

            _hitbox = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y), 60, 57);

        }

        public void debugLocation()
        {
            SplashKit.DrawText("X: " + _location.X / 75, Color.Black, _location.X + 10, _location.Y - 30);
            SplashKit.DrawText("Y: " + _location.Y, Color.Black, _location.X + 10, _location.Y - 20);
        }

        // Check collision with platforms



        float yVelocity = 0;
        bool moving;
        float gravityVlc = 1;
        bool _movingRight = true;


        bool _iframe = false;
        DateTime _iframeStart;
        const int iframeDuration = 1000; // 1 second


        public override void Moving(List<Rectangle> rects, Rectangle Mario_hitbox, Rectangle mario_rectDown, int lives)
        {
            setHitbox();
  

            if (_movingRight)                       //Patrolling behaviour
            {
                if (Collision(rects, _rectRight))
                {
                    _movingRight = false; // Switch direction    
                }
                else
                {
                    double distance = AntiGlitch(rects, _rectRight, -speed, 0);
                    _location.X -= distance;
                }
            }
            else
            {
                if (Collision(rects, _rectLeft))
                {
                    _movingRight = true;  //moves right 
                }
                else
                {
                    double distance = AntiGlitch(rects, _rectLeft, speed, 0);
                    _location.X -= distance;
                }

            }

            if (yVelocity >= 0 && onAir(rects))
            {
                moving = true;
                yVelocity += gravityVlc;            //faster falling speed as the player is falling 

                if (Math.Abs(yVelocity) > 30)
                {
                    yVelocity = yVelocity < 0 ? -30 : 30;             //cap at 50
                }
                double speed = AntiGlitch(rects, _rectDown, 0, yVelocity);
                _location.Y += speed;
                if (speed == 0)     //indicate ground collision
                {
                    yVelocity = 0;      //stops falling
                }
            }




            Draw();
        }
        //Logics check for Goomba collision with Mario
        //Minus one life or game over 
        public override int Lives(int lives)
        {
            Console.WriteLine("Lost a life!");
            return -1;
        }
        public override bool CheckCollision(Rectangle Mario_hitbox)
        {
            if (_hitbox.IntersectsWith(Mario_hitbox) && !_iframe)
            {
                Console.WriteLine(" Goomba ");
                _iframe = true;
                _iframeStart = DateTime.Now;

                return true;
            }
            if (_iframe && (DateTime.Now - _iframeStart).TotalMilliseconds >= iframeDuration)           //Mario will have 1 second invincibiliy timer before he takes dmg from the same Goomba again
            {
                _iframe = false;
            }

            return false;
        }


        public override void Draw()
        {
            SplashKit.DrawBitmap(_moving, _location.X, _location.Y);
        }



    }


    public class Mushroom : GameCharacter
    {
        public Bitmap mushroom;
        Point2D _location;

        const int speed = 7;
        private Rectangle _rectUp;
        private Rectangle _rectDown;
        private Rectangle _rectLeft;
        private Rectangle _rectRight;

        private Rectangle _hitbox;

        SoundEffect _1up;


        public Mushroom(double x, double y)
        {
            mushroom = new Bitmap("mushroom", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\mushroom2.png");
            _location.X = x;
            _location.Y = y;
            _1up = SplashKit.LoadSoundEffect("1Up", "F:\\Projects\\repo\\CustomMario\\Resources\\SFX\\SFX1up.mp3");
        }


        public override void setHitbox()
        {
            _rectUp = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y), 60, 1);
            _rectDown = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y) + 60, 60, 1);
            _rectLeft = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y), 1, 60);
            _rectRight = new Rectangle(Convert.ToInt32(_location.X) + 60, Convert.ToInt32(_location.Y), 1, 60);

            _hitbox = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y), 60, 57);


        }

        public void debugLocation()
        {
            SplashKit.DrawText("X: " + _location.X / 75, Color.Black, _location.X + 10, _location.Y - 30);
            SplashKit.DrawText("Y: " + _location.Y, Color.Black, _location.X + 10, _location.Y - 20);
        }

        float yVelocity = 0;
        bool moving;
        float gravityVlc = 1;
        bool _movingRight = true;


        bool _iframe = false;
        DateTime _iframeStart;
        const int iframeDuration = 1000; // 1 second


        public Boolean onAir(List<Rectangle> rects)
        {

            foreach (Rectangle rect in rects)
            {

                if (_rectDown.IntersectsWith(rect))         //if bottom hitbox intersects with the terrain tiles, it means is on the ground.
                {
                    return false;
                }

            }
            return true;            //else is on air
        }

        public override void Moving(List<Rectangle> rects, Rectangle Mario_hitbox, Rectangle mario_rectDown, int lives)
        {
            setHitbox();


            if (_movingRight)                       //Patrolling behaviour
            {
                if (Collision(rects, _rectRight))
                {
                    _movingRight = false; // Switch direction    
                }
                else
                {
                    double distance = AntiGlitch(rects, _rectRight, -speed, 0);
                    _location.X -= distance;
                }
            }
            else
            {
                if (Collision(rects, _rectLeft))
                {
                    _movingRight = true;  //moves right 
                }
                else
                {
                    double distance = AntiGlitch(rects, _rectLeft, speed, 0);
                    _location.X -= distance;
                }

            }


            //falling
            if (yVelocity >= 0 && onAir(rects))
            {
                moving = true;
                yVelocity += gravityVlc;            //faster falling speed while falling 

                if (Math.Abs(yVelocity) > 30)
                {
                    yVelocity = yVelocity < 0 ? -30 : 30;             //cap at 50
                }
                double speed = AntiGlitch(rects, _rectDown, 0, yVelocity);
                _location.Y += speed;
                if (speed == 0)     //indicate ground collision
                {
                    yVelocity = 0;      //stops falling
                }
            }

            Draw();
        }


        public override bool CheckCollision(Rectangle Mario_hitbox)
        {
            if (_hitbox.IntersectsWith(Mario_hitbox) && !_iframe)
            {
                _iframe = true;
                _iframeStart = DateTime.Now;
                return true;
            }
            if (_iframe && (DateTime.Now - _iframeStart).TotalMilliseconds >= iframeDuration)           //Mario will have 1 second invincibiliy timer before he takes dmg from the same Goomba again
            {
                _iframe = false;
            }

            return false;
        }

        public override int Lives(int lives)
        {
            Console.WriteLine("Gained a life");
            SplashKit.PlaySoundEffect(_1up);
            return 1;
        }

        public override void Draw()
        {
            SplashKit.DrawBitmap(mushroom, _location.X, _location.Y);
        }

    }


    public class coinList
    {
        List<Coin> _coinList;
        public coinList()
        {
            _coinList = new List<Coin>();
        }

        public void Add(Coin coin)
        {
            _coinList.Add(coin);
        }

        public void Remove(Coin coin)
        {
            _coinList.Remove(coin);
        }
        public List<Coin> getCoinList()
        {
            return _coinList;
        }
        public void InitializeCoins(double[,] positions)
        {
            for (int i = 0; i < positions.GetLength(0); i++)
            {
                double x = positions[i, 0];
                double y = positions[i, 1];
                _coinList.Add(new Coin(x, y));
            }
        }





        public int Draw(Rectangle Mario_hitbox)
        {
            for (int i = 0; i < _coinList.Count; i++)
            {
                Coin coin = _coinList[i];
                coin.setHitbox();
                coin.Draw();
                if (coin.CheckCollision(Mario_hitbox))
                {
                    _coinList.Remove(coin);
                    return 1;
                }

            }
            return 0;
        }
    }


    public class Coin : GameCharacter
    {
        Bitmap _coin;
        Point2D _location;

        const int speed = 0;
        private Rectangle _rectUp;
        private Rectangle _rectDown;
        private Rectangle _rectLeft;
        private Rectangle _rectRight;

        private Rectangle _hitbox;

        SoundEffect coinSFX;


        public Coin(double x, double y)
        {
            _coin = new Bitmap("Coinsss", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\coin4.png");
            _location.X = x * 60;
            _location.Y = y;
            coinSFX = SplashKit.LoadSoundEffect("Coin", "F:\\Projects\\repo\\CustomMario\\Resources\\SFX\\SFXcoin.mp3");
        }


        public override void setHitbox()
        {
            _rectUp = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y), 48, 1);
            _rectDown = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y) + 48, 48, 1);
            _rectLeft = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y), 1, 48);
            _rectRight = new Rectangle(Convert.ToInt32(_location.X) + 48, Convert.ToInt32(_location.Y), 1, 48);

            _hitbox = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y), 48, 48);


        }

        bool _movingRight = true;


        public Boolean onAir(List<Rectangle> rects)
        {

            foreach (Rectangle rect in rects)
            {

                if (_rectDown.IntersectsWith(rect))         //if bottom hitbox intersects with the terrain tiles, it means is on the ground.
                {
                    return false;
                }

            }
            return true;            //else is on air
        }

        public override void Moving(List<Rectangle> rects, Rectangle Mario_hitbox, Rectangle mario_rectDown, int lives)
        {
            setHitbox();
         

            if (_movingRight)                       //Patrolling behaviour
            {
                if (Collision(rects, _rectRight))
                {
                    _movingRight = false; // Switch direction    
                }
                else
                {
                    double distance = AntiGlitch(rects, _rectRight, -speed, 0);
                    _location.X -= distance;
                }
            }
            else
            {
                if (Collision(rects, _rectLeft))
                {
                    _movingRight = true;  //moves right 
                }
                else
                {
                    double distance = AntiGlitch(rects, _rectLeft, speed, 0);
                    _location.X -= distance;
                }

            }


            Draw();
        }


        public override bool CheckCollision(Rectangle Mario_hitbox)
        {
            if (_hitbox.IntersectsWith(Mario_hitbox))
            {
                SplashKit.PlaySoundEffect(coinSFX);
                return true;
            }
            return false;
        }

        public override int Lives(int lives)
        {
            return 0;
        }
        public override void Draw()
        {
            SplashKit.DrawBitmap(_coin, _location.X, _location.Y);
        }

    }
}
    