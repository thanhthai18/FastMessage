using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Runtime.Message
{
    public class MessageHandlersHolder<T> where T : struct, IMessage
    {
        #region Members

        private readonly List<MessageHandler<T>> _handlers = new();
        private MessageHandler<T>[] _cache = Array.Empty<MessageHandler<T>>();
        private bool _isDirty = false;
        private readonly object _lockObj = new(); // for thread safety.
        
        #endregion Members

        #region Properties

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _handlers.Count == 0;
        }

        #endregion Properties

        #region Class Methods

        public MessageRegistry<T> Subscribe(MessageHandler<T> messageHandler)
        {
            lock (_lockObj)
            {
                _handlers.Add(messageHandler);
                _isDirty = true;
                return new MessageRegistry<T>(() => Unsubscribe(messageHandler));
            }
        }
        
        private void Unsubscribe(MessageHandler<T> messageHandler)
        {
            lock (_lockObj)
            {
                if (_handlers.Remove(messageHandler))
                {
                    _isDirty = true;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Publish(T message)
        {
            if (_isDirty)
            {
                lock (_lockObj)
                {
                    if (_isDirty)
                    {
                        _cache = _handlers.ToArray();
                        _isDirty = false;
                    }
                }
            }

            var handlers = _cache;
            var count = handlers.Length;

            for (var i = 0; i < count; i++)
            {
                handlers[i].Invoke(message);
            }
        }

        #endregion Class Methods
    }
}