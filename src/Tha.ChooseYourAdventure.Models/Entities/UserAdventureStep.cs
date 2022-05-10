using System;
using Tha.ChooseYourAdventure.Data.Interfaces;

namespace Tha.ChooseYourAdventure.Data.Entities
{
    public class UserAdventureStep : IEntity, IAuditable<int>
    {
        public Guid Id { get; set; }
        public Guid AdventureStepId { get; set; }
        public Guid UserAdventureId { get; set; }

        public UserAdventureStep()
        {

        }

        public virtual AdventureNode AdventureStep { get; set; }
        public Guid UserId { get; set; }

        public int CreatedById { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int UpdatedById { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
