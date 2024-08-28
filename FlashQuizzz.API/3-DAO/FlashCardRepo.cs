
using FlashQuizzz.API.DAO.Interfaces;
using FlashQuizzz.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashQuizzz.API.DAO;

public class FlashCardRepo : IFlashCardRepo
{

    private readonly AppDbContext _context;

    public FlashCardRepo(AppDbContext context)
    {
        this._context = context;
    }

    public async Task<FlashCard> Create(FlashCard item)
    {
        _context.FlashCard.Add(item);
        await _context.SaveChangesAsync();

        return item;
    }

    public async Task<ICollection<FlashCard>> GetAll()
    {
        return await _context.FlashCard.Include(r => r.User).ToListAsync();
    }

    public async Task<FlashCard?> GetByID(int ID)
    {
        return await _context.FlashCard
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.FlashCardID == ID);
    }

    public async Task<ICollection<FlashCard>>? GetByCategoryID(int ID)
    {
        return await _context.FlashCard
                .Where(r => r.FlashCardCategoryID == ID)
                .ToListAsync();
    }

    public async Task<FlashCard?> GetByFlashCardQuestion(string flashCardQuestion)
    {
        return await _context.FlashCard
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.FlashCardQuestion == flashCardQuestion);
    }

    public async Task<FlashCard?> GetByFlashCardQuestionAndUserID(string flashCardQuestion, string UserID)
    {
        return await _context.FlashCard
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.FlashCardQuestion == flashCardQuestion && r.UserID == UserID);
    }

    public async Task<FlashCard?> GetByFlashCardAnswerAndUserID(string flashCardAnswer, string userId)
    {
        return await _context.FlashCard
                 .Include(r => r.User)
                 .FirstOrDefaultAsync(r => r.FlashCardAnswer == flashCardAnswer && r.UserID == userId);
    }

    public async Task<ICollection<FlashCard>> GetByUser(string ID)
    {

        return await _context.FlashCard
                .Where(r => r.UserID == ID)
                .ToListAsync();

    }

    public async Task<bool> Update(int ID, FlashCard newItem)
    {
        FlashCard? oldFlashCard = await _context.FlashCard.FirstOrDefaultAsync(r => r.FlashCardID == ID);

        if (oldFlashCard == null)
        {
            return false;
        }

        oldFlashCard.FlashCardQuestion = newItem.FlashCardQuestion;
        oldFlashCard.FlashCardAnswer = newItem.FlashCardAnswer;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<FlashCard> Delete(int ID)
    {
        FlashCard flashCard = _context.FlashCard.Find(ID)!;

        _context.FlashCard.Remove(flashCard);
        await _context.SaveChangesAsync();

        return flashCard;
    }

    public async Task<FlashCard?> GetByFlashCardAnswer(string flashCardAnswer)
    {
        return await _context.FlashCard
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.FlashCardAnswer == flashCardAnswer);
    }

    Task<List<FlashCard>> IFlashCardRepo.GetByUser(string userId)
    {
        throw new NotImplementedException();
    }

    public void GetByFlashCardNumberAndUserID(int flashCardID, string userId)
    {
        throw new NotImplementedException();
    }
}