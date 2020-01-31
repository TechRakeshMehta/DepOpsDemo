using System;
using System.Collections.Generic;

namespace INTSOF.Services.Observer
{
    public interface ISubject
    {
        /// <summary>
        /// list of obsrvers
        /// </summary>
        IList<IObserver<object>> Observers { get; set; }

        /// <summary>
        /// common update action for observers which it's update action has set to null
        /// </summary>
        Action<ISubject, object> UpdateAction { get; set; }

        /// <summary>
        /// the state of subject
        /// </summary>
        object SubjectState { get; set; }

        /// <summary>
        /// attach a observer to this subject
        /// </summary>
        /// <param name="observer"></param>
        void Attach(IObserver<object> observer);

        /// <summary>
        /// detach a observer from this subject
        /// </summary>
        /// <param name="observer"></param>
        void Detach(IObserver<object> observer);

        /// <summary>
        /// create new observer with given element and common
        /// update action and attach it to this subject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <param name="updateAction"></param>
        void AttachElement<T>(T element, Action<ISubject,
             object> updateAction = null) where T : class;

        /// <summary>
        /// detach observer that contains given element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void DetachElement<T>(T element) where T : class;

        /// <summary>
        /// notify observers that they should update
        /// </summary>
        void Notify();
    }
}

 