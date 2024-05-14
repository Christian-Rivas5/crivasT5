using CommunityToolkit.Mvvm.Messaging.Messages;

namespace crivasT5.Utilidades
{
    public class PersonaMensajeria : ValueChangedMessage<PersonaMensaje>
    {
        public PersonaMensajeria(PersonaMensaje value) : base(value)
        {
        }
    }
}
