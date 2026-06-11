using LotteryApi.Dtos;
using LotteryApi.Enums;
using LotteryApi.Exceptions;
using LotteryApi.Models;
using LotteryApi.Repositories;
using Serilog;

namespace LotteryApi.Services
{
    public class GiftService : IGiftService
    {
        private readonly IGiftRepoditory _giftRepository;
        public GiftService(IGiftRepoditory giftRepository)
        {
            _giftRepository = giftRepository;
        }
        //public async Task<IEnumerable<GiftDto>> GetGiftsAsync()
        //{
        //    var gifts = await _giftRepository.GetGiftsAsync();
        //    return gifts.Select(g => new GiftDto
        //    {
        //        Id = g.Id,
        //        Name = g.Name,
        //       Description = g.Description,
        //       CategoryId = g.CategoryId,
        //       DonorId = g.DonorId,
        //       CategoryName = g.Category!= null ? g.Category.Name : null,
        //       DonorName = g.Donor != null ? g.Donor.Name : null,
        //       PrizeQuantity = g.PrizeQuantity,
        //       CardPrice = g.CardPrice.ToString(),
        //       PictureUrl = g.PictureUrl,
        //       GifPurchased=g.GifPurchased.Select(go=> new GiftInOrderModel
        //       {
        //           Id= go.Id,
        //           GiftId= go.GiftId,
        //           IsWinner= go.IsWinner
        //       }).ToList()
        //    });

        //}
        public async Task<IEnumerable<GiftDto>> GetGiftsAsync()
        {
            // שליפת הנתונים מה-Repository (כולל כל ה-Includes שסידרנו)
            var gifts = await _giftRepository.GetGiftsAsync();

            return gifts.Select(g => new GiftDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                CategoryId = g.CategoryId,
                CategoryName = g.Category?.Name, // מגיע מ-CategoryModel
                PrizeQuantity = g.PrizeQuantity,
                CardPrice = g.CardPrice.ToString(),
                PictureUrl = g.PictureUrl,
                DonorId = g.DonorId,
                DonorName = g.Donor?.Name, // מגיע מ-DonorModel
                PurchasersCount = g.GifPurchased?.Count() ?? 0,
                // מיפוי רשימת הרוכשים לתוך ה-DTO הייעודי
                // שימי לב: אנחנו לא שולחים כאן שוב את שם המתנה כי הוא כבר למעלה (g.Name)
                GifPurchased = g.GifPurchased?.Select(gp => new GiftPurchaserDto
                {
                    Id = gp.Id,
                    IsWinner = gp.IsWinner,
                    ParticipantName = gp.PackageInOrder?.Order?.Participant?.Name,
                    ParticipantPhone = gp.PackageInOrder?.Order.Participant?.Phone,
                    ParticipantEmail = gp.PackageInOrder?.Order.Participant?.Email
                }).ToList() ?? []
            });
        }
        public async Task<GiftDto?> GetGiftByIdAsync(int id)
        {
            var gift = await _giftRepository.GetGiftByIdAsync(id.ToString());
            return gift != null ? new GiftDto
            {
                Id = gift.Id,
                Name = gift.Name,
                Description = gift.Description,
                CategoryId = gift.CategoryId,
                CategoryName = gift.Category?.Name,
                PrizeQuantity = gift.PrizeQuantity,
                CardPrice = gift.CardPrice.ToString(),
                PictureUrl = gift.PictureUrl,
                DonorId = gift.DonorId,
                DonorName = gift.Donor?.Name,
                PurchasersCount = gift.GifPurchased?.Count() ?? 0,

                GifPurchased = gift.GifPurchased?.Select(gp => new GiftPurchaserDto
                {
                    Id = gp.Id,
                    IsWinner = gp.IsWinner,
                    ParticipantName = gp.PackageInOrder?.Order?.Participant?.Name,
                    ParticipantPhone = gp.PackageInOrder?.Order.Participant?.Phone,
                    ParticipantEmail = gp.PackageInOrder?.Order.Participant?.Email

                }).ToList() ?? []
            } : null;

        }

        public async Task<GiftDto> CreateGiftAsync(GiftCreateDto gift)
        {
            var newGift = new GiftModel()
            {
                Name = gift.Name,
                Description = gift.Description,
                CategoryId = gift.CategoryId,
                DonorId = gift.DonorId,
                PrizeQuantity = gift.PrizeQuantity,
                CardPrice = gift.CardPrice,
                PictureUrl = gift.PictureUrl
            };

            var createGift = await _giftRepository.CreateGiftAsync(newGift);
            var giftWithDetails = await _giftRepository.GetGiftByIdAsync(createGift.Id);
            if (giftWithDetails == null) return null;
            return new GiftDto
            {
                Id = giftWithDetails.Id,
                Name = giftWithDetails.Name,
                Description = giftWithDetails.Description,
                CategoryId = giftWithDetails.CategoryId,
                CategoryName = giftWithDetails.Category?.Name, // מגיע מ-CategoryModel
                PrizeQuantity = giftWithDetails.PrizeQuantity,
                CardPrice = giftWithDetails.CardPrice.ToString(),
                PictureUrl = giftWithDetails.PictureUrl,
                DonorId = giftWithDetails.DonorId,
                DonorName = giftWithDetails.Donor?.Name,
                PurchasersCount = giftWithDetails.GifPurchased?.Count() ?? 0,
                GifPurchased = giftWithDetails.GifPurchased?.Select(gp => new GiftPurchaserDto
                {
                    Id = gp.Id,
                    IsWinner = gp.IsWinner,
                    ParticipantName = gp.PackageInOrder?.Order?.Participant?.Name,
                    ParticipantPhone = gp.PackageInOrder?.Order.Participant?.Phone,
                    ParticipantEmail = gp.PackageInOrder?.Order.Participant?.Email

                }).ToList() ?? []
            };
        }

        public async Task<GiftDto?> UpdateGiftAsync(int id, GiftUpdateDto updateGift)
        {
            var existing = await _giftRepository.GetGiftByIdAsync(id.ToString());
            if (existing == null)
            {
                return null;
            }
            if (updateGift.Name != null) existing.Name = updateGift.Name;
            if (updateGift.Description != null) existing.Description = updateGift.Description;
            existing.CategoryId = updateGift.CategoryId ?? existing.CategoryId;
            existing.DonorId = updateGift.DonorId ?? existing.DonorId;
            existing.PrizeQuantity = updateGift.PrizeQuantity ?? existing.PrizeQuantity;
            existing.CardPrice = updateGift.CardPrice ?? existing.CardPrice;
            if (updateGift.PictureUrl != null) existing.PictureUrl = updateGift.PictureUrl;
            var newUpdateGift = await _giftRepository.UpdateGiftAsync(existing);
            var giftWithDetails = await _giftRepository.GetGiftByIdAsync(newUpdateGift.Id);
            if (giftWithDetails == null) return null;
            return new GiftDto
            {
                Id = giftWithDetails.Id,
                Name = giftWithDetails.Name,
                Description = giftWithDetails.Description,
                CategoryId = giftWithDetails.CategoryId,
                CategoryName = giftWithDetails.Category?.Name,
                PrizeQuantity = giftWithDetails.PrizeQuantity,
                CardPrice = giftWithDetails.CardPrice.ToString(),
                PictureUrl = giftWithDetails.PictureUrl,
                DonorId = giftWithDetails.DonorId,
                DonorName = giftWithDetails.Donor?.Name,
                PurchasersCount = giftWithDetails.GifPurchased?.Count() ?? 0,
                GifPurchased = giftWithDetails.GifPurchased?.Select(gp => new GiftPurchaserDto
                {
                    Id = gp.Id,
                    IsWinner = gp.IsWinner,
                    ParticipantName = gp.PackageInOrder?.Order?.Participant?.Name,
                    ParticipantPhone = gp.PackageInOrder?.Order.Participant?.Phone,
                    ParticipantEmail = gp.PackageInOrder?.Order.Participant?.Email

                }).ToList() ?? []
            };
        }
        public async Task<bool> DeleteGiftAsync(int id)
        {
            return await _giftRepository.DeleteGiftAsync(id.ToString());
        }
        public async Task<IEnumerable<GiftDto>> SearchGiftsAsync(string? giftName, string? donorName, int? minPurchasers)
        {
            var gifts = await _giftRepository.SearchGiftsAsync(giftName, donorName, minPurchasers);

            return gifts.Select(g => new GiftDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                CategoryName = g.Category?.Name,
                DonorName = g.Donor?.Name,
                PrizeQuantity = g.PrizeQuantity,
                CardPrice = g.CardPrice.ToString(),
                PictureUrl = g.PictureUrl,
                PurchasersCount = g.GifPurchased?.Count ?? 0,

                GifPurchased = g.GifPurchased?
                    .Select(gp => new GiftPurchaserDto
                    {
                        Id = gp.Id,
                        ParticipantName = gp.PackageInOrder?.Order?.Participant?.Name,
                        ParticipantPhone = gp.PackageInOrder?.Order.Participant?.Phone,
                        ParticipantEmail = gp.PackageInOrder?.Order.Participant?.Email,
                        IsWinner = gp.IsWinner,
                    }).ToList() ?? new List<GiftPurchaserDto>()
            });
        }
        public async Task<IEnumerable<GiftDto>> FilteredGiftsAsync(int? categoryId, CardPriceEnum? priceType)
        {
            var gifts = await _giftRepository.FilterGiftsAsync(categoryId, priceType);

            return gifts.Select(g => new GiftDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                CategoryName = g.Category?.Name,
                DonorName = g.Donor?.Name,
                PrizeQuantity = g.PrizeQuantity,
                CardPrice = g.CardPrice.ToString(),
                PictureUrl = g.PictureUrl,
                PurchasersCount = g.GifPurchased?.Count ?? 0,

                GifPurchased = g.GifPurchased?
                    .Select(gp => new GiftPurchaserDto
                    {
                        Id = gp.Id,
                        ParticipantName = gp.PackageInOrder?.Order?.Participant?.Name,
                        ParticipantPhone = gp.PackageInOrder?.Order.Participant?.Phone,
                        ParticipantEmail = gp.PackageInOrder?.Order.Participant?.Email,
                        IsWinner = gp.IsWinner,
                    }).ToList() ?? new List<GiftPurchaserDto>()
            });
        }
        public async Task<IEnumerable<GiftDto>> SortedGiftsExpensiveAsync(string sortBy)
        {

            var gifts = await _giftRepository.SortedGiftsExpensiveAsync(sortBy);

            return gifts.Select(g => new GiftDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                CategoryName = g.Category?.Name,
                DonorName = g.Donor?.Name,
                PrizeQuantity = g.PrizeQuantity,
                CardPrice = g.CardPrice.ToString(),
                PictureUrl = g.PictureUrl,
                PurchasersCount = g.GifPurchased?.Count ?? 0,

                GifPurchased = g.GifPurchased?
                    .Select(gp => new GiftPurchaserDto
                    {
                        Id = gp.Id,
                        ParticipantName = gp.PackageInOrder?.Order?.Participant?.Name,
                        ParticipantPhone = gp.PackageInOrder?.Order.Participant?.Phone,
                        ParticipantEmail = gp.PackageInOrder?.Order.Participant?.Email,
                        IsWinner = gp.IsWinner,
                    }).ToList() ?? new List<GiftPurchaserDto>()
            });
        }
        //public async Task<UserDto> RunLotteryForGiftAsync(int giftId)
        //{
        //    // 1. שליפת המתנה עם כל הכרטיסים שנקנו עבורה (GifPurchased)
        //    var gift = await _giftRepository.GetGiftByIdAsync(giftId);

        //    if (gift == null) throw new NotFoundException("מתנה לא נמצאה.");

        //    // 2. בדיקה האם כבר יש כרטיס שסומן כ"זוכה" עבור המתנה הזו
        //    var existingWinner = gift.GifPurchased.FirstOrDefault(gp => gp.IsWinner);
        //    if (existingWinner != null)
        //        throw new ConflictException($"כבר בוצעה הגרלה למתנה זו. הזוכה הוא: {existingWinner.PackageInOrder.Order.Participant.Name}");

        //    // 3. איסוף כל הכרטיסים
        //    var allTickets = gift.GifPurchased.ToList();
        //    if (!allTickets.Any())
        //        throw new BadRequestException("אין רוכשים למתנה זו, לא ניתן להגריל.");

        //    // 4. הגרלה אקראית של כרטיס אחד
        //    Random random = new Random();
        //    var winningTicket = allTickets[random.Next(allTickets.Count)];

        //    // 5. עדכון הכרטיס כ"זוכה" ושמירה ב-Database
        //    winningTicket.IsWinner = true; // נניח שלשדה קוראים IsWinner
        //    await _giftRepository.UpdatePurchaseAsync(winningTicket);

        //    Log.Information("הגרלה הסתיימה! הזוכה במתנה {GiftName} הוא {User}", gift.Name, winningTicket.Order.User.FirstName);

        //    return _mapper.Map<UserDTO>(winningTicket.Order.User);
        //}
        //// אופציה 2: הגרלת כל מה שנותר
        //public async Task<List<LotteryResultDTO>> RunAllRemainingLotteriesAsync()
        //{
        //    var results = new List<LotteryResultDTO>();
        //    // שליפת כל המתנות שעדיין אין להן זוכה
        //    var pendingGifts = await _giftRepository.GetGiftsWithoutWinnersAsync();

        //    foreach (var gift in pendingGifts)
        //    {
        //        try
        //        {
        //            var winner = await RunLotteryForGiftAsync(gift.Id);
        //            results.Add(new LotteryResultDTO { GiftName = gift.Name, WinnerName = winner.UserName });
        //        }
        //        catch (Exception ex)
        //        {
        //            // אם מתנה מסוימת נכשלה (למשל אין רוכשים), נרשום לוג ונמשיך לבאה בתור
        //            Log.Warning("דילוג על הגרלת מתנה {GiftId}: {Message}", gift.Id, ex.Message);
        //        }
        //    }

        //    return results;
        //}
    }
}
