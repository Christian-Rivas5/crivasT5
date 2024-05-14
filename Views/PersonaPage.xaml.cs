using crivasT5.ViewModels;

namespace crivasT5.Views;

public partial class PersonaPage : ContentPage
{
	public PersonaPage(PersonaViewModel viewModel )
	{
		InitializeComponent();
		BindingContext=viewModel;
	}
}