using AutoMapper;
using Core.Domain.Entities;

namespace Core.Application.Mapping
{
    public static class CustomMapper
    {
        public static IMapper Instance { get; set; }

        public static T To<T>(this object input)
        {
            IMapper mapper = Instance;
            return mapper.Map<T>(input);
        }

        

        public static IEnumerable<T> To<T>(this IEnumerable<object> input)
        {
            IMapper mapper = Instance;

            return mapper.Map<IEnumerable<T>>(input);
        }
    }
}
