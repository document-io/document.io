using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DocumentIO
{
	public class QueryEventsResovler : IDocumentIOResolver<IEnumerable<CardEvent>>
	{
		private readonly DatabaseContext databaseContext;

		public QueryEventsResovler(DatabaseContext databaseContext)
		{
			this.databaseContext = databaseContext;
		}

		public async Task<IEnumerable<CardEvent>> Resolve(DocumentIOResolveFieldContext<object> context)
		{
			var accountId = context.GetAccountId();
			var filter = context.GetFilter<EventsFilter>();

			return await filter.Filtered(
					databaseContext.CardEvents.AsNoTracking(),
					query: events => events.Where(@event =>
						@event.Account.Organization.Accounts.Any(account => account.Id == accountId)),
					orderBy: @event => @event.Id)
				.ToListAsync();
		}
	}
}