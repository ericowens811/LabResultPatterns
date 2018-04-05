using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QTB3.Api.Abstractions.Repositories;
using QTB3.Model.Abstractions;

namespace QTB3.Api.Common.Repositories
{
    public class WriteRepository<TItem> : IWriteRepository<TItem> where TItem: class, IId, INullifyNavProp
    {
        public const string BadIdMessage = "Id < 1";
        private readonly DbContext _context;

        public WriteRepository
        (
            DbContext context
        )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TItem> PostAsync(TItem value)
        {
            if(value == null) throw new ArgumentNullException(nameof(value));
            value.NullifyNavProps();
            _context.Add(value);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return value;
        }

        public async Task PutAsync(TItem value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            value.NullifyNavProps();
            _context.Update(value);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if(id<1) throw new ArgumentOutOfRangeException(BadIdMessage);
            var result = true;
            var item = await _context
                .Set<TItem>()
                .SingleOrDefaultAsync(u => u.Id == id)
                .ConfigureAwait(false);
            if (item == null)
            {
                result = false;
            }
            else
            {
                _context.Set<TItem>().Remove(item);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            return result;
        }

    }
}
