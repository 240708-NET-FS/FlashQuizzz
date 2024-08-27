using FlashQuizzz.API.DAO;
using FlashQuizzz.API.DAO.Interfaces;
using FlashQuizzz.API.Exceptions;
using FlashQuizzz.API.Models;
using Microsoft.EntityFrameworkCore;
using FlashQuizzz.API.Utilities;
using Microsoft.IdentityModel.Tokens;

namespace FlashQuizzz.API.Services
{
    public class FlashCardService : IFlashCardService
    {
        private readonly AppDbContext _context;
        private readonly IFlashCardRepo _flashCardRepo;
        private AppDbContext appDbContext;

        public FlashCardService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public FlashCardService(AppDbContext context, IFlashCardRepo flashCardRepo)
        {
            _context = context;
            _flashCardRepo = flashCardRepo;
        }

        /// <summary>
        /// Creates a new flashcard with the given information, saves the changes to the database, and returns the newly created flashcard.
        /// </summary>
        /// <param name="newFlashCardDTO">The flashcard information to be saved.</param>
        /// <returns>The newly created flashcard.</returns>
        public async Task<FlashCard> CreateFlashCard(FlashCardDTO flashCardDTO)
        {
            FlashCard newFlashCard = FlashCardUtility.DTOToFlashCard(flashCardDTO);

            return await _flashCardRepo.Create(newFlashCard);

            // if (newFlashCard == null)
            //     return BadRequest("FlashCard data is required.");

            // var flashCard = await _flashCardService.CreateFlashCard(newFlashCard);
            // if (flashCard == null)
            //     return BadRequest("Failed to create FlashCard.");

            // return CreatedAtAction(nameof(GetFlashCardById), new { id = flashCard.FlashCardID }, flashCard);
        }

        public async Task<FlashCard?> Delete(int ID)
        {
            FlashCard? flashCard = await _flashCardRepo.GetByID(ID) ?? throw new InvalidFlashCardException("FlashCard does not exist.");
            return await _flashCardRepo.Delete(ID);
            // var flashCard = await _context.FlashCard.FindAsync(id);

            // if (flashCard == null)
            //     return null;

            // _context.FlashCard.Remove(flashCard);
            // await _context.SaveChangesAsync();

            // return flashCard;
        }

        public async Task<ICollection<FlashCard>> GetAllFlashCards()
        {
            ICollection<FlashCard> flashCard = await _flashCardRepo.GetAll();
            List<FlashCard> flashCardsList = flashCard.ToList();

            if(flashCardsList.IsNullOrEmpty())
            {
                throw new InvalidFlashCardCategoryException($"No flashcard found.");
            }

            return flashCardsList;
            // return await _context.FlashCard.ToListAsync();
        }

        public async Task<ICollection<FlashCard>> GetByCategory(int flashCardCategoryID)
        {
            if (flashCardCategoryID < 1) throw new ArgumentException("Invalid ID");

            ICollection<FlashCard> flashCard = await _flashCardRepo.GetByCategoryID(flashCardCategoryID);
             List<FlashCard> flashCardsList = flashCard.ToList();

            if(flashCardsList.IsNullOrEmpty())
            {
                throw new InvalidFlashCardException($"FlashCard Category with ID {flashCardCategoryID} could not be found.");
            }

            return flashCardsList;
            // return await _context.FlashCard
            //     .Where(fc => fc.FlashCardCategoryID == flashCardCategoryID)
            //     .ToListAsync();
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