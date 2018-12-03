namespace Task3
{
    using System;
    using System.Threading;

    public class Task3
    {
        private SubTask[] subTasks =
        {
                new BoxAreaCalculation(),
                new RightTriangle(),
                new IsoscelesTriangle(),
                new Triangles(),
                new FilteredArraySum(),
                new FontAttributes(),
                new ArrayTask1(),
                new CubeArray(),
                new PositiveElementSum(),
                new EvenElementsSum(),
                new AverageWordLength(),
                new DoubledSymbols(),
                new StringAndStringBuilder()
        };

        /// <summary>
        /// Shows menu to user
        /// </summary>
        public void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the lab \"Basics of C#\"\n");
            Console.WriteLine("These subtasks are implemented:\n");
            for (int i = 0; i < this.subTasks.Length; i++)
            {
                Console.WriteLine("{0}. {1}", i, this.subTasks[i].TellAboutMyself());
            }

            Console.WriteLine();
        }

        /// <summary>
        ///  Main functionality of an object is here
        /// </summary>
        public void Run()
        {
            int userInput;
            string sentence;
            ConsoleKeyInfo keyPressed;
            do
            {
                this.ShowMenu();
                Console.Write("Please make your choice (any other input to exit) :");
                sentence = Console.ReadLine();
                if (!int.TryParse(sentence, out userInput)
                   || userInput < 0 || userInput >= this.subTasks.Length)
                {
                    Console.WriteLine("Bye-bye!");
                    Thread.Sleep(2000);
                    return;
                }

                do
                {
                    this.subTasks[userInput].Run();
                    do
                    {
                        Console.WriteLine("press Enter to repeat current task, Space to menu, ESC key to exit");
                        keyPressed = Console.ReadKey();
                    }
                    while (!(keyPressed.Key == ConsoleKey.Escape || keyPressed.Key == ConsoleKey.Enter || keyPressed.Key == ConsoleKey.Spacebar));
                    if (keyPressed.Key == ConsoleKey.Escape)
                    {
                        return;
                    }
                }
                while (keyPressed.Key == ConsoleKey.Enter);
            }
            while (true);
        }
    }
}
