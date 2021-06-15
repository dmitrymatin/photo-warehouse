using PhotoWarehouse.Domain.Photos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoWarehouse.Data.Repositories
{
    public interface IPhotoRepository
    {
        int Commit();
        void Add(Photo photo);
        Task AddPhotoItemAsync(Stream stream, PhotoItem photoItem, string path);
    }

    public class SQLPhotoRepository : IPhotoRepository
    {
        private readonly ApplicationDbContext _context;

        public void Add(Photo photo)
        {
            _context.Add(photo);
        }

        public async Task AddPhotoItemAsync(Stream stream, PhotoItem photoItem, string path)
        {
            using (FileStream output = File.Create(path))
            {
                await stream.CopyToAsync(output);
            }
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }
    }
}
