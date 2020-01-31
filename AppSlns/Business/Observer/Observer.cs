using INTSOF.Services.Observer;
using System;

namespace Business.Observer
{
    /// <summary>
    ///  Generic observer class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Observer<T> : INTSOF.Services.Observer.IObserver<T> where T : class
    {
        public T Element { get; set; }
        private ISubject Subject { get; set; }
        
        /// <summary>
        /// Action to be invoked when  notified
        /// </summary>
        private Action<ISubject, T> Action { get; set; }
        
        /// <summary>
        /// Creates observer object for given subject, elemnent and action
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="element"></param>
        /// <param name="action"></param>
        public Observer(ISubject subject, T element,
               Action<ISubject, T> action = null)
        {
            Subject = subject;
            Element = element;
            Action = action ?? subject.UpdateAction;
        }
        /// <summary>
        /// update method to be called when notified
        /// </summary>
        public void Update()
        {
            if (Action != null)
                Action.Invoke(Subject, Element);
        }
    }
}
