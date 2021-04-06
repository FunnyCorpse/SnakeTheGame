using System;
using System.Threading;

/* TO DO:
 * - отрисовка змейки
 * - счётчик очков - ГОТОВО
 * - отселживание конца игры - ГОТОВО НА 50% (ПОКА ТОЛЬКО ПЕРЕСЕЧЕНИЕ ГРАНИЦЫ ПОЛЯ)
 * - управление - ГОТОВО
 * - устранение мигания (отрисовка только изменяющихся элементов?) - ГОТОВО
 * - перейти на ASCII
 */
namespace SnakeTheGame
{
    class Program
    {
        static void Main(string[] args)
        {
            bool check, gameOver = false, eating=false, notFirstEteration = false;
            int area = 0, threadCount = 0, score = 0;
            char symbol, egg = '0';
            char[,] arena;
            int[] eggPosition = {0, 0};
            int[] oldEggPosition = new int[2];
            int[] oldSnakePosition = new int[2];
            ConsoleKey currentDirection = ConsoleKey.LeftArrow;
            Console.Write("Выберите размер игрового поля:\n1) 20*20\n2) 40*40\n3) 60*60\nВаш выбор: ");
            do
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            area = 20;
                            check = true;
                        }
                        break;
                    case "2":
                        {
                            area = 40;
                            check = true;
                        }
                        break;
                    case "3":
                        {
                            area = 60;
                            check = true;
                        }
                        break;
                    default:
                        {
                            Console.WriteLine("Некорректный ввод, попробуйте снова!");
                            check = false;
                        }
                        break;
                }
            } while (!check);
            Console.WriteLine("Введите символ для отрисовки поля");
            symbol = Console.ReadLine()[0];
            int[] snakePosition = { 15, 15 };
            do
            {
                

                eggPosition = EggPosition(eggPosition, threadCount, area);
                currentDirection = Movment(currentDirection);
                snakePosition = SnakePosition(snakePosition, currentDirection);
                eating = Eating(eggPosition, snakePosition);
                if (eating)
                {
                    score++;
                    eggPosition[0] = 0;
                    eggPosition[1] = 0;
                    egg = symbol;

                }
                gameOver = GameOver(snakePosition, area);
                arena = Arena(area, symbol);
                if (!notFirstEteration)
                {
                    Console.Clear();
                    for (int i = 0; i < arena.GetLength(0); i++)
                    {
                        for (int j = 0; j < arena.GetLength(1); j++)
                        {
                            Console.Write(arena[i, j]);
                        }
                        Console.Write("\n");
                    }
                    Console.SetCursorPosition(6, area + 1);
                    Console.Write("Score: {0}", score);
                    notFirstEteration = true;
                }
                else
                {
                    Console.SetCursorPosition(oldEggPosition[0], oldEggPosition[1]);
                    if (oldEggPosition[0] == 0) Console.Write(symbol);
                    else Console.Write('.');
                    Console.SetCursorPosition(eggPosition[0], eggPosition[1]);
                    Console.Write(egg);
                    Console.SetCursorPosition(oldSnakePosition[0], oldSnakePosition[1]);
                    Console.Write('.');
                    Console.SetCursorPosition(snakePosition[0], snakePosition[1]);
                    Console.Write(SnakeHeadSymbol(currentDirection));
                    Console.SetCursorPosition(13, area + 1);
                    Console.Write(score);
                }
                for (int i = 0; i < oldEggPosition.Length; i++)
                {
                    oldEggPosition[i] = eggPosition[i];
                    oldSnakePosition[i] = snakePosition[i];
                }
                threadCount++;
                if (threadCount >= 40)
                {
                    threadCount = 0;
                    egg = '0';
                }
                Thread.Sleep(70);

            } while (!gameOver);

            Console.WriteLine("GameOver!");

            Console.ReadLine();
        }

        static char[,] Arena(int area, char symbol) //Метод для расчёта игрового поля
        {
            bool proverka;
            char[,] arena = new char[area, area];

            
            for (int i = 0; i < arena.GetLength(0); i++) //строки
            {
                if (i == 0 || i == arena.GetLength(0) - 1)
                {
                    proverka = true;
                }
                else proverka = false;
                for (int j = 0; j < arena.GetLength(1); j++) //столбцы
                {
                    if (proverka)
                    {
                        arena[i, j] = symbol;
                    }
                    else if (j == 0 || j == arena.GetLength(1) - 1)
                    {
                        arena[i, j] = symbol;
                    }
                    else arena[i, j] = '.';
                }
            }


            return arena;
        }

        static int [] EggPosition(int[] eggPosition, int threadCount, int area) //расчёт позиции яйца
        {
            Random rnd = new Random();
            if (threadCount==0)
            {
                for (int i = 0; i < eggPosition.Length; i++)
                {
                    eggPosition[i] = rnd.Next(1, area - 1);
                }
            }
            return eggPosition;
        }

        static int [] SnakePosition(int[] snakePosition, ConsoleKey currentDirection) //расчёт положения змениной головы (пока без проверок)|| ПРОВЕРКИ В ОТДЕЛЬНОМ МЕТОДЕ
        {
            switch (currentDirection)
            {

                case ConsoleKey.LeftArrow:
                    {
                        
                        snakePosition[0] = --snakePosition[0];
                    }
                    break;
                case ConsoleKey.UpArrow:
                    {
                        
                        snakePosition[1] = --snakePosition[1];
                    }
                    break;
                case ConsoleKey.RightArrow:
                    {
                       
                        snakePosition[0] = ++snakePosition[0];
                    }
                    break;
                case ConsoleKey.DownArrow:
                    {
                        
                        snakePosition[1] = ++snakePosition[1];
                    }
                    break;
 
                default:
                    break;
                    
            }
            return snakePosition;
        }

        static bool GameOver (int[] snakePosition, int area) //проверка конца игры, пока что только на пересечение границы поля. Срабатывает не на всех сторонах. Почему?||РЕШЕНО
        {
            bool gameOver=false;
            for (int i = 0; i < snakePosition.Length; i++)
            {
                if (snakePosition[i] < 1 || snakePosition[i] >= area-1)
                {
                    gameOver = true;
                }
            }
            return gameOver;
        }

        static ConsoleKey Movment(ConsoleKey currentDirection) //направеление движения, почему-то всегда надо нажимать клавишу, возможно стоит сделать отработку на случай отсутсвия нажатия. РЕШЕНО
        {
            ConsoleKeyInfo consoleKey = new ConsoleKeyInfo();
            if (Console.KeyAvailable == true)
            {
                consoleKey = Console.ReadKey(true);

                switch (consoleKey.Key)
                {
                    case ConsoleKey.LeftArrow:
                        currentDirection = ConsoleKey.LeftArrow;
                        break;
                    case ConsoleKey.UpArrow:
                        currentDirection = ConsoleKey.UpArrow;
                        break;
                    case ConsoleKey.RightArrow:
                        currentDirection = ConsoleKey.RightArrow;
                        break;
                    case ConsoleKey.DownArrow:
                        currentDirection = ConsoleKey.DownArrow;
                        break;
                }
            }
            return currentDirection;
        }

        static char SnakeHeadSymbol (ConsoleKey currentDirection) //меняет символ змеиной головы в зависимости от направления движения
        {
            char headSymbol = ' ';

            switch (currentDirection)
            {
                
                case ConsoleKey.LeftArrow:
                    headSymbol = '<';
                    break;
                case ConsoleKey.UpArrow:
                    headSymbol = '^';
                    break;
                case ConsoleKey.RightArrow:
                    headSymbol = '>';
                    break;
                case ConsoleKey.DownArrow:
                    headSymbol = 'v';
                    break;
                default:
                    break;
            }
            return headSymbol;
        }

        static bool Eating ( int[] eggPosition, int[] snakePosition) //было ли съедено яйцо
        {
            bool eating = false;
            int eggY = eggPosition[0];
            int eggX = eggPosition[1];
            int snakeY = snakePosition[0];
            int snakeX = snakePosition[1];
            if (eggY == snakeY && eggX == snakeX)
            {
                eating = true;
            }


            return eating;
        }
    }
}
