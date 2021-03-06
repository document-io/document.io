namespace DocumentIO
{
	public class ReadCommentType : DocumentIOGraphType<CardComment>
	{
		public ReadCommentType()
		{
			Field(x => x.Id);
			Field(x => x.Text);
			Field(x => x.CreatedAt);
			NullField(x => x.UpdatedAt);

			NonNullDocumentIOField<ReadCardType, Card>("card")
				.AllowUser()
				.ResolveAsync<CommentCardResolver>();

			NonNullDocumentIOField<ReadAccountType, Account>("account")
				.AllowUser()
				.ResolveAsync<CommentAccountResolver>();
		}
	}
}