using System.Linq;

namespace Output
{
    public interface IMapper
    {
        TOutput Map<TOutput>(object input);

        TOutput Map<TOutput>(object input, TOutput output);

        TOutput Map<TInput, TOutput>(TInput input);

        TOutput Map<TInput, TOutput>(TInput input, TOutput output);

        IQueryable<TOutput> Project<TOutput>(IQueryable input);
    }
}