using PhotoWarehouse.Domain.Photos;
using System.Linq;

namespace PhotoWarehouse.Data.Repositories
{
    public interface IFileFormatRepository
    {
        void Add(FileFormat fileFormat);
        FileFormat GetByName(string name);
        int Commit();
    }

    public class SqlFileFormatRepository : IFileFormatRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlFileFormatRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public void Add(FileFormat fileFormat)
        {
            _context.FileFormats.Add(fileFormat);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public FileFormat GetByName(string name)
        {
            return _context.FileFormats.FirstOrDefault(f => f.Name == name);
        }
    }
}
