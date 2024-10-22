using api.Interfaces;
using api.Models;
using FinShark.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository(ApplicationDBContext context) : ICommentRepository
{
    private readonly ApplicationDBContext context = context;

    public async Task<Comment> CreateAsync(Comment commentModel)
    {
        await context.Comments.AddAsync(commentModel);
        await context.SaveChangesAsync();

        return commentModel;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var commentModel = await context.Comments.FindAsync(id);

        if (commentModel == null)
        {
            return null;
        }

        context.Comments.Remove(commentModel);
        await context.SaveChangesAsync();

        return commentModel;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        return await context.Comments.Include(c => c.AppUser).ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        var commentModel = await context
            .Comments.Include(c => c.AppUser)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (commentModel == null)
        {
            return null;
        }

        return commentModel;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
    {
        var existingComment = await context.Comments.FindAsync(id);

        if (existingComment == null)
        {
            return null;
        }

        existingComment.Title = commentModel.Title;
        existingComment.Content = commentModel.Content;

        await context.SaveChangesAsync();

        return existingComment;
    }
}
