using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DocumentIO
{
	public class QueryColumnsResolver : IDocumentIOResolver<IEnumerable<Column>>
	{
		private readonly DatabaseContext databaseContext;

		public QueryColumnsResolver(DatabaseContext databaseContext)
		{
			this.databaseContext = databaseContext;
		}

		public async Task<IEnumerable<Column>> Resolve(DocumentIOResolveFieldContext<object> context)
		{
			var accountId = context.GetAccountId();
			var filter = context.GetFilter<ColumnsFilter>();

			return await filter.Filtered(
					databaseContext.Columns.AsNoTracking(),
					columns => columns.Where(column =>
						column.Board.Organization.Accounts.Any(account => account.Id == accountId)),
					orderBy: column => column.Order)
				.ToListAsync();
		}
	}
}