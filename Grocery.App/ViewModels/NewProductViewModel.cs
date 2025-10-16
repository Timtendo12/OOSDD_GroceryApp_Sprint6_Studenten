using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Core.Interfaces.Services;

namespace Grocery.App.ViewModels
{
    public partial class NewProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;

        public NewProductViewModel(IProductService productService)
        {
            _productService = productService;
        }
    }
}
