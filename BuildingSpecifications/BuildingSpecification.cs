using Common.Dynamic;

namespace BuildingSpecifications
{
    public class BuildingSpecification<T>: Expando where T : new()
    {
        public T Build()
        {
            var res = new T();
            return res;
        }

        
    }
}
