using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;

using Microsoft.EntityFrameworkCore;
using crivasT5.DataAccess;
using crivasT5.DTOs;
using crivasT5.Utilidades;
using crivasT5.Modelos;


namespace crivasT5.ViewModels
{
    public partial class PersonaViewModel : ObservableObject, IQueryAttributable
    {

        private readonly PersonaDbContext _dbContext;

        [ObservableProperty]
        private PersonaDTO personaDto =new PersonaDTO();

        [ObservableProperty]
        private string titutloPagina;

        private int IdPersona;

        [ObservableProperty]
        private bool loadingEsVisible=false;

       // public PersonaViewModel(PersonaDbContext context )
        //{
           //  _dbContext = context;
         //   personaDto
       // }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = int.Parse(query["id"].ToString());
            IdPersona = id;

            if (IdPersona ==0) 
            {
                TitutloPagina = "Editar Persona";
              
            }
            else
            {
                TitutloPagina = "Editar Persona";
                LoadingEsVisible = true;
                await Task.Run(async () =>
                {
                var encontrado = await _dbContext.Personas.FirstAsync(e => e.IdPersona == IdPersona);
                PersonaDto.IdPersona = encontrado.IdPersona;
                PersonaDto.Nombre = encontrado.Nombre;

                MainThread.BeginInvokeOnMainThread(() => { LoadingEsVisible = false; });
                 
                });
            }
        }

        [RelayCommand]
        private async Task Guardar()

        { 
        
            LoadingEsVisible= true;
            PersonaMensaje mensaje = new PersonaMensaje();
            await Task.Run(async () =>
            {
                if (IdPersona == 0)
                {
                    var tbPersona = new Persona()
                    {
                        Nombre = PersonaDto.Nombre,

                    };
                    _dbContext.Personas.Add(tbPersona);
                    await _dbContext.SaveChangesAsync();

                    PersonaDto.IdPersona = tbPersona.IdPersona;
                    mensaje = new PersonaMensaje()
                    { 
                        EsCrear= true,
                        PersonaDto= PersonaDto

                    };

                }
                else
                {
                        var encontrado= await _dbContext.Personas.FirstAsync(e =>e.IdPersona==IdPersona);
                    encontrado.Nombre=PersonaDto.Nombre;

                    await _dbContext.SaveChangesAsync();

                    mensaje = new PersonaMensaje()
                    {
                        EsCrear = false,
                        PersonaDto = PersonaDto

                    };
                }

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    LoadingEsVisible = false;
                    WeakReferenceMessenger.Default.Send(new PersonaMensajeria(mensaje));
                    await Shell.Current.Navigation.PopAsync();
                });




            });
        }

    }
}
