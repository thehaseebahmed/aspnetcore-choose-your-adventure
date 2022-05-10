using System;
using System.Collections.Generic;
using Tha.ChooseYourAdventure.Data.Enums;
using Tha.ChooseYourAdventure.Data.Interfaces;

namespace Tha.ChooseYourAdventure.Data.Entities
{
    public class UserAdventure : IEntity, IAuditable<int>
    {
        public Guid Id { get; set; }
        public Guid AdventureId { get; set; }

        public UserAdventure()
        {
            Steps = new List<UserAdventureStep>();
            Status = UserAdventureStatus.InProgress;
        }

        public virtual AdventureNode Adventure { get; set; }
        public virtual ICollection<UserAdventureStep> Steps { get; set; }
        public UserAdventureStatus Status { get; set; }
        public Guid UserId { get; set; }

        public int CreatedById { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int UpdatedById { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
