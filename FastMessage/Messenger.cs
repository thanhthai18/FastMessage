using System.Runtime.CompilerServices;
using Runtime.Common.Singleton;

// Copyright (c) thanhthai18 and TweakKit.
namespace Runtime.Message
{
    public static class Messenger
    {
        #region Class Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessageRegistry<TMessage> Subscribe<TMessage>(MessageHandler<TMessage> messageHandler) where TMessage : struct, IMessage
            => Singleton.Of<MessageHandlersHolder<TMessage>>().Subscribe(messageHandler);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Publish<TMessage>(TMessage message) where TMessage : struct, IMessage
            => Singleton.Of<MessageHandlersHolder<TMessage>>().Publish(message);

        #endregion Class Methods
    }
}