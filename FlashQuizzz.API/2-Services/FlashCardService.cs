using FlashQuizzz.API.DAO;
using FlashQuizzz.API.DAO.Interfaces;
using FlashQuizzz.API.Exceptions;
using FlashQuizzz.API.Models;
using Microsoft.EntityFrameworkCore;
using FlashQuizzz.API.Utilities;

namespace FlashQuizzz.API.Services
{
    public class FlashCardService : IFlashCardService
    {
        private readonly AppDbContext _context;

        public FlashCardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FlashCard> CreateFlashCard(FlashCardDTO newFlashCardDTO)
        {
            var flashCard = FlashCardUtility.DTOToFlashCard(newFlashCardDTO);

            _context.FlashCard.Add(flashCard);
            await _context.SaveChangesAsync();

            return flashCard;
        }

        public async Task<FlashCard?> Delete(int id)
        {
            var flashCard = await _context.FlashCard.FindAsync(id);

            if (flashCard == null)
                return null;

            _context.FlashCard.Remove(flashCard);
            await _context.SaveChangesAsync();

            return flashCard;
        }

        public async Task<ICollection<FlashCard>> GetAllFlashCards()
        {
            return await _context.FlashCard.ToListAsync();
        }

        public async Task<FlashCard?> GetByFlashCardName(string flashCardName)
        {
            return await _context.FlashCard
                .FirstOrDefaultAsync(fc => fc.FlashCardQuestion == flashCardName);
        }

        public async Task<FlashCard?> GetByFlashCardNameAndUserID(string flashCardName, string userId)
        {
            return await _context.FlashCard
                .FirstOrDefaultAsync(fc => fc.FlashCardQuestion == flashCardName && fc.UserID == userId);
        }

        public async Task<FlashCard?> GetByFlashCardNumber(int flashCardNumber)
        {
            return await _context.FlashCard
                .FirstOrDefaultAsync(fc => fc.FlashCardID == flashCardNumber);
        }

        public async Task<FlashCard?> GetByFlashCardNumberAndUserID(int flashCardID, string userId)
        {
            return await _context.FlashCard
                .FirstOrDefaultAsync(fc => fc.FlashCardID == flashCardID && fc.UserID == userId);
        }

        public async Task<ICollection<FlashCard>> GetByUser(string userId)
        {
            return await _context.FlashCard
                .Where(fc => fc.UserID == userId)
                .ToListAsync();
        }

        public async Task<FlashCard?> GetFlashCardById(int flashCardID)
        {
            return await _context.FlashCard.FindAsync(flashCardID);
        }

        public async Task<bool> Update(int id, FlashCardDTO updatedFlashCardDTO)
        {
            var flashCard = await _context.FlashCard.FindAsync(id);

            if (flashCard == null)
                return false;

            flashCard.FlashCardQuestion = updatedFlashCardDTO.FlashCardQuestion;
            flashCard.FlashCardAnswer = updatedFlashCardDTO.FlashCardAnswer;
            flashCard.CreatedDate = updatedFlashCardDTO.CreatedDate;
            flashCard.UserID = updatedFlashCardDTO.UserID ?? flashCard.UserID;

            _context.FlashCard.Update(flashCard);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
