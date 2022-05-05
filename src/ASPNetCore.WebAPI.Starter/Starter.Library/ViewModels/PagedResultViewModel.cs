using AutoMapper;
using System.Collections.Generic;

namespace Starter.Library.ViewModels
{
    public class PagedResultViewModel<T>
    {
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }
    }

    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap(typeof(PagedResultViewModel<>), typeof(PagedResultViewModel<>));
        }
    }
}
