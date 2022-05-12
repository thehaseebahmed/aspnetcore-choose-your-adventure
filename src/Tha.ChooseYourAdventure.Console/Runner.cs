using RestEase;

namespace Tha.ChooseYourAdventure.Client
{
    public sealed class Runner
    {
        private readonly IApi _api;

        public Runner()
        {
            _api = RestClient.For<IApi>("https://localhost:44310/api/v1");
        }

        public async Task Run()
        {
            Console.Clear();

            // Console.Write("Enter User Id? ");
            // var userId = Guid.Parse(Console.ReadLine() ?? "00000000-0000-0000-0000-000000000001");
            var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            int userOption = -1;
            do
            {
                Console.Write("1.Start a New Adventure\n2.Continue an Adventure\n3.Show All of My Adventures\n0.Exit\n\nChoose an option? ");
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
                Console.WriteLine($"{i + 1}. {adventure.Name} - {status}");
                if (status == "Completed") { PrintAdventurePath(adventure); }
            }
        }

        private void PrintAdventurePath(GetUserAdventuresViewModel adventure)
        {
            for (int i = 0; i < adventure.Steps.Count; i++)
            {
                var spaces = (4 * i) + 3;
                for (int j = 0; j < spaces; j++) { Console.Write(" "); }
                if (spaces > 3) { Console.Write("-> "); }

                var step = adventure.Steps.ElementAt(i);
                if (i == adventure.Steps.Count - 1) { Console.WriteLine($"{step.Name}"); }
                else { Console.WriteLine($"{step.Name} {step.OptionTitle}"); }
            }

            Console.WriteLine();
        }
    }
}
