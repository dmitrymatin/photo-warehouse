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


        /// <summary>
        /// Search method. By default this method will query for all photos that have a set name and have at least one associated photo item.
        /// </summary>
        /// <param name="searchTerm">Search term to be matched.</param>
        /// <param name="maxCount">Specifies the number of elements to be searched.</param>
        /// <param name="requireSetPhotoName">Enables searching only photos that have non empty name.</param>
        /// <param name="requirePhotoItems">Enables searching only photos that have at least one associated photo item.</param>
        Task<IList<Photo>> GetPhotosAsync(string searchTerm, int maxCount = 300, bool requireSetPhotoName = true, bool requirePhotoItems = true);
        Task<IList<Photo>> GetPhotosInCategoryAsync(string categoryName, int maxCount = 300, bool requireSetPhotoName = true, bool requirePhotoItems = true);
        Task<IList<PhotoItem>> GetPhotoItemsAsync(int photoId);
        IList<PhotoItem> GetPhotoItems(int photoId);
        Task<IList<PhotoItem>> GetUserPhotoItemsAsync(string userId);
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

        public IList<PhotoItem> GetPhotoItems(int photoId)
        {
            return _context.PhotoItems
                .AsNoTracking()
                .Include(pi => pi.Size)
                .Include(pi => pi.FileFormat)
                .Where(p => p.PhotoId == photoId).ToList();
        }

        public async Task<IList<PhotoItem>> GetUserPhotoItemsAsync(string userId)
        {
            return await _context.PhotoItems
                .AsNoTracking()
                //.Include(pi => pi.ApplicationUsers.Where(u => u.Id == userId))
                .Where(pi => pi.ApplicationUsers.Any(u => u.Id == userId))
                .ToListAsync();
        }


        public async Task<IList<Photo>> GetPhotosAsync(string searchTerm, int maxCount = 300, bool requireSetPhotoName = true, bool requirePhotoItems = true)
        {
            IQueryable<Photo> queryBuilder = _context.Photos
                .Include(p => p.Category)
                .Include(p => p.PhotoItems)
                    .ThenInclude(pi => pi.FileFormat)
                .Include(p => p.PhotoItems)
                    .ThenInclude(pi => pi.Size)
                .AsNoTracking();

            IQueryable<Photo> filteredQuery = queryBuilder;

            if (requireSetPhotoName)
                filteredQuery = filteredQuery.Where(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(searchTerm));
            else
                filteredQuery = filteredQuery.Where(p => string.IsNullOrEmpty(p.Name) || p.Name.Contains(searchTerm));

            if (requirePhotoItems)
                filteredQuery = filteredQuery.Where(p => p.PhotoItems.Any());

            return await filteredQuery
                .OrderByDescending(p => p.InitialUploadDate) // TODO: consider indexing by InitialUploadDate field
                .Take(maxCount)
                .ToListAsync();
        }

        public async Task<IList<Photo>> GetPhotosInCategoryAsync(string categoryName, int maxCount = 300, bool requireSetPhotoName = true, bool requirePhotoItems = true)
        {
            IQueryable<Photo> queryBuilder = _context.Photos
                .Include(p => p.Category)
                .Include(p => p.PhotoItems)
                    .ThenInclude(pi => pi.FileFormat)
                .Include(p => p.PhotoItems)
                    .ThenInclude(pi => pi.Size)
                .Where(p => p.Category.Name == categoryName)
                .AsNoTracking();

            IQueryable<Photo> filteredQuery = queryBuilder;

            if (requireSetPhotoName)
                filteredQuery = filteredQuery.Where(p => !string.IsNullOrEmpty(p.Name));
            else
                filteredQuery = filteredQuery.Where(p => string.IsNullOrEmpty(p.Name));

            if (requirePhotoItems)
                filteredQuery = filteredQuery.Where(p => p.PhotoItems.Any());

            return await filteredQuery
                .OrderByDescending(p => p.InitialUploadDate) // TODO: consider indexing by InitialUploadDate field
                .Take(maxCount)
                .ToListAsync();
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
