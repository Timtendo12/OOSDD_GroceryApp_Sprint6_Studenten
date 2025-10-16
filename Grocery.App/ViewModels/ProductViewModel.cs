using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;

namespace Grocery.App.ViewModels
{
    public partial class ProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        public ObservableCollection<Product> Products { get; set; }

        [ObservableProperty]
        Client client;

        public ProductViewModel(IProductService productService, GlobalViewModel global)
        {
            _productService = productService;
            Products = [];
            client = global.Client;
            foreach (Product p in _productService.GetAll()) Products.Add(p);
        }

        [RelayCommand]
        public async Task ShowNewProductViewCommand()
        {
            // navigate to new product view, if the user has admin role
            if (client.Role == Role.Admin) await Shell.Current.GoToAsync(nameof(NewProductView), true);
        }
    }
}
