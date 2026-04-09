using LotteryApi.Dtos;
using LotteryApi.Models;
using LotteryApi.Repositories;
using System.Reflection;

namespace LotteryApi.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        public PackageService(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }
        public async Task<IEnumerable<PackageDto>> GetPackageAsync()
        {
            var packages = await _packageRepository.GetPackageAsync();
            return packages.Select(d => new PackageDto
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price,
                QtyClassicCards = d.QtyClassicCards,
                QtySpecialCards = d.QtySpecialCards,
                QtyPrimumCards = d.QtyPrimumCards
            });

        }
        public async Task<PackageDto?> GetPackageByIdAsync(int id)
        {
            var package = await _packageRepository.GetPackageByIdAsync(id);
            return package != null ? new PackageDto { Id = package.Id, Name = package.Name, Price = package.Price, QtyClassicCards = package.QtyClassicCards, QtySpecialCards = package.QtySpecialCards, QtyPrimumCards = package.QtyPrimumCards } : null;
        }

        public async Task<PackageDto> CreatePackageAsync(PackageCreateDto package)
        {
            var newPackage = new PackageModel()
            {
                Name = package.Name,
                Price = package.Price,
                QtyClassicCards = package.QtyClassicCards,
                QtySpecialCards = package.QtySpecialCards,
                QtyPrimumCards = package.QtyPrimumCards


            };

            var createPackage = await _packageRepository.CreatePackageAsync(newPackage);
            return new PackageDto { Id = createPackage.Id, Name = createPackage.Name, Price = createPackage.Price, QtyClassicCards = createPackage.QtyClassicCards, QtySpecialCards = createPackage.QtySpecialCards, QtyPrimumCards = createPackage.QtyPrimumCards };
        }

        public async Task<PackageDto?> UpdatePackageAsync(int id, PackageUpdateDto updatePackage)
        {
            var existing = await _packageRepository.GetPackageByIdAsync(id);
            if (existing == null)
            {
                return null;
            }
            if (updatePackage.Name != null) existing.Name = updatePackage.Name;
            existing.Price = updatePackage.Price ?? existing.Price;
            existing.QtyClassicCards = updatePackage.QtyClassicCards ?? existing.QtyClassicCards;
            existing.QtySpecialCards = updatePackage.QtySpecialCards ?? existing.QtySpecialCards;
            existing.QtyPrimumCards = updatePackage.QtyPrimumCards ?? existing.QtyPrimumCards;
            var newUpdatePackage = await _packageRepository.UpdatePackageAsync(existing);
            return newUpdatePackage != null ? new PackageDto { Id = newUpdatePackage.Id, Name = newUpdatePackage.Name, Price = newUpdatePackage.Price, QtyClassicCards = newUpdatePackage.QtyClassicCards, QtySpecialCards = newUpdatePackage.QtySpecialCards, QtyPrimumCards = newUpdatePackage.QtyPrimumCards } : null;

        }
        public async Task<bool> DeletePackageAsync(int id)
        {
            return await _packageRepository.DeletePackageAsync(id);
        }

    }
}
