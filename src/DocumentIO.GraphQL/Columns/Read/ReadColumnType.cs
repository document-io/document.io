using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace DocumentIO
{
	public class ReadColumnType : ObjectGraphType<Column>
	{
		public ReadColumnType(IDataLoaderContextAccessor accessor)
		{
			Field(x => x.Id);
			Field(x => x.Name);
			Field(x => x.Order);

			Field<ReadBoardType, Board>("board")
				.ResolveAsync(context =>
				{
					var databaseContext = context.GetDatabaseContext();

					var loader = accessor.Context.GetOrAddBatchLoader<Guid, Board>(
						"ColumnBoard",
						async ids => await databaseContext.Columns
							.Include(column => column.Board)
							.Where(column => ids.Contains(column.Id))
							.ToDictionaryAsync(column => column.Id, column => column.Board));

					return loader.LoadAsync(context.Source.Id);
				});

			Field<ListGraphType<ReadCardType>, IEnumerable<Card>>("cards")
				.Argument<CardsFilterType>("filter", q => q.DefaultValue = new CardsFilter())
				.ResolveAsync(context =>
				{
					var databaseContext = context.GetDatabaseContext();
					var filter = context.GetArgument<CardsFilter>("filter");

					var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid, Card>(
						"ColumnCards",
						async ids => 
							await filter.Filter(databaseContext.Cards)
								.Where(card => ids.Contains(card.ColumnId))
								.ToListAsync(),
						card => card.ColumnId);

					return loader.LoadAsync(context.Source.Id);
				});
		}
	}
}