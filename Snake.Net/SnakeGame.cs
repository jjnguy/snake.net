using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake.Net
{
    public class SnakeGame
    {
        private int X;
        private int Y;

        private ConsoleKey Direction;

        private int TailLength;
        private List<Tuple<int, int>> History;

        private Tuple<int, int> Apple;
        private char AppleChar = 'A';

        public SnakeGame()
        {
            X = 0;
            Y = 0;
            TailLength = 0;
            History = new List<Tuple<int, int>>();
            Direction = ConsoleKey.DownArrow;
            Apple = null;
        }

        private char SnakeHead()
        {
            switch (Direction)
            {
                case ConsoleKey.DownArrow: return 'V';
                case ConsoleKey.UpArrow: return '^';
                case ConsoleKey.LeftArrow: return '<';
                case ConsoleKey.RightArrow: return '>';
                default: throw new Exception("Bad value for direction: " + Direction);
            }
        }

        private long TicksPerFrame = 1000000;

        public void Run()
        {
            while (true)
            {
                var frameStart = DateTime.UtcNow.Ticks;
                ClearDisplay();
                DrawGame();
                ProcessInput();
                Advance();
                AddApple();
                var frameEnd = DateTime.UtcNow.Ticks;
                var totalTicks = frameEnd - frameStart;
                Thread.Sleep((int)((TicksPerFrame - totalTicks) / TimeSpan.TicksPerMillisecond));
            }
        }

        Random r = new Random();
        private void AddApple()
        {
            if (Apple == null)
            {
                var maxY = Console.WindowHeight;
                var maxX = Console.WindowWidth;
                var appleX = r.Next(0, maxX);
                var appleY = r.Next(0, maxY);

                Apple = Tuple.Create(appleX, appleY);
            }
        }

        private void ClearDisplay()
        {
            Console.Clear();
        }

        private void DrawGame()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(SnakeHead());
            
            if (Apple != null)
            {
                Console.SetCursorPosition(Apple.Item1, Apple.Item2);
                Console.Write(AppleChar);
            }
            foreach (var point in History.Take(TailLength))
            {
                Console.SetCursorPosition(point.Item1, point.Item2);
                Console.Write('0');
            }
        }

        private void Advance()
        {
            History.Insert(0, Tuple.Create(X, Y));
            switch (Direction)
            {
                case ConsoleKey.DownArrow: Y++; break;
                case ConsoleKey.UpArrow: Y--; break;
                case ConsoleKey.LeftArrow: X--; break;
                case ConsoleKey.RightArrow: X++; break;
                default: throw new Exception("Bad value for direction: " + Direction);
            }

            if (Apple != null && X == Apple.Item1 && Y == Apple.Item2)
            {
                TailLength++;
                Apple = null;
            }
        }

        private ConsoleKey[] movekeys = new ConsoleKey[] { ConsoleKey.DownArrow, ConsoleKey.UpArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow };

        private void ProcessInput()
        {
            if (!Console.KeyAvailable) return;
            var key = Console.ReadKey(true);
            if (!movekeys.Contains(key.Key))
            {
                return;
            }

            Direction = key.Key;
        }
    }
}
