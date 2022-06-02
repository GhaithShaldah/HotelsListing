using HotelsListing.Data.Entities;
using System;
using System.Threading.Tasks;

namespace HotelsListing.Repository_Pattern.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Country> Countries { get; }
        IGenericRepository<Hotel> Hotels { get; }

        Task Save();

    }
}
