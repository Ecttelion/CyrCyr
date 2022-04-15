using Cyrilusconsolus.Games.Cyrilus;

public static class Program

{

    public static void Main(string[] args)
    {
        new CyrilusGamus().Start();
        Console.Clear();

        var name = "cyr";
        Console.WriteLine($"coucou {name} <3");

        while (true)
        {
            var data = Console.ReadLine();
            switch (data)
            {
                case "quit":
                    return;
                case "test":

                    break;

                case "play":
                    Console.WriteLine("0: linetest");
                    Console.WriteLine("1: Maze");
                    Console.WriteLine("2: Physics");
                    Console.WriteLine("3: Snake");
                    Console.WriteLine("4: Tetris");
                    Console.WriteLine("5: CyrilusGamus");
                    Console.WriteLine("6: quit");
                    var value = ReadInt("type 0-6");


                    switch (value)
                    {
                        case 0:
                            new Cyrilusconsolus.Games.LineTest().Start();
                            Console.Clear();
                            break;
                        case 1:
                            new Cyrilusconsolus.Games.Maze().Start();
                            Console.Clear();
                            break;
                        case 2:
                            new Cyrilusconsolus.Games.Physics().Start();
                            Console.Clear();
                            break;
                        case 3:
                            new Cyrilusconsolus.Games.Snake().Start();
                            Console.Clear();
                            break;
                        case 4:
                            new Cyrilusconsolus.Games.Tetris().Start();
                            Console.Clear();
                            break;
                        case 5:
                            new CyrilusGamus().Start();
                            Console.Clear();
                            break;
                        case 6:
                            return;
                        default:
                            break;

                    }


                    break;
                case "calc":

                    break;
                case "...":

                    break;
                default:
                    break;
            }
        }


    }

    public static int ReadInt(string message)
    {
        var continueDo = true;
        int outVal;
        do
        {
            Console.WriteLine(message);
            continueDo = !int.TryParse(Console.ReadLine(), out outVal);
        } while (continueDo);

        return outVal;
    }

}