using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;

using Microsoft.EntityFrameworkCore;
using crivasT5.DataAccess;
using crivasT5.DTOs;
using crivasT5.Utilidades;
using crivasT5.Modelos;
using System.Collections.ObjectModel;
using crivasT5.Views;

namespace crivasT5.ViewModels
{
    public partial class MainVewModels : ObservableObject
    {
        private readonly PersonaDbContext _dbContext;
        [ObservableProperty]
        private ObservableCollection<PersonaDTO> listaPersona =new ObservableCollection<PersonaDTO>();

        public MainVewModels(PersonaDbContext context)
        {
            _dbContext = context;

            MainThread.BeginInvokeOnMainThread(new Action(async () => await Obtener()));

            WeakReferenceMessenger.Default.Register<PersonaMensajeria>(this, (r, m) =>
            {
                PersonaMensajeRecibido(m.Value);
            });

        }

        public async Task Obtener()
        { 
            var lista=await _dbContext.Personas.ToListAsync();
            if (lista.Any())
            {
                foreach(var item in lista)
                { 
                    ListaPersona.Add(new PersonaDTO
                    { 
                            IdPersona= item.IdPersona,
                            Nombre=item.Nombre,
                    });

                }
                            
            }
        }

        private void PersonaMensajeRecibido(PersonaMensaje personaMensaje)
        { 
            var personaDto= personaMensaje.PersonaDto;

            if (personaMensaje.EsCrear)
            {
                ListaPersona.Add(personaDto);
            }
            else
            {
                var encontrado = ListaPersona.First(e => e.IdPersona == personaDto.IdPersona);
                encontrado.Nombre = personaDto.Nombre;

            }
        
        }

        [RelayCommand]
        private async Task Crear()
        {
            var uri = $"{nameof(PersonaPage)}?id=0";
            await Shell.Current.GoToAsync(uri);
        
        }

        [RelayCommand]
        private async Task Editar(PersonaDTO personaDto)
        {
            var uri = $"{nameof(PersonaPage)}?id{personaDto.IdPersona}";
            await Shell.Current.GoToAsync(uri);

        }


        [RelayCommand]
        private async Task Eliminar(PersonaDTO personaDto)
        {
            bool answer = 
                await Shell.Current.DisplayAlert("Mensaje", "Desea eliminar a la Persona Seleccionada", "Si", "No");
            if (answer) 
            {
                var encontrado = await _dbContext.Personas.FirstAsync(e=>e.IdPersona== personaDto.IdPersona);

                _dbContext.Personas.Remove(encontrado);
                await _dbContext.SaveChangesAsync();
                ListaPersona.Remove(personaDto);
            
            }

        }


    }
}
