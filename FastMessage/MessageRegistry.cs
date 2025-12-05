using System;
using System.Collections.Generic;

namespace Runtime.Message
{
    public struct MessageRegistry<T> : IDisposable where T : struct, IMessage
    {
        #region Members

        private Action _unsubscribeAction;
        private bool _isDisposed;

        #endregion Members

        #region Struct Methods

        public MessageRegistry(Action unsubscribeAction)
        {
            _unsubscribeAction = unsubscribeAction;
            _isDisposed = false;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _unsubscribeAction?.Invoke();
                _unsubscribeAction = null;
            }
        }

        #endregion Struct Methods
    }
}