using Microsoft.EntityFrameworkCore;
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
        void AddPhoto(Photo photo);
        Task AddPhotoItemAsync(Stream stream, string path, PhotoItem photoItem);
        void UpdatePhoto(Photo photo);
    }

    public class SqlPhotoRepository : IPhotoRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlPhotoRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public void AddPhoto(Photo photo)
        {
            _context.Photos.Add(photo);
        }

        public async Task AddPhotoItemAsync(Stream stream, string path, PhotoItem photoItem)
        {
            using (FileStream output = File.Create(path))
            {
                await stream.CopyToAsync(output);
            }

            _context.PhotoItems.Add(photoItem);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void UpdatePhoto(Photo photo)
        {
            _context.Entry(photo).State = EntityState.Modified;
        }
    }
}
