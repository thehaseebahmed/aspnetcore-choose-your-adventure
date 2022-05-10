using System;
using System.Collections.Generic;
using Tha.ChooseYourAdventure.Data.Interfaces;

namespace Tha.ChooseYourAdventure.Data.Entities
{
    public class AdventureNode : IEntity, IAuditable<int>
    {
        public Guid Id { get; set; }

        public AdventureNode()
        {
            Children = new List<AdventureNode>();
        }

        public ICollection<AdventureNode> Children { get; set; }
        public bool IsRootNode { get; set; }
        public string Name { get; set; }
        public string OptionTitle { get; set; }

        public int CreatedById { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int UpdatedById { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
