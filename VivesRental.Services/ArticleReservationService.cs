using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Extensions;
using VivesRental.Services.Mappers;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services;

public class ArticleReservationService : IArticleReservationService
{
    private readonly VivesRentalDbContext _context;

    public ArticleReservationService(VivesRentalDbContext context)
    {
        _context = context;
    }
    public Task<ArticleReservationResult?> Get(Guid id)
    {
        return _context.ArticleReservations
            .Where(ar => ar.Id == id)
            .MapToResults()
            .FirstOrDefaultAsync();
    }
        
    public Task<List<ArticleReservationResult>> Find(ArticleReservationFilter? filter = null)
    {
        return _context.ArticleReservations
            .ApplyFilter(filter)
            .MapToResults()
            .ToListAsync();
    }
        
    public async Task<ArticleReservationResult?> Create(ArticleReservationRequest request)
    {
        request.FromDateTime ??= DateTime.Now;
        request.UntilDateTime ??= DateTime.Now.AddMinutes(5);

        var articleReservation = new ArticleReservation
        {
            CustomerId = request.CustomerId,
            ArticleId = request.ArticleId,
            FromDateTime = request.FromDateTime.Value,
            UntilDateTime = request.UntilDateTime.Value
        };

        _context.ArticleReservations.Add(articleReservation);

        await _context.SaveChangesAsync();

        return await Get(articleReservation.Id);
    }

    /// <summary>
    /// Removes one ArticleReservation
    /// </summary>
    /// <param name="id">The id of the ArticleReservation</param>
    /// <returns>True if the article reservation was deleted</returns>
    public async Task<bool> Remove(Guid id)
    {
        var articleReservation = new ArticleReservation { Id = id };
        _context.ArticleReservations.Attach(articleReservation);
        _context.ArticleReservations.Remove(articleReservation);

        await _context.SaveChangesAsync();

        return true;
    }
}