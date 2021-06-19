using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PhotoWarehouse.Domain.Photos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PhotoWarehouse.Data.Repositories
{
    public interface IPhotoRepository
    {
        void AddPhoto(Photo photo);
        Task AddPhotoItemAsync(Stream stream, string path, PhotoItem photoItem);
        int Commit();
        void DeletePhotoItems(params PhotoItem[] photoItem);
        Task<Photo> GetPhotoAsync(int photoId,
            bool includeCategory = false,
            bool includeFileFormat = false,
            bool includeSize = false);
        Task<IList<PhotoItem>> GetPhotoItemsAsync(int photoId);
        bool PhotoExists(int photoId);
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

        public void DeletePhotoItems(params PhotoItem[] photoItems)
        {
            foreach (var photoItem in photoItems)
            {
                var deletedPhotoItem = _context.PhotoItems.Attach(photoItem);
                deletedPhotoItem.State = EntityState.Deleted;
            }
        }

        public async Task<Photo> GetPhotoAsync(int photoId, bool includeCategory = false,
            bool includeFileFormat = false, bool includeSize = false)
        {
            IQueryable<Photo> queryBuilder = _context.Photos;
            if (includeCategory)
                queryBuilder = queryBuilder.Include(p => p.Category);
            if (includeFileFormat)
                queryBuilder = queryBuilder.Include(p => p.PhotoItems).ThenInclude(pi => pi.FileFormat);
            if (includeSize)
                queryBuilder = queryBuilder.Include(p => p.PhotoItems).ThenInclude(pi => pi.Size);

            return await queryBuilder.AsNoTracking().FirstOrDefaultAsync(p => p.Id == photoId);
        }

        public async Task<IList<PhotoItem>> GetPhotoItemsAsync(int photoId)
        {
            return await _context.PhotoItems
                .AsNoTracking()
                .Where(p => p.PhotoId == photoId).ToListAsync();
        }

        public bool PhotoExists(int photoId)
        {
            return _context.Photos.AsNoTracking().Any(p => p.Id == photoId);
        }

        public void UpdatePhoto(Photo photo)
        {
            var updatedPhoto = _context.Photos.Attach(photo);
            updatedPhoto.State = EntityState.Modified;
        }
    }
}
