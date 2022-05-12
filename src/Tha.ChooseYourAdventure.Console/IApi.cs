using RestEase;

namespace Tha.ChooseYourAdventure.Client
{
    public interface IApi
    {
        [Get("adventures")]
        Task<PagedResultViewModel<GetAdventuresViewModel>> GetAdventuresAsync();

        [Get("adventures/{id}")]
        Task<GetAdventureByIdViewModel> GetAdventureByIdAsync(
            [Path] Guid id
            );

        [Get("users/{userId}/adventures")]
        Task<PagedResultViewModel<GetUserAdventuresViewModel>> GetUserAdventures(
            [Path] Guid userId
            );

        [Post("users/{userId}/adventures")]
        Task<CommandResultViewModel> CreateUserAdventure(
            [Path] Guid userId,
            [Body] CreateUserAdventureRequestModel payload
            );

        [Put("users/{userId}/adventures/{adventureId}")]
        Task<CommandResultViewModel> UpdateUserAdventure(
            [Path] Guid userId,
            [Path] Guid adventureId,
            [Body] UpdateUserAdventureRequestModel payload
            );
    }

    public class CommandResultViewModel
    {
        public Guid Id { get; set; }
    }

    public class PagedResultViewModel<T>
    {
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; } = new List<T>();
    }

    public class GetAdventuresViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string OptionTitle { get; set; } = string.Empty;
    }

    public class GetAdventureByIdViewModel
    {
        public Guid Id { get; set; }

        public GetAdventureByIdViewModel()
        {
            Children = new List<GetAdventureByIdViewModel>();
        }

        public IEnumerable<GetAdventureByIdViewModel> Children { get; set; }
        public string Name { get; set; } = string.Empty;
        public string OptionTitle { get; set; } = string.Empty;
    }

    public class CreateUserAdventureRequestModel
    {
        public Guid AdventureId { get; set; }
        public Guid UserId { get; set; }
    }

    public class UpdateUserAdventureRequestModel
    {
        public Guid AdventureStepId { get; set; }
        public Guid UserAdventureId { get; set; }
        public Guid UserId { get; set; }
    }

    public class GetUserAdventuresViewModel
    {
        public Guid Id { get; set; }

        public GetUserAdventuresViewModel()
        {
            Steps = new List<GetAdventuresViewModel>();
        }

        public string Name { get; set; } = string.Empty;
        public string OptionTitle { get; set; } = string.Empty;
        public UserAdventureStatus Status { get; set; }
        public ICollection<GetAdventuresViewModel> Steps { get; set; }
    }

    public enum UserAdventureStatus
    {
        InProgress = 0,
        Completed = 1
    }
}
