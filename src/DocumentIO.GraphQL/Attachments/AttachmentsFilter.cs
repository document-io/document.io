using System;
using System.Linq;
using System.Linq.Expressions;

namespace DocumentIO
{
	public class AttachmentsFilter : DocumentIOFilter<CardAttachment>
	{
		public Guid? Id { get; set; }

		public override IQueryable<TPaginated> Filtered<TPaginated, TOrderBy>(
			IQueryable<CardAttachment> queryable,
			Func<IQueryable<CardAttachment>, IQueryable<TPaginated>> query,
			Expression<Func<TPaginated, TOrderBy>> orderBy)
		{
			if (Id != null)
				queryable = queryable.Where(attachment => attachment.Id == Id);

			return base.Filtered(queryable, query, orderBy);
		}
	}
}