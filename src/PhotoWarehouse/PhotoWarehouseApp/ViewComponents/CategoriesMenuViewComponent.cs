using Microsoft.AspNetCore.Mvc;
using PhotoWarehouse.Data.Repositories;
using System.Threading.Tasks;

namespace PhotoWarehouseApp.ViewComponents
{
    public class CategoriesMenuViewComponent : ViewComponent
    {
        private readonly IPhotoCategoryRepository _photoCategoryRepository;

        public CategoriesMenuViewComponent(IPhotoCategoryRepository photoCategoryRepository)
        {
            _photoCategoryRepository = photoCategoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _photoCategoryRepository.GetAllAsync();
            return View(categories);
        }
    }
}
