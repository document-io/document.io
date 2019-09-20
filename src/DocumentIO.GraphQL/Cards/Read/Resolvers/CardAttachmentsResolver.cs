using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace DocumentIO
{
	public class CardAttachmentsResolver : IGraphQLResolver<Card, IEnumerable<CardAttachment>>
	{
		private readonly DatabaseContext databaseContext;
		private readonly IDataLoaderContextAccessor accessor;

		public CardAttachmentsResolver(IDataLoaderContextAccessor accessor, DatabaseContext databaseContext)
		{
			this.accessor = accessor;
			this.databaseContext = databaseContext;
		}

		public Task<IEnumerable<CardAttachment>> Resolve(DocumentIOResolveFieldContext<Card> context)
		{
			var filter = context.GetFilter<AttachmentFilter>();

			var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid, CardAttachment>(
				"CardAttachment",
				async ids =>
					await filter.Filtered(
							databaseContext.CardAttachments.AsNoTracking(),
							attachments => attachments.Where(attachment => ids.Contains(attachment.CardId)))
						.ToListAsync(),
				cardLabel => cardLabel.CardId);

			return loader.LoadAsync(context.Source.Id);
		}
	}
}