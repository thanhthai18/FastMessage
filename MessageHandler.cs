namespace Runtime.Message
{
    public delegate void MessageHandler<T>(T message) where T : struct, IMessage;
}