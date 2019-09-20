namespace DocumentIO
{
	public class InviteFilterType : FilterType<Invite, InviteFilter>
	{
		public InviteFilterType()
		{
			Field(x => x.Id, nullable: true);
			Field(x => x.Role, nullable: true);
			Field(x => x.Description, nullable: true);
		}
	}
}