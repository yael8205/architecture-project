using LotteryApi.Dtos;
using LotteryApi.Exceptions;
using LotteryApi.Models;
using LotteryApi.Repositories;



namespace LotteryApi.Services
    {
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }


        public async Task<ShoppingCartDto?> GetShoppingCartByIdAsync(int id)
        {
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartByIdAsync(id.ToString());

            return shoppingCart != null ? new ShoppingCartDto
            {
                Id = shoppingCart.Id,
                ParticipantId = shoppingCart.ParticipantId,
                ParticipantName = shoppingCart.Participant?.Name,
                PackagesInShoppingCart = shoppingCart.PackagesInShoppingCart?.Select(p => new PackageInCartDto
                {
                    Id = p.Id,
                    PackageId = p.PackageId,
                    PackageName = p.Package?.Name,
                    PackagePrice = p.Package?.Price ?? 0,
                    GiftsInPackage = p.GiftsInPackage?.Select(g => new GiftInCartDto
                    {
                        Id = g.Id,
                        GiftId = g.GiftId,
                        GiftName = g.Gift?.Name,
                        giftPictureUrl = g.Gift?.PictureUrl,
                        giftCardPrice = g.Gift?.CardPrice.ToString(),
                        Qty = g.Qty
                    }).ToList() ?? []
                }).ToList() ?? [],
                SumPrice = shoppingCart.SumPrice
            } : null;
        }
        public async Task<ShoppingCartDto?> GetShoppingCartByUserIdAsync(int id)
        {
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartByUserIdAsync(id.ToString());

            return shoppingCart != null ? new ShoppingCartDto
            {
                Id = shoppingCart.Id,
                ParticipantId = shoppingCart.ParticipantId,
                ParticipantName = shoppingCart.Participant?.Name,
                PackagesInShoppingCart = shoppingCart.PackagesInShoppingCart?.Select(p => new PackageInCartDto
                {
                    Id = p.Id,
                    PackageId = p.PackageId,
                    PackageName = p.Package?.Name,
                    PackagePrice = p.Package?.Price ?? 0,
                    GiftsInPackage = p.GiftsInPackage?.Select(g => new GiftInCartDto
                    {
                        Id = g.Id,
                        GiftId = g.GiftId,
                        GiftName = g.Gift?.Name,
                        giftPictureUrl = g.Gift?.PictureUrl,
                        giftCardPrice = g.Gift?.CardPrice.ToString(),
                        Qty = g.Qty
                    }).ToList() ?? []
                }).ToList() ?? [],
                SumPrice = shoppingCart.SumPrice
            } : null;
        }
        public async Task<ShoppingCartDto> CreateShoppingCartAsync(ShoppingCartCreateDto shoppingcart)
        {
            if (shoppingcart == null || string.IsNullOrEmpty(shoppingcart.ParticipantId))
            {
                throw new BadRequestException("נתוני סל הקניות אינם תקינים.");
            }
            var newShoppingCart = new ShoppingCartModel()
            {

                ParticipantId = shoppingcart.ParticipantId,

            };

            var createShoppingCart = await _shoppingCartRepository.CreateShoppingCartAsync(newShoppingCart);
            if (createShoppingCart == null)
            {
                throw new ConflictException("לא ניתן היה ליצור את סל הקניות..");
            }
            var createShoppingCartWithDetail = await _shoppingCartRepository.GetShoppingCartByIdAsync(createShoppingCart.Id);
            if (createShoppingCartWithDetail == null)
            {
                throw new NotFoundException("הסל נוצר אך לא נמצא במערכת.");
            }
            return createShoppingCart != null ? new ShoppingCartDto
            {
                Id = createShoppingCartWithDetail.Id,
                ParticipantId = createShoppingCartWithDetail.ParticipantId,
                ParticipantName = createShoppingCartWithDetail.Participant?.Name,
                PackagesInShoppingCart = [],
                SumPrice = 0

            } : null;
        }

        public async Task<bool> DeleteShoppingCartAsync(int id)
        {
            return await _shoppingCartRepository.DeleteShoppingCartAsync(id.ToString());
        }
        public async Task<bool> EmptyCartAsync(int id)
        {
            return await _shoppingCartRepository.EmptyCartAsync(id.ToString());
        }
    }

}

