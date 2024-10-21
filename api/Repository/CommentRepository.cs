using api.Interfaces;
using api.Models;
using FinShark.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDBContext context;

    public CommentRepository(ApplicationDBContext context)
    {
        this.context = context;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        return await context.Comments.ToListAsync();
    }

    public Task<Comment?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
