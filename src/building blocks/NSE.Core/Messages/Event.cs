

using MediatR;
using System;

namespace NSE.Core.Messages
{
    //O (INotification) é uma inferface de marcação.
    public class Event : Message, INotification
    {
        //A classe Event vai ter uma informação dizendo (momento) quando foi execultado
        public  DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
