namespace Output.Benchmarking.Mappers
{
    public interface ITypeMapper<TSource, TTarget> where TTarget : class
    {
        TTarget Map(TSource source);
    }
}