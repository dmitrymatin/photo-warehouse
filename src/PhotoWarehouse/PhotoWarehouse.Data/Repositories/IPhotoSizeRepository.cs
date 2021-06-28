using PhotoWarehouse.Domain.Photos;
using System.Linq;

namespace PhotoWarehouse.Data.Repositories
{
    public interface IPhotoSizeRepository
    {
        void Add(PhotoSize photo);
        PhotoSize GetByDimensions(int width, int height);
        int Commit();
    }

    public class SqlPhotoSizeRepository : IPhotoSizeRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlPhotoSizeRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public void Add(PhotoSize photo)
        {
            _context.PhotoSizes.Add(photo);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public PhotoSize GetByDimensions(int width, int height)
        {
            return _context.PhotoSizes.FirstOrDefault(size => size.Width == width && size.Height == height);
        }
    }
}
