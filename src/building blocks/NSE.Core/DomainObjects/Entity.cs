using NSE.Core.Messages;
using System;
using System.Collections.Generic;

namespace NSE.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }
        //esta é uma lista privada.
        private List<Event> _notificacoes;
        //eu só posso ler desta coleção eu não posso adicionar.
        //esta é uma lista de somente leitura.
        public IReadOnlyCollection<Event> Notificacoes => _notificacoes?.AsReadOnly();

        //vai adicionar um evento na lista caso não tenha (??)
        public void AdicionarEvento(Event evento)
        {
            //estou dizendo se a notificação ainda não existe (??) eu vou criar uma nova instancia (new List<Event>)
            //e adicionar.
            _notificacoes = _notificacoes ?? new List<Event>();
            _notificacoes.Add(evento);
        }

        //ele vai remover um evento especifico atraves do (eventItem)
        public void RemoverEvento(Event eventItem)
        {
            _notificacoes?.Remove(eventItem);
        }

        //Vai limpar a lista. Para não ficar dando o evento a todo momento
        public void LimparEventos()
        {
            _notificacoes?.Clear();
        }



        #region Comparacao
        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        #endregion
    }
}