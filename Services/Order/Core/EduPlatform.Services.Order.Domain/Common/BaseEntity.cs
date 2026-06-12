namespace EduPlatform.Services.Order.Domain.Common
{
	public abstract class BaseEntity
	{
		private int? _requestedHashCode;
		private int _Id;

		public virtual int Id
		{
			get => _Id;
			set => _Id = value;
		}

		public bool IsTransient()
		{
			return this.Id == default(Int32);
		}

		public override int GetHashCode()
		{
			if (!IsTransient())
			{
				if (!_requestedHashCode.HasValue)
					_requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

				return _requestedHashCode.Value;
			}
			else
				return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is BaseEntity))
				return false;

			if (Object.ReferenceEquals(this, obj))
				return true;

			if (this.GetType() != obj.GetType())
				return false;

			BaseEntity item = (BaseEntity)obj;

			if (item.IsTransient() || this.IsTransient())
				return false;
			else
				return item.Id == this.Id;
		}

		public static bool operator ==(BaseEntity left, BaseEntity right)
		{
			if (Object.Equals(left, null))
				return (Object.Equals(right, null)) ? true : false;
			else
				return left.Equals(right);
		}

		public static bool operator !=(BaseEntity left, BaseEntity right)
		{
			if (Object.Equals(left, null))
				return (Object.Equals(right, null)) ? true : false;
			else
				return left.Equals(right);
		}
	}
}
