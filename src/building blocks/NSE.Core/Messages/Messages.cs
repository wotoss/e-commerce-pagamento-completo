using System;


namespace NSE.Core.Messages
{
    //é uma classe abstrata porque só pode ser herdada e não implementada.
    public abstract class Message
    {
        public string MessageType { get; protected set; }

        public Guid AggregateId { get; protected set; }

        protected Message()
        {
            //eu pego a minha propriedade MessageType e passo no contrutor e busco o tipo dela como o GetType().Name
            MessageType = GetType().Name;
        }
    }
}
