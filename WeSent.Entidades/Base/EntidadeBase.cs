using System;

namespace WeSent.Entidades
{
	public class EntidadeBase<TPrimaryKey>
	{
		/// <summary>
		/// Unique identifier for this entity.
		/// </summary>
		public virtual TPrimaryKey Codigo { get; set; }
	}
}

