namespace Tha.ChooseYourAdventure.Library.Core
{
    public interface IPagedGetRequest
    {
        public bool Count { get; set; }
        public int Limit { get; set; }
        public int Skip { get; set; }
    }
}
