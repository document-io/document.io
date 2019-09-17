using System.Linq;
using GraphQL.Authorization;
using GraphQL.Types;
using GraphQL.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace DocumentIO
{
	public static class InviteMutations
	{
		public static void AddInviteMutations(this DocumentIOMutations mutationses)
		{
			mutationses.Field<ReadInviteGraphType, ReadInviteModel>()
				.Name("createInvite")
				.AuthorizeWith(Roles.Admin)
				.Argument<NonNullGraphType<CreateInviteGraphType>>("payload")
				.ResolveAsync(async context =>
				{
					var accountId = context.GetAccountId();
					var databaseContext = context.GetDatabaseContext();
					var model = context.GetArgument<CreateInviteModel>("payload");

					var account = await databaseContext.Accounts
						.Include(x => x.Organization)
						.SingleAsync(x => x.Id == accountId);

					var invite = await model.Create(databaseContext, account, account.Organization);

					await databaseContext.SaveChangesAsync();

					return new ReadInviteModel
					{
						Id = invite.Id,
						Email = invite.Email,
						Description = invite.Description,
						Identifier = invite.Identifier,
						Role = invite.Role,
						CreatedAt = invite.CreatedAt,
						DueDate = invite.DueDate
					};
				});

			mutationses.Field<DeleteInviteGraphType, DeleteInviteModel>()
				.Name("deleteInvite")
				.Argument<IntGraphType>("id")
				.AuthorizeWith(Roles.Admin)
				.ResolveAsync(async context =>
				{
					var databaseContext = context.GetDatabaseContext();
					var inviteId = context.GetArgument<int>("id");

					var invite = await databaseContext.Invites.SingleAsync(x => x.Id == inviteId);

					databaseContext.Invites.Remove(invite);

					await databaseContext.SaveChangesAsync();

					return new DeleteInviteModel
					{
						Id = invite.Id
					};
				});
		}
	}
}