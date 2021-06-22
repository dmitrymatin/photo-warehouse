using Microsoft.EntityFrameworkCore;
using PhotoWarehouse.Domain.Photos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoWarehouse.Data.Repositories
{
    public interface IPhotoCategoryRepository
    {
        Task<IEnumerable<PhotoCategory>> GetAllAsync();
    }

    public class SqlPhotoCategoryRepository : IPhotoCategoryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SqlPhotoCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<PhotoCategory>> GetAllAsync()
        {
            return await _applicationDbContext.PhotoCategories
                .AsNoTracking()
                .OrderBy(pc => pc.Name) // TODO: consider adding index
                .ToListAsync();
        }
    }
}
