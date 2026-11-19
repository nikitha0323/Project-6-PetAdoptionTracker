using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetAdoptionTracker.Data
{
    public class DataProvider
    {
        private readonly AdoptionDbContext _context;

        public DataProvider(AdoptionDbContext context)
        {
            _context = context;
        }

        // Read all records from a table.
        public async Task<List<T>> GetAllAsync<T>() where T : class
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        // Read one record by its ID.
        public async Task<T?> GetByIdAsync<T>(int id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        // Insert a new record.
        public async Task<T> InsertAsync<T>(T record) where T : class
        {
            _context.Set<T>().Add(record);
            await _context.SaveChangesAsync();

            return record;
        }

        // Update an existing record.
        // The record's ID must match the supplied ID.
        public async Task<bool> UpdateAsync<T>(int id, T record)
            where T : class
        {
            var existing = await _context.Set<T>().FindAsync(id);

            if (existing == null)
            {
                return false;
            }

            _context.Entry(existing).CurrentValues.SetValues(record);
            await _context.SaveChangesAsync();

            return true;
        }

        // Delete an existing record.
        public async Task<bool> DeleteAsync<T>(int id) where T : class
        {
            var existing = await _context.Set<T>().FindAsync(id);

            if (existing == null)
            {
                return false;
            }

            _context.Set<T>().Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}