using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phema.Validation;
using Phema.Validation.Conditions;

namespace DocumentIO
{
	public class CreateBoardValidation : IDocumentIOValidation<object>
	{
		private readonly DatabaseContext databaseContext;

		public CreateBoardValidation(DatabaseContext databaseContext)
		{
			this.databaseContext = databaseContext;
		}

		public async Task Validate(DocumentIOResolveFieldContext<object> context, IValidationContext validationContext)
		{
			var accountId = context.GetAccountId();
			var model = context.GetArgument<Board>();

			validationContext.When(model, m => m.Name)
				.IsNullOrWhitespace()
				.AddError("Название доски не задано");

			if (validationContext.IsValid(model, m => m.Name))
			{
				var boardExists = await databaseContext.Boards
					.Where(x => x.Organization.Accounts.Any(account => account.Id == accountId))
					.AnyAsync(x => x.Name == model.Name);

				validationContext.When(model, m => m.Name)
					.Is(() => boardExists)
					.AddError("Доска с таким именем уже существует");
			}
		}
	}
}