using crivasT5.ViewModels;

namespace crivasT5
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(MainVewModels viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        
    }

}
