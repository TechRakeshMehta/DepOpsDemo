
namespace Business.SearchAbstractFactory
{
    public class NonMessage : ISearchFactory<NonMessage>
    {
        public TProduct Build<TProduct>() where TProduct : ISearchEntities<NonMessage>, new()
        {
            return new TProduct();
        }
    }
}
