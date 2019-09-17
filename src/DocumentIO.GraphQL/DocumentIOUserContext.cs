using System;
using GraphQL.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentIO
{
	public class DocumentIOUserContext : Dictionary<string, object>, IProvideClaimsPrincipal
	{
		public DocumentIOUserContext(HttpContext httpContext)
		{
			HttpContext = httpContext;
		}

		public HttpContext HttpContext { get; }

		public ClaimsPrincipal User => HttpContext.User;

		public int AccountId => int.Parse(User.Identity.Name);

		public IServiceProvider ServiceProvider => HttpContext.RequestServices;

		public DatabaseContext DatabaseContext => ServiceProvider.GetRequiredService<DatabaseContext>();
	}
}