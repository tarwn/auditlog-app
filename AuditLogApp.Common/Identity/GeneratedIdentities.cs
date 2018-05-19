
using System;
namespace AuditLogApp.Common.Identity
{


	public class CustomerId : IIdentity<int>
	{	
		[Obsolete("Serialization use only", true)]
		public CustomerId() { }

		public CustomerId(int id)
		{
			RawValue = id;
		}

		public int RawValue { get; set; }

		public static implicit operator CustomerId(int id)
		{
			return new CustomerId(id);
		}


		public override bool Equals(object obj)
		{
			if(obj is CustomerId)
			{
				return RawValue.Equals(((CustomerId)obj).RawValue);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		public static bool operator ==(CustomerId a, CustomerId b)
		{
			if(System.Object.ReferenceEquals(a,b))
			{
				return true;
			}

			if(((object)a == null && (object)b != null) || ((object)a != null && (object)b == null))
			{
				return false;
			}

			return a.RawValue == b.RawValue;
		}

		public static bool operator !=(CustomerId a, CustomerId b)
		{
			return !(a == b);
		}

		
        public override int GetHashCode()
        {
            return RawValue.GetHashCode();
        }

		public override string ToString()
		{
			return $"CustomerId[{RawValue}]";
		}
	}


	public class UserId : IIdentity<int>
	{	
		[Obsolete("Serialization use only", true)]
		public UserId() { }

		public UserId(int id)
		{
			RawValue = id;
		}

		public int RawValue { get; set; }

		public static implicit operator UserId(int id)
		{
			return new UserId(id);
		}


		public override bool Equals(object obj)
		{
			if(obj is UserId)
			{
				return RawValue.Equals(((UserId)obj).RawValue);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		public static bool operator ==(UserId a, UserId b)
		{
			if(System.Object.ReferenceEquals(a,b))
			{
				return true;
			}

			if(((object)a == null && (object)b != null) || ((object)a != null && (object)b == null))
			{
				return false;
			}

			return a.RawValue == b.RawValue;
		}

		public static bool operator !=(UserId a, UserId b)
		{
			return !(a == b);
		}

		
        public override int GetHashCode()
        {
            return RawValue.GetHashCode();
        }

		public override string ToString()
		{
			return $"UserId[{RawValue}]";
		}
	}


}