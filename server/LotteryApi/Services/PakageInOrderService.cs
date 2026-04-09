using LotteryApi.Dtos;
using LotteryApi.Models;
using LotteryApi.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace LotteryApi.Services
{
    public class PakageInOrderService : IPakageInOrderService
    {
        private readonly IPackageInOrderRepository _packageInOrderRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IShoppingCartRepository _ShoppingCartRepository;
        public PakageInOrderService(IPackageInOrderRepository packageInOrderRepository,
            IPackageRepository packageRepository,
            IShoppingCartRepository shoppingCartRepository)
        {
            _packageInOrderRepository = packageInOrderRepository;
            _packageRepository = packageRepository;
            _ShoppingCartRepository = shoppingCartRepository;
        }

        public async Task<PackageInOrderDto?> GetPackageInOrderByIdAsync(int id)
        {
            var packageInOrder = await _packageInOrderRepository.GetPackageInOrderByIdAsync(id);

            return packageInOrder != null ? new PackageInOrderDto
            {
                PackageId = packageInOrder.PackageId,
                PackageName = packageInOrder.Package?.Name,
                PriceAtPurchase = packageInOrder.PriceAtPurchase,

                GiftsInPackage = packageInOrder.GiftsInPackage?.Select(g => new GiftInOrderDto
                {
                    Id = g.Id,
                    GiftId = g.GiftId,
                    GiftName = g.Gift?.Name,
                    GiftPictureUrl = g.Gift?.PictureUrl,
                    GiftCardPrice = g.Gift?.CardPrice.ToString(),
                    IsWinner = g.IsWinner
                }).ToList() ?? []
            } : null;
        }

        //public async Task<List<PackageInOrderDto>> CreatePackagesAndGiftsInCartAsync(ShoppingCartDto shoppingCartDto, int orderId)
        //{

        //    var cart = await _ShoppingCartRepository.GetShoppingCartByIdAsync(shoppingCartDto.Id);
        //    if (cart == null)
        //    {
        //        return null;
        //    }
        //    var packageToOrder = cart.PackagesInShoppingCart?.Select(p => new PackageInOrderModel
        //    {
        //        PackageId = p.PackageId,
        //        Package = p.Package,
        //        PriceAtPurchase = p.Package?.Price ?? 0,
        //        OrderId = orderId,
        //        GiftsInPackage = p.GiftsInPackage?.Select(g => new GiftInOrderModel
        //        {
        //            GiftId = g.GiftId,
        //            Gift = g.Gift,
        //            OrderId = orderId,
        //            IsWinner = false
        //        }).ToList()
        //    }).ToList();
        //    if (packageToOrder == null || !packageToOrder.Any())
        //    {
        //        return null;
        //    }
        //    var isSaved = await _packageInOrderRepository.CreatePackagesAndGiftsInCartAsync(packageToOrder);

        //    if (!isSaved) return null;

        //    var resultDto = packageToOrder.Select(p => new PackageInOrderDto
        //    {
        //        PackageId = p.PackageId,
        //        PackageName = p.Package?.Name,
        //        PriceAtPurchase = p.PriceAtPurchase,

        //        GiftsInPackage = p.GiftsInPackage?.Select(g => new GiftInOrderDto
        //        {
        //            Id = g.Id,
        //            GiftId = g.GiftId,
        //            GiftName = g.Gift?.Name,
        //            GiftPictureUrl = g.Gift?.PictureUrl,
        //           GiftCardPrice = g.Gift?.CardPrice.ToString(),
        //            IsWinner = g.IsWinner
        //        }).ToList() ?? []
        //    }).ToList();


        //    return resultDto;
        //}
    }
}




