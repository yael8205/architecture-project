using LotteryApi.Dtos;
using LotteryApi.Exceptions;
using LotteryApi.Models;
using LotteryApi.Repositories;
using System.Security.Claims;

namespace LotteryApi.Services
{
    public class PackageInCartService : IPackageInCartService
    {
        private readonly IPackageInCartRepository _packageInCartRepository ;
        private readonly IPackageRepository _packageRepository ;
        private readonly IShoppingCartRepository _ShoppingCartRepository ;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PackageInCartService(IPackageInCartRepository packageInCartRepository,
            IPackageRepository packageRepository,
           IShoppingCartRepository shoppingCartRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _packageInCartRepository = packageInCartRepository;
            _packageRepository = packageRepository;
            _ShoppingCartRepository = shoppingCartRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PackageInCartDto?> GetPackageInCartByIdAsync(int id)
        {
            var packageInCart = await _packageInCartRepository.GetPackageInCartByIdAsync(id.ToString());

            return packageInCart != null ? new PackageInCartDto
            {
                Id = packageInCart.Id,
                PackageId = packageInCart.PackageId,
                PackageName = packageInCart.Package?.Name,
                PackagePrice = packageInCart.Package?.Price ?? 0,
                GiftsInPackage = packageInCart.GiftsInPackage?.Select(g => new GiftInCartDto
                {
                    Id = g.Id,
                    GiftId = g.GiftId,
                    GiftName = g.Gift?.Name,
                    giftPictureUrl = g.Gift?.PictureUrl,
                    giftCardPrice = g.Gift?.CardPrice.ToString(),
                    Qty = g.Qty
                }).ToList() ?? []
            } : null;
        }

        public async Task<PackageInCartDto> CreatePackageInCartAsync(PackageInCartCreateDto packageInCart)
        {
            var userIdCalm = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdCalm))
            {
                throw new UnauthorizedException("משתמש לא מחובר או חסר הרשאות.");
            }
            if (!int.TryParse(userIdCalm, out int userId))
            {
                return null;
            }
            var cart = await _ShoppingCartRepository.GetShoppingCartByUserIdAsync(userId.ToString());
            if (cart == null)
                throw new NotFoundException("סל הקניות של המשתמש לא נמצא.");
            var package = await _packageRepository.GetPackageByIdAsync(packageInCart.PackageId);
            if (package == null)
                throw new NotFoundException($"החבילה עם מזהה {packageInCart.PackageId} לא קיימת במערכת.");
            var newPackageInCart = new PackageInCartModel()
            {
                PackageId = packageInCart.PackageId,

                CartId = cart.Id

            };

            var createPackageInCart = await _packageInCartRepository.CreatePackageInCartAsync(newPackageInCart);
            var createPackageWithDetails = await _packageInCartRepository.GetPackageInCartByIdAsync(createPackageInCart.Id);
            if (createPackageWithDetails == null)
                throw new BadRequestException("אירעה שגיאה בשמירת החבילה בסל.");
            cart.SumPrice += package.Price;
            await _ShoppingCartRepository.UpdateShoppingCartAsync(cart);

            return new PackageInCartDto
            {
                Id = createPackageWithDetails.Id,
                PackageId = createPackageWithDetails.PackageId,
                PackageName = createPackageWithDetails.Package?.Name,
                PackagePrice = createPackageWithDetails.Package?.Price ?? 0,
                GiftsInPackage = createPackageWithDetails.GiftsInPackage?.Select(g => new GiftInCartDto
                {
                    Id = g.Id,
                    GiftId = g.GiftId,
                    GiftName = g.Gift?.Name,
                    giftPictureUrl = g.Gift?.PictureUrl,
                    giftCardPrice = g.Gift?.CardPrice.ToString(),
                    Qty = g.Qty
                }).ToList() ?? []
            };

        }
        public async Task<bool> DeletePackageInCartAsync(int id)
        {

            var packageInCart = await _packageInCartRepository.GetPackageInCartByIdAsync(id.ToString());
            if (packageInCart == null)
                return false;
            var userIdCalm = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdCalm, out int userId))
            {
                return false;
            }
            var cart = await _ShoppingCartRepository.GetShoppingCartByUserIdAsync(userId.ToString());
            if (cart == null)
                return false;
            if (cart.Id != packageInCart.CartId)
                return false;
            var price = packageInCart?.Package?.Price ?? 0;


            cart.SumPrice -= price;
            await _ShoppingCartRepository.UpdateShoppingCartAsync(cart);


            return await _packageInCartRepository.DeletePackageInCartAsync(id.ToString());


        }
    }
}
