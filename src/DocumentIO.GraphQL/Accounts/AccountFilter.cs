using System;
using System.Linq;

namespace DocumentIO
{
	public class AccountFilter : IFilter<Account>
	{
		public Guid? Id { get; set; }
		public string Role { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }

		public IQueryable<Account> Filter(IQueryable<Account> queryable)
		{
			if (Id != null)
				queryable = queryable.Where(account => account.Id == Id);

			if (Role != null)
				queryable = queryable.Where(account => account.Role == Role);
			
			if (Email != null)
				queryable = queryable.Where(account => account.Email == Email);
			
			if (FirstName != null)
				queryable = queryable.Where(account => account.FirstName.Contains(FirstName));

			if (MiddleName != null)
				queryable = queryable.Where(account => account.MiddleName.Contains(MiddleName));
			
			if (LastName != null)
				queryable = queryable.Where(account => account.LastName.Contains(LastName));

			return queryable;
		}
	}
}