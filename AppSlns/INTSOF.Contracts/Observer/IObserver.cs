
namespace INTSOF.Services.Observer
{
    public interface IObserver<T> where T : class
    {
        /// <summary>
        /// update observer according to update action that set to the observer
        /// </summary>
        void Update();

        /// <summary>
        /// element that observer should update it
        /// </summary>
        T Element { get; set; }

    }
}
