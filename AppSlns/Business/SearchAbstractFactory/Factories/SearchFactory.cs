
namespace Business.SearchAbstractFactory
{
  public class SearchFactory<TFactory> where TFactory : ISearchFactory<TFactory>, new()
    {
        public TProduct Create<TProduct>() where TProduct : ISearchEntities<TFactory>, new()
        {
            return new TFactory().Build<TProduct>();
        }
    }
}
