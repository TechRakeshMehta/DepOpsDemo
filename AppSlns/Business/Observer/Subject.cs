using INTSOF.Services.Observer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Observer
{

    /// <summary>
    /// The subject class which represents a subject of observation in Observer pattern
    /// </summary>
    public class Subject : ISubject
    {
        private IList<INTSOF.Services.Observer.IObserver<dynamic>> _observers;

        /// <summary>
        /// List of registered observers
        /// </summary>
        public IList<INTSOF.Services.Observer.IObserver<dynamic>> Observers
        {
            get
            {
                return _observers ?? (_observers =
                       new List<INTSOF.Services.Observer.IObserver<object>>());
            }
            set
            {
                _observers = value;
            }
        }

        /// <summary>
        /// Holds update action for the subject
        /// </summary>
        public Action<ISubject, object> UpdateAction { get; set; }
        
        public object SubjectState { get; set; }
        /// <summary>
        /// Instantiates subject with given action
        /// </summary>
        /// <param name="action"></param>
        public Subject(Action<ISubject, dynamic> action = null)
        {
            UpdateAction = action;
        }
        /// <summary>
        /// Registers an observer
        /// </summary>
        /// <param name="observer"></param>
        public void Attach(INTSOF.Services.Observer.IObserver<dynamic> observer)
        {
            Observers.Add(observer);
        }
        /// <summary>
        /// Detatches an observer
        /// </summary>
        /// <param name="observer"></param>
        public void Detach(INTSOF.Services.Observer.IObserver<dynamic> observer)
        {
            Observers.Remove(observer);
        }
        /// <summary>
        /// Registers a new observer with given element and update action
        /// </summary>
        /// <typeparam name="T">Type of element which has to be updated</typeparam>
        /// <param name="element">The element object to be updated</param>
        /// <param name="updateAction">The update action</param>
        public void AttachElement<T>(T element, Action<ISubject,
               dynamic> updateAction = null) where T : class
        {
            if (element == null)
                return;

            Observers.Add(new Observer<dynamic>(this, element,
                          updateAction ?? UpdateAction));
        }
        /// <summary>
        /// Remvoes a registered observer element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        public void DetachElement<T>(T element) where T : class
        {
            var isExist = Observers.Any(p => p.Element == element);

            if (isExist)
                Observers.Remove(Observers.First(p => p.Element == element));
        }
        /// <summary>
        /// Notifies all the registered observers
        /// </summary>
        public void Notify()
        {
            foreach (var vo in Observers)
            {
                vo.Update();
            }
        }
    }
}
