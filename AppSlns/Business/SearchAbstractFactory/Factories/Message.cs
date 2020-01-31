
namespace Business.SearchAbstractFactory
{
    public class Message : ISearchFactory<Message>
    {
        public TProduct Build<TProduct>() where TProduct : ISearchEntities<Message>, new()
        {
            return new TProduct();
        }
    }
}
