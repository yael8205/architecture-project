using LotteryApi.Dtos;
using LotteryApi.Enums;
using LotteryApi.Models;
using LotteryApi.Repositories;

namespace LotteryApi.Services
{
    public class GiftInCartService : IGiftInCartService
    {
        private readonly IGiftInCartRepository _giftInCartRepository;
        private readonly IPackageInCartRepository _PackageInCartRepository;
        private readonly IGiftRepoditory _giftRepoditory ;
      public  GiftInCartService(IGiftInCartRepository giftInCartRepository, IPackageInCartRepository packageInCartRepository, IGiftRepoditory giftRepoditory)
        {
            _giftInCartRepository = giftInCartRepository;
            _PackageInCartRepository = packageInCartRepository;
            _giftRepoditory = giftRepoditory;
        }

        public async Task<GiftInCartDto?> GetGiftInCartByIdAsync(int id)
        {
            var giftInCart = await _giftInCartRepository.GetGiftInCartByIdAsync(id);
            return giftInCart != null ? new GiftInCartDto
            {
                Id = giftInCart.Id,
                GiftId = giftInCart.GiftId,
                GiftName = giftInCart.Gift?.Name,
                giftPictureUrl = giftInCart.Gift?.PictureUrl,
                giftCardPrice = giftInCart.Gift?.CardPrice.ToString(),
                Qty = giftInCart.Qty
            } : null;

        }
        public async Task<GiftInCartDto?> GetGiftInCartByIdAndByPackageAsync(int giftid, int packageInCartId)
        {

            var giftInCart = await _giftInCartRepository.GetGiftInCartByIdAndByPackageAsync(giftid, packageInCartId);

            return giftInCart != null ? new GiftInCartDto
            {
                Id = giftInCart.Id,
                GiftId = giftInCart.GiftId,
                GiftName = giftInCart.Gift?.Name,
                giftPictureUrl = giftInCart.Gift?.PictureUrl,
                giftCardPrice = giftInCart.Gift?.CardPrice.ToString(),
                Qty = giftInCart.Qty
            } : null;
        }
        private void ValidateSpaceInPackage(PackageInCartModel package, GiftModel gift, int requestedQty)
        {
            int currentTypeCount = package.GiftsInPackage
                 .Where(g => g.Gift.CardPrice == gift.CardPrice)
                 .Sum(g => g.Qty);
            int maxAllowed = gift.CardPrice switch
            {
                CardPriceEnum.Classic => package.Package.QtyClassicCards,
                CardPriceEnum.Special => package.Package.QtySpecialCards,
                CardPriceEnum.Primum => package.Package.QtyPrimumCards,
                _ => 0
            };
            if (currentTypeCount + requestedQty > maxAllowed)
            {
                throw new Exception($"אין מספיק מקום בסל לכרטיסים מסוג {gift.CardPrice}");
            }
        }
        public async Task<GiftInCartDto> CreateOrUpdateGiftInCartAsync(GiftInCartCreateDto giftInCart)
        {
            var existing = await _giftInCartRepository.GetGiftInCartByIdAndByPackageAsync(giftInCart.GiftId, giftInCart.PackageInCartId);
            var package = await _PackageInCartRepository.GetPackageInCartByIdAsync(giftInCart.PackageInCartId);
            if (package == null)
                return null;
            var gift = await _giftRepoditory.GetGiftByIdAsync(giftInCart.GiftId);
            if (gift == null)
                return null;
            if (existing == null)
            {

                ValidateSpaceInPackage(package, gift, giftInCart.Qty);

                var newGiftInCart = new GiftInCartModel()
                {
                    PackageInCartId = giftInCart.PackageInCartId,
                    GiftId = giftInCart.GiftId,
                    Qty = giftInCart.Qty,
                    CartId = package.CartId
                };

                var createGiftInCart = await _giftInCartRepository.CreateGiftInCartAsync(newGiftInCart);
                return new GiftInCartDto
                {
                    Id = createGiftInCart.Id,
                    GiftId = createGiftInCart.GiftId,
                    GiftName = createGiftInCart.Gift?.Name,
                    giftPictureUrl = createGiftInCart.Gift?.PictureUrl,
                    giftCardPrice = createGiftInCart.Gift?.CardPrice.ToString(),
                    Qty = createGiftInCart.Qty
                };
            }
            else
            {
                if (existing.Qty + giftInCart.Qty > 0)
                {
                    ValidateSpaceInPackage(package, gift, giftInCart.Qty);
                    existing.Qty += giftInCart.Qty;
                    var updatedGiftInCart = await _giftInCartRepository.UpdateGiftAsync(existing);
                    return new GiftInCartDto
                    {
                        Id = updatedGiftInCart.Id,
                        GiftId = updatedGiftInCart.GiftId,
                        GiftName = updatedGiftInCart.Gift?.Name,
                        giftPictureUrl = updatedGiftInCart.Gift?.PictureUrl,
                        giftCardPrice = updatedGiftInCart.Gift?.CardPrice.ToString(),
                        Qty = updatedGiftInCart.Qty
                    };
                }
                else
                {
                    await _giftInCartRepository.DeleteGiftInCartAsync(existing.Id);
                    return null;
                }
            }

        }
        public async Task<bool> DeleteGiftInCartAsync(int id)
        {
            return await _giftInCartRepository.DeleteGiftInCartAsync(id);
        }
    }
}
