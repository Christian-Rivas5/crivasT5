using CommunityToolkit.Mvvm.ComponentModel;

namespace crivasT5.DTOs
{
    public partial class PersonaDTO : ObservableObject
    {
        [ObservableProperty]
        public int idPersona;
        [ObservableProperty]
        public string nombre;
    }
}
