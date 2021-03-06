using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phema.Validation;
using Phema.Validation.Conditions;

namespace DocumentIO
{
	public class UpdateAccountValidation : IDocumentIOValidation
	{
		private readonly DatabaseContext databaseContext;

		public UpdateAccountValidation(DatabaseContext databaseContext)
		{
			this.databaseContext = databaseContext;
		}

		public async Task Validate(DocumentIOResolveFieldContext<object> context, IValidationContext validationContext)
		{
			var model = context.GetArgument<Account>();

			if (model.Login != null)
			{
				validationContext.When(model, m => m.Login)
					.IsNullOrWhitespace()
					.AddValidationDetail("Логин не задан");

				if (validationContext.IsValid(model, m => m.Login))
				{
					var accountExists = await databaseContext
						.Accounts
						.AnyAsync(x => x.Login == model.Login);

					validationContext.When(model, m => m.Login)
						.Is(() => accountExists)
						.AddValidationDetail("Логин уже существует");
				}
			}

			if (model.Email != null)
			{
				validationContext.When(model, m => m.Email)
					.IsNotEmail()
					.AddValidationDetail("Это не email =/");

				if (validationContext.IsValid(model, m => m.Email))
				{
					var accountExists = await databaseContext
						.Accounts
						.AnyAsync(account => account.Email == model.Email);

					validationContext.When(model, m => m.Email)
						.Is(() => accountExists)
						.AddValidationDetail("Email уже используется");
				}

				if (model.Password != null)
				{
					validationContext.When(model, m => m.Password)
						.IsNullOrWhitespace()
						.AddValidationDetail("Пароль не задан");
				}

				if (model.FirstName != null)
				{
					validationContext.When(model, m => m.FirstName)
						.IsNullOrWhitespace()
						.AddValidationDetail("Имя не задано");
				}

				if (model.MiddleName != null)
				{
					validationContext.When(model, m => m.MiddleName)
						.IsNullOrWhitespace()
						.AddValidationDetail("Отчество не задано");
				}

				if (model.LastName != null)
				{
					validationContext.When(model, m => m.LastName)
						.IsNullOrWhitespace()
						.AddValidationDetail("Фамилия не задана");
				}
			}
		}
	}
}