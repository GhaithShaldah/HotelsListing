using HotelsListing.Data;
using HotelsListing.Data.Entities;
using HotelsListing.Repository_Pattern.IRepository;
using System;
using System.Threading.Tasks;

namespace HotelsListing.Repository_Pattern.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext _context;
        public IGenericRepository<Country> _countries;
        public IGenericRepository<Hotel> _hotels;

        public UnitOfWork(DataBaseContext contetxt)
        {
            _context = contetxt;
        }

        public IGenericRepository<Country> Countries => _countries ??= new GenericRepository<Country>(_context);

        public IGenericRepository<Hotel> Hotels => _hotels ??= new GenericRepository<Hotel>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
