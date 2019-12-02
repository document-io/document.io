using System;
using System.Collections.Generic;

namespace DocumentIO
{
	public class CardAttachment
	{
		public Guid Id { get; set; }
		public DateTimeOffset CreatedAt { get; set; }

		public Guid CardId { get; set; }
		public Card Card { get; set; }

		public Guid AccountId { get; set; }
		public Account Account { get; set; }

		public ICollection<File> Files { get; set; }
	}
}