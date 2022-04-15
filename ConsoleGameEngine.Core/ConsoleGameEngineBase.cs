using ConsoleGameEngine.Core.GameObjects;
using ConsoleGameEngine.Core.Input;
using ConsoleGameEngine.Core.Math;
using ConsoleGameEngine.Core.Win32;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;


namespace ConsoleGameEngine.Core
{
    public abstract class ConsoleGameEngineBase : ConsoleGameEngineWin32
    {
        private CharInfo[] _screenBuffer;
        private bool _isInit;

        private bool _gameRunning;
        private readonly KeyboardInput _input;
        private int _targetFps;

        /// <summary>
        /// The name of the game
        /// </summary>
        protected abstract string Name { get; }

        /// <summary>
        /// Shortcut to grab the Screen Width
        /// </summary>
        public int ScreenWidth => (int)ScreenRect.Size.X;

        /// <summary>
        /// Shortcut to grab the Screen Height
        /// </summary>
        public int ScreenHeight => (int)ScreenRect.Size.Y;

        /// <summary>
        /// The bounds of the screen
        /// </summary>
        public Rect ScreenRect { get; private set; }

        /// <summary>
        /// Enabling performance mode allows the game loop to run as fast as possible
        /// instead of suspending the game loop thread to hit the target framerate.
        /// This uses a lot more CPU.
        /// </summary>
        protected bool PerformanceModeEnabled { get; init; }

        protected ConsoleGameEngineBase()
        {
            _isInit = false;
            PerformanceModeEnabled = false;
            _input = new KeyboardInput();
        }

        /// <summary>
        /// Initializes the Console to the specified size, must be called before Start()
        /// </summary>
        /// <param name="width">The desired Width of the console in columns (not pixels)</param>
        /// <param name="height">The desired Height of the console in rows (not pixels)</param>
        /// <param name="pixelSize">The size of each cell in pixels</param>
        /// <param name="targetFps">The target framerate the application should aim to achieve</param>
        protected void InitConsole(int width, int height, short pixelSize = 8, int targetFps = 60)
        {
            if (!_isInit)
            {
                Console.OutputEncoding = Encoding.UTF8;

                if (pixelSize < 4) pixelSize = 4;
                SetCurrentFont("Modern DOS 8x8", pixelSize);

                // Clamp width and height while maintaining aspect ratio
                var maxWidth = Console.LargestWindowWidth - 1;
                var maxHeight = Console.LargestWindowHeight - 1;

                if (width > maxWidth || height > maxHeight)
                {
                    var widthRatio = (float)maxWidth / width;
                    var heightRatio = (float)maxHeight / height;

                    // use whichever multiplier is smaller
                    var ratio = widthRatio < heightRatio ? widthRatio : heightRatio;

                    width = (int)(width * ratio);
                    height = (int)(height * ratio);
                }

                ScreenRect = new Rect(Vector.Zero, new Vector(width, height));

                _screenBuffer = new CharInfo[width * height];

                Console.SetWindowSize(width, height);
                Console.SetBufferSize(width, height);

                _targetFps = targetFps;
                if (_targetFps < 30) _targetFps = 30;
                _isInit = true;
            }
            else
            {
                throw new InvalidOperationException("Console is already initialized.");
            }
        }

        /// <summary>
        /// Starts the game loop, must call InitConsole first to set dimensions and target fps.
        /// </summary>
        public void Start()
        {
            if (!_isInit)
            {
                throw new InvalidOperationException("Console Window must be initialized with InitConsole() before calling Start()");
            }

            _gameRunning = true;
            GameLoop();
        }

        private void GameLoop()
        {
            if (!Create())
            {
                _gameRunning = false;
            }

            long framesRendered = 0;

            var timer = new Stopwatch();
            timer.Start();

            var previousTime = timer.Elapsed.TotalMilliseconds;
            while (_gameRunning)
            {
                var currentTime = timer.Elapsed.TotalMilliseconds;
                var elapsedTime = (currentTime - previousTime);
                previousTime = currentTime;

                _input.Update();

                // Game Logic
                if (!Update((float)elapsedTime / 1000f, _input))
                {
                    _gameRunning = false;
                }

                // Render to screen
                DrawBuffer(_screenBuffer, ScreenWidth, ScreenHeight);

                var averageFps = ++framesRendered / (timer.Elapsed.TotalMilliseconds / 1000f);
                Console.Title = $"{Name} ~ Average FPS: {averageFps:F}";

                if (!PerformanceModeEnabled)
                {
                    // Give back some system resources by suspending the thread if update loop takes less time than necessary to hit our target FPS.
                    // This vastly reduces CPU usage!
                    var waitTime = 1f / _targetFps * 1000f - elapsedTime;
                    if (waitTime > 0)
                    {
                        Thread.Sleep((int)waitTime);
                    }
                }
            }
        }

        /// <summary>
        /// Runs at startup, used to set up game data.
        /// This function should return true if setup was successful, false otherwise.
        /// </summary>
        protected abstract bool Create();

        /// <summary>
        /// The main game loop, runs once per frame.
        /// If this function returns false, the game loop stops and the application is terminated.
        /// </summary>
        /// <param name="elapsedTime">The elapsed time since the last frame (in seconds)</param>
        /// <param name="input">The keyboard input state for the current frame</param>
        protected abstract bool Update(float elapsedTime, KeyboardInput input);

        /// <summary>
        /// Draws a character to the screen at the given position.
        /// </summary>
        public void Draw(Vector position, char c, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
        {
            Draw((int)position.X, (int)position.Y, c, fgColor, bgColor);
        }

        /// <summary>
        /// Draws a character to the screen at the given position.
        /// </summary>
        public void Draw(int x, int y, char c, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
        {
            if (x >= ScreenWidth || x < 0 ||
                y >= ScreenHeight || y < 0)
            {
                return;
            }

            var index = y * ScreenWidth + x;
            var color = (short)((int)fgColor + ((int)bgColor << 4));

            _screenBuffer[index].Attributes = color;
            _screenBuffer[index].Char.UnicodeChar = c;
        }

        public void Draw(int x, int y, char c, int fgColor, int bgColor)
        {
            if (x >= ScreenWidth || x < 0 ||
                y >= ScreenHeight || y < 0)
            {
                return;
            }

            var index = y * ScreenWidth + x;
            var color = (short)((int)fgColor + ((int)bgColor << 4));

            _screenBuffer[index].Attributes = color;
            _screenBuffer[index].Char.UnicodeChar = c;
        }

        /// <summary>
        /// Draws a rectangle to the screen.
        /// </summary>
        public void Fill(Rect rect, char c, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
        {
            Fill(rect.Position, rect.Size, c, fgColor, bgColor);
        }

        public void Fill(Vector position, Vector size, char c, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
        {
            Fill((int)position.X,
                 (int)position.Y,
                 (int)(size.X + position.X),
                 (int)(size.Y + position.Y),
                 c, fgColor, bgColor);
        }

        public void Fill(int x1, int y1, int x2, int y2, char c, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
        {
            Clip(ref x1, ref y1);
            Clip(ref x2, ref y2);

            for (int y = y1; y < y2; y++)
            {
                for (int x = x1; x < x2; x++)
                {
                    Draw(x, y, c, fgColor, bgColor);
                }
            }
        }

        /// <summary>
        /// Draws a sprite to the screen using the sprite's position.
        /// </summary>
        public void DrawSprite(Sprite sprite)
        {
            for (var y = 0; y < sprite.Height; y++)
            {
                for (int x = 0; x < sprite.Width; x++)
                {
                    if (sprite.GetGlyph(x, y) != ' ')
                    {
                        Draw(
                            (int)sprite.Position.X + x,
                            (int)sprite.Position.Y + y,
                            sprite.GetGlyph(x, y),
                            sprite.GetFgColor(x, y),
                            sprite.GetBgColor(x, y));
                    }
                }
            }
        }

        /// <summary>
        /// Draws a line from the starting point to the ending point.
        /// </summary>
        public void DrawLine(Vector start, Vector end, char c, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
        {
            DrawLine(
                (int)start.X, (int)start.Y,
                (int)end.X, (int)end.Y,
                c, fgColor, bgColor);
        }

        /// <summary>
        /// Draws a line from the starting point to the ending point.
        /// </summary>
        public void DrawLine(int x1, int y1, int x2, int y2, char c, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
        {
            int x;
            int y;

            var dx = x2 - x1;
            var dy = y2 - y1;

            var dx1 = System.Math.Abs(dx);
            var dy1 = System.Math.Abs(dy);

            var px = 2 * dy1 - dx1;
            var py = 2 * dx1 - dy1;

            if (dy1 <= dx1)
            {
                int xe;
                if (dx >= 0)
                {
                    x = x1;
                    y = y1;
                    xe = x2;
                }
                else
                {
                    x = x2;
                    y = y2;
                    xe = x1;
                }

                Draw(x, y, c, fgColor, bgColor);

                for (var i = 0; x < xe; i++)
                {
                    x = x + 1;
                    if (px < 0)
                    {
                        px += 2 * dy1;
                    }
                    else
                    {
                        if (dx < 0 && dy < 0 || dx > 0 && dy > 0)
                        {
                            y += 1;
                        }
                        else
                        {
                            y -= 1;
                        }
                        px += 2 * (dy1 - dx1);
                    }

                    Draw(x, y, c, fgColor, bgColor);
                }
            }
            else
            {
                int ye;
                if (dy >= 0)
                {
                    x = x1;
                    y = y1;
                    ye = y2;
                }
                else
                {
                    x = x2;
                    y = y2;
                    ye = y1;
                }

                Draw(x, y, c, fgColor, bgColor);

                for (var i = 0; y < ye; i++)
                {
                    y += 1;
                    if (py <= 0)
                    {
                        py += 2 * dx1;
                    }
                    else
                    {
                        if (dx < 0 && dy < 0 || dx > 0 && dy > 0)
                        {
                            x += 1;
                        }
                        else
                        {
                            x -= 1;
                        }
                        py += 2 * (dx1 - dy1);
                    }

                    Draw(x, y, c, fgColor, bgColor);
                }
            }
        }


        /// <summary>
        /// Draws a string of text to the screen at the given coordinates.
        /// </summary>
        public void DrawString(Vector position, string msg, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black, bool centered = false)
        {
            DrawString(
                (int)position.X,
                (int)position.Y,
                msg,
                fgColor,
                bgColor,
                centered);
        }

        /// <summary>
        /// Draws a string of text to the screen at the given coordinates.
        /// </summary>
        public void DrawString(int x, int y, string msg, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black, bool centered = false)
        {
            if (centered)
            {
                x -= msg.Length / 2;
            }

            for (int i = 0; i < msg.Length; i++)
            {
                Draw(x + i, y, msg[i], fgColor, bgColor);
            }
        }

        private void Clip(ref int x, ref int y)
        {
            if (x < 0) x = 0;
            if (x >= ScreenWidth) x = ScreenWidth;
            if (y < 0) y = 0;
            if (y >= ScreenHeight) y = ScreenHeight;
        }
    }
}
