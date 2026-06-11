using LotteryApi.Configuration;
using LotteryApi.Dtos;
using LotteryApi.Models;
using LotteryApi.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LotteryApi.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IDistributedCache _cache;
        private readonly CacheSettings _cacheSettings;
        private readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        private string PackageListKey => $"{_cacheSettings.Prefix}Packages:List";
        private string PackageKey(string id) => $"{_cacheSettings.Prefix}Packages:{id}";

        public PackageService(
            IPackageRepository packageRepository,
            IDistributedCache cache,
            IOptions<CacheSettings> cacheOptions)
        {
            _packageRepository = packageRepository;
            _cache = cache;
            _cacheSettings = cacheOptions.Value;
        }

        public async Task<IEnumerable<PackageDto>> GetPackageAsync()
        {
            var cachedData = await _cache.GetStringAsync(PackageListKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<IEnumerable<PackageDto>>(cachedData, _serializerOptions)!;
            }

            var packages = await _packageRepository.GetPackageAsync();
            var dtos = packages.Select(d => new PackageDto
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price,
                QtyClassicCards = d.QtyClassicCards,
                QtySpecialCards = d.QtySpecialCards,
                QtyPrimumCards = d.QtyPrimumCards
            }).ToList();

            var cacheOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(_cacheSettings.SlidingExpirationMinutes));

            await _cache.SetStringAsync(PackageListKey,
                JsonSerializer.Serialize(dtos, _serializerOptions),
                cacheOptions);

            return dtos;
        }

        public async Task<PackageDto?> GetPackageByIdAsync(string id)
        {
            var cacheKey = PackageKey(id);
            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<PackageDto>(cachedData, _serializerOptions);
            }

            var package = await _packageRepository.GetPackageByIdAsync(id);
            if (package == null) return null;

            var dto = new PackageDto
            {
                Id = package.Id,
                Name = package.Name,
                Price = package.Price,
                QtyClassicCards = package.QtyClassicCards,
                QtySpecialCards = package.QtySpecialCards,
                QtyPrimumCards = package.QtyPrimumCards
            };

            var cacheOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(_cacheSettings.SlidingExpirationMinutes));

            await _cache.SetStringAsync(cacheKey,
                JsonSerializer.Serialize(dto, _serializerOptions),
                cacheOptions);

            return dto;
        }

        public async Task<PackageDto> CreatePackageAsync(PackageCreateDto package)
        {
            var newPackage = new PackageModel
            {
                Name = package.Name,
                Price = package.Price,
                QtyClassicCards = package.QtyClassicCards,
                QtySpecialCards = package.QtySpecialCards,
                QtyPrimumCards = package.QtyPrimumCards
            };

            var created = await _packageRepository.CreatePackageAsync(newPackage);
            await InvalidatePackageCacheAsync(created.Id);

            return new PackageDto
            {
                Id = created.Id,
                Name = created.Name,
                Price = created.Price,
                QtyClassicCards = created.QtyClassicCards,
                QtySpecialCards = created.QtySpecialCards,
                QtyPrimumCards = created.QtyPrimumCards
            };
        }

        public async Task<PackageDto?> UpdatePackageAsync(string id, PackageUpdateDto updatePackage)
        {
            var existing = await _packageRepository.GetPackageByIdAsync(id);
            if (existing == null) return null;

            if (updatePackage.Name != null) existing.Name = updatePackage.Name;
            existing.Price = updatePackage.Price ?? existing.Price;
            existing.QtyClassicCards = updatePackage.QtyClassicCards ?? existing.QtyClassicCards;
            existing.QtySpecialCards = updatePackage.QtySpecialCards ?? existing.QtySpecialCards;
            existing.QtyPrimumCards = updatePackage.QtyPrimumCards ?? existing.QtyPrimumCards;

            var updated = await _packageRepository.UpdatePackageAsync(existing);
            if (updated == null) return null;

            await InvalidatePackageCacheAsync(id);

            return new PackageDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Price = updated.Price,
                QtyClassicCards = updated.QtyClassicCards,
                QtySpecialCards = updated.QtySpecialCards,
                QtyPrimumCards = updated.QtyPrimumCards
            };
        }

        public async Task<bool> DeletePackageAsync(string id)
        {
            var result = await _packageRepository.DeletePackageAsync(id);
            if (result) await InvalidatePackageCacheAsync(id);
            return result;
        }

        private Task InvalidatePackageCacheAsync(string id)
        {
            return Task.WhenAll(
                _cache.RemoveAsync(PackageListKey),
                _cache.RemoveAsync(PackageKey(id)));
        }
    }
}