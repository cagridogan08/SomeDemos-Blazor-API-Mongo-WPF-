using System;
using System.Collections.Generic;

namespace InMemoryEventBus.Handlers
{
    public class Observable<T> : IObservable<T>
    {
        private readonly HashSet<IObserver<T>> _observers = new HashSet<IObserver<T>>();
        public IDisposable Subscribe(IObserver<T> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        public void UpdateData(T data)
        {
            foreach (var obs in _observers)
            {
                obs.OnNext(data);
            }
        }

        public void OnError(Exception exception)
        {
            foreach (var obs in _observers)
            {
                obs.OnError(exception);
            }
        }

        public void Complete()
        {
            foreach (var obs in _observers)
            {
                obs.OnCompleted();
            }
            _observers.Clear();
        }
        private sealed class Unsubscriber : IDisposable
        {
            private readonly HashSet<IObserver<T>> _observers;
            private readonly IObserver<T> _observer;

            public Unsubscriber(ICollection<IObserver<T>> observers, IObserver<T> observer)
            {
                _observers = new(observers);
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observers.Contains(_observer))
                {
                    _observers.Remove(_observer);
                }
            }
        }
    }
}
