using RestEase;

namespace Tha.ChooseYourAdventure.Client
{
    public sealed class Runner
    {
        private readonly IApi _api = RestClient.For<IApi>("https://localhost:52027/api/v1");

        public async Task Run()
        {
            Console.Clear();

            Guid userId = Guid.Empty;
            do
            {
                Console.Write("Enter User Id? ");
                Guid.TryParse(Console.ReadLine(), out userId);

                if (userId == Guid.Empty) { Console.WriteLine("User Id is not a GUID, please try again!"); }
            } while (userId == Guid.Empty);

            Console.WriteLine("Starting the application...");
            Console.Clear();

            int userOption = -1;
            do
            {
                Console.Write($"Current User: {userId}\n\n1.Start a New Adventure\n2.Continue an Adventure\n3.Show All of My Adventures\n0.Exit\n\nChoose an option? ");
                userOption = int.Parse(Console.ReadLine() ?? "-1");

                var actions = new Dictionary<int, Func<Task>>()
                {
                    { 1, async () => await PlayAnAdventure(userId) },
                    { 2, async () => await ContinueMyAdventures(userId) },
                    { 3, async () => await ShowMyAdventures(userId) }
                };

                if (actions.ContainsKey(userOption))
                {
                    await actions[userOption]();

                    Console.WriteLine("Press any key to return to the main menu.");
                    _ = Console.ReadLine();
                    Console.Clear();
                }
                else if (userOption != 0)
                {
                    Console.WriteLine("Invalid option! Press any key to choose again.");
                    _ = Console.ReadLine();
                }

                Console.Clear();
            } while (userOption != 0);
        }

        private async Task ContinueMyAdventures(Guid userId)
        {
            Console.WriteLine("Loading adventures, please wait!");
            var adventures = await _api.GetUserAdventures(userId);
            Console.Clear();

            for (int i = 0; i < adventures.Data.Count(); i++)
            {
                var adventure = adventures.Data.ElementAt(i);
                if (adventure.Status == UserAdventureStatus.InProgress)
                {
                    var status = adventure.Status == UserAdventureStatus.InProgress ? "In Progress" : "Completed";
                    Console.WriteLine($"{i + 1}. {adventure.Name} - {status}");
                }
            }
        }

        private async Task PlayAnAdventure(Guid userId)
        {
            Console.WriteLine("Loading adventures, please wait!");
            var adventures = await _api.GetAdventuresAsync();
            Console.Clear();

            Console.WriteLine("Here is a list of adventures that you can enjoy:");
            for (int i = 0; i < adventures.Data.Count(); i++)
            {
                Console.WriteLine($"{i + 1}. {adventures.Data.ElementAt(i).OptionTitle}");
            }

            Console.Write("\nChoose an adventure? ");
            var adventureIndex = int.Parse(Console.ReadLine() ?? "-1");
            var choosenAdventureId = adventures.Data.ElementAt(adventureIndex - 1).Id;

            Console.WriteLine("Starting adventure, please wait!");
            var userAdventure = await _api.CreateUserAdventure(
                userId,
                new CreateUserAdventureRequestModel { UserId = userId, AdventureId = choosenAdventureId }
                );

            var choosenAdventure = await _api.GetAdventureByIdAsync(choosenAdventureId);
            Console.Clear();

            while (choosenAdventure.Children.Count() > 0)
            {
                try
                {
                    Console.WriteLine(choosenAdventure.Name);
                    for (int i = 0; i < choosenAdventure.Children.Count(); i++)
                    {
                        Console.WriteLine($"{i + 1}. {choosenAdventure.Children.ElementAt(i).OptionTitle}");
                    }

                    Console.Write("\nChoose an option? ");
                    var optionIndex = int.Parse(Console.ReadLine() ?? "-1");
                    var choosenOptionId = choosenAdventure.Children.ElementAt(optionIndex - 1).Id;

                    Console.WriteLine("Saving, please wait...");
                    _ = await _api.UpdateUserAdventure(
                        userId,
                        userAdventure.Id,
                        new UpdateUserAdventureRequestModel()
                        {
                            AdventureStepId = choosenOptionId,
                            UserAdventureId = userAdventure.Id,
                            UserId = userId
                        });

                    choosenAdventure = await _api.GetAdventureByIdAsync(choosenOptionId);
                    Console.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Oops! Something went wrong...");
                    Console.WriteLine("Press any key to retry.");
                    _ = Console.ReadLine();
                    Console.Clear();
                }
            }

            if (choosenAdventure.Children.Count() < 1)
            {
                Console.WriteLine($"{choosenAdventure.Name}");
            }
        }

        private async Task ShowMyAdventures(Guid userId)
        {
            Console.WriteLine("Loading adventures, please wait!");
            var adventures = await _api.GetUserAdventures(userId);
            Console.Clear();

            for (int i = 0; i < adventures.Data.Count(); i++)
            {
                var adventure = adventures.Data.ElementAt(i);
                var status = adventure.Status == UserAdventureStatus.InProgress ? "In Progress" : "Completed";
                Console.WriteLine($"{i + 1}. {adventure.OptionTitle} - {status}");
                if (status == "Completed") { PrintAdventurePath(adventure); }
            }
        }

        private void PrintAdventurePath(GetUserAdventuresViewModel adventure)
        {
            Console.Write($"   {adventure.Name}");

            for (int i = 0; i < adventure.Steps.Count; i++)
            {
                var step = adventure.Steps.ElementAt(i);
                Console.WriteLine($" -> {step.OptionTitle}");

                var spaces = 3;
                for (int j = 0; j < spaces; j++) { Console.Write(" "); }

                Console.Write($"{step.Name}");
            }

            Console.WriteLine();
        }
    }
}
