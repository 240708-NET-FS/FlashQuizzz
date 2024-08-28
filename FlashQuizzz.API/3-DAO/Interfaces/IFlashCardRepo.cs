using FlashQuizzz.API.Models;

namespace FlashQuizzz.API.DAO.Interfaces;

public interface IFlashCardRepo
{
    // Create
    public Task<FlashCard> Create(FlashCard item);

    // Read
    public Task<FlashCard?> GetByID(int ID);
    public Task<List<FlashCard>> GetByUser(string userId);

    public Task<ICollection<FlashCard>>? GetByCategoryID(int ID);

    public Task<ICollection<FlashCard>>? GetAll();

    // Update
    public Task<bool> Update(int ID, FlashCard newItem);

    // Delete
    public Task<FlashCard> Delete(int ID);
    void GetByFlashCardNumberAndUserID(int flashCardID, string userId);
    
}