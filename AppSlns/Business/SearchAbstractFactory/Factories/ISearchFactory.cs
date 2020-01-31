
namespace Business.SearchAbstractFactory
{
   public interface ISearchFactory<TFactory>
    {
        TProduct Build<TProduct>() where TProduct : ISearchEntities<TFactory>, new();
    }
}
