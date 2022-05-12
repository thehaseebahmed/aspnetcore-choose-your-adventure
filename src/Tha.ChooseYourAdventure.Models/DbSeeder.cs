using System;
using System.Linq;
using Tha.ChooseYourAdventure.Data.Entities;

namespace Tha.ChooseYourAdventure.Data
{
    public class DbSeeder : IDbSeeder
    {
        private ApiDbContext Context { get; }

        public DbSeeder(
            ApiDbContext context
            )
        {
            Context = context;
        }

        public void Seed()
        {
            Context.Database.EnsureCreated();

            var doughnutAdventure = new AdventureNode()
            {
                Id = Guid.Parse("07a3df4a-cd28-4b54-a04f-3bad824e00c1"),
                IsRootNode = true,
                Name = "Do i want a doughnut?",
                OptionTitle = "Doughnut Decision Helper",
                Children = new[] {
                    new AdventureNode() {
                        Id = Guid.Parse("40700ac8-e618-48c6-bb1f-43dca30b53fe"),
                        Name = "Maybe you want an apple?",
                        OptionTitle = "No"
                    },
                    new AdventureNode() {
                        Id = Guid.Parse("1dde68e6-6ee6-4fa1-a8ea-f6dec530fbad"),
                        Name = "Do I deserve it?",
                        OptionTitle = "Yes",
                        Children = new[] {
                            new AdventureNode() {
                                Id = Guid.Parse("649c5ea3-34ba-43f3-8fc3-518a097c28d6"),
                                Name = "Are you sure?",
                                OptionTitle = "Yes",
                                Children = new[] {
                                    new AdventureNode() {
                                        Id = Guid.Parse("eeee44f0-ba65-49c2-9230-ace3fe48605a"),
                                        Name = "Get it.",
                                        OptionTitle = "Yes",
                                    },
                                    new AdventureNode() {
                                        Id = Guid.Parse("ef966a4d-22c7-482b-a6de-1248433112d4"),
                                        Name = "Do jumping jacks first.",
                                        OptionTitle = "No"
                                    }
                                }
                            },
                            new AdventureNode() {
                                Id = Guid.Parse("e1578374-1414-4dc5-be6f-7af5bd43c5f3"),
                                Name = "Is it a good doughnut?",
                                OptionTitle = "No",
                                Children = new[] {
                                    new AdventureNode() {
                                        Id = Guid.Parse("df5aebb2-8d38-45b7-acde-bd25341a05f1"),
                                        Name = "What are you waiting for? Grab it now.",
                                        OptionTitle = "Yes",
                                    },
                                    new AdventureNode() {
                                        Id = Guid.Parse("e2f40f7a-f87e-45b3-86fb-5e012249748e"),
                                        Name = "Wait til you find a sinful, unforgetable doughnut.",
                                        OptionTitle = "No"
                                    }
                                }
                            }
                        }
                    }
                }
            };
            if (!Context.Adventures.Any(a => a.Id.Equals(doughnutAdventure.Id)))
            {
                Context.Adventures.AddRange(doughnutAdventure);
            }

            Context.SaveChanges();
        }
    }
}
