namespace GenSoft.Interfaces
{
    public interface IPredicate
    {
        string Path { get; }
        string Operation { get; }
        string Value { get; }
    }
}
