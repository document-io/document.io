using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DocumentIO
{
	public class QueryLabelsResovler : IDocumentIOResolver<IEnumerable<Label>>
	{
		private readonly DatabaseContext databaseContext;

		public QueryLabelsResovler(DatabaseContext databaseContext)
		{
			this.databaseContext = databaseContext;
		}

		public async Task<IEnumerable<Label>> Resolve(DocumentIOResolveFieldContext<object> context)
		{
			var accountId = context.GetAccountId();
			var filter = context.GetFilter<LabelsFilter>();

			return await filter.Filtered(
					databaseContext.Labels.AsNoTracking(),
					labels => labels.Where(label => label.Board.Organization.Accounts.Any(account => account.Id == accountId)),
					label => label.Id)
				.ToListAsync();
		}
	}
}