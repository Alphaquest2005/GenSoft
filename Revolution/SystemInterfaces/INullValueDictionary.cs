namespace SystemInterfaces
{
    public interface INullValueDictionary<TKey, TValue>
        where TValue : class
    {
        TValue this[TKey key] { get; }
    }
}