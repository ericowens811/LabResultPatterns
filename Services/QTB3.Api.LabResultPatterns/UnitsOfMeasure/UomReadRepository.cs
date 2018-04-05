using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QTB3.Api.Abstractions.Repositories;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.Contexts;
using QTB3.Model.LabResultPatterns.Paging;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Api.LabResultPatterns.UnitsOfMeasure
{
    public class UomReadRepository : IReadRepository<Uom>
    {
        public const string BadSkipMessage = "Skip must not be < 0";
        public const string BadTakeMessage = "Take must not be < 1";
        public const string BadIdMessage = "Invalid id";

        private readonly PropertyContext _context;
        public UomReadRepository(PropertyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IPage<Uom>> GetAsync(string searchText, int skip, int take)
        {
            if (skip < 0) throw new ArgumentOutOfRangeException(BadSkipMessage);
            if (take < 1) throw new ArgumentOutOfRangeException(BadTakeMessage);

            var query = _context.Uoms.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(u => u.Name.Contains(searchText));
            }

            var totalCount = await query
                .CountAsync()
                .ConfigureAwait(false);

            var items = await query
                .OrderBy(u => u.Name)
                .Skip(skip)
                .Take(take)
                .ToListAsync()
                .ConfigureAwait(false);

            return new Page<Uom>
            (
                searchText: searchText,
                totalCount: totalCount,
                skip: skip,
                take: take,
                items: items
            );
        }

        public async Task<Uom> GetAsync(int id)
        {
            if(id < 1) throw new ArgumentOutOfRangeException(BadIdMessage);

            return await _context.Uoms
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == id)
                .ConfigureAwait(false);
        }
    }
}
