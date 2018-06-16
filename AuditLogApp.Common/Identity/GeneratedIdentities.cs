
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

		public static CustomerId FromString(string rawValue)
		{
						return new CustomerId((int)Convert.ChangeType(rawValue, typeof(int)));
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

		public static UserId FromString(string rawValue)
		{
						return new UserId((int)Convert.ChangeType(rawValue, typeof(int)));
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


	public class CustomerAuthenticationId : IIdentity<Guid>
	{	
		[Obsolete("Serialization use only", true)]
		public CustomerAuthenticationId() { }

		public CustomerAuthenticationId(Guid id)
		{
			RawValue = id;
		}

		public Guid RawValue { get; set; }

		public static implicit operator CustomerAuthenticationId(Guid id)
		{
			return new CustomerAuthenticationId(id);
		}

		public static CustomerAuthenticationId FromString(string rawValue)
		{
						return Guid.Parse(rawValue);
					}

		public override bool Equals(object obj)
		{
			if(obj is CustomerAuthenticationId)
			{
				return RawValue.Equals(((CustomerAuthenticationId)obj).RawValue);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		public static bool operator ==(CustomerAuthenticationId a, CustomerAuthenticationId b)
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

		public static bool operator !=(CustomerAuthenticationId a, CustomerAuthenticationId b)
		{
			return !(a == b);
		}

		
        public override int GetHashCode()
        {
            return RawValue.GetHashCode();
        }

		public override string ToString()
		{
			return $"CustomerAuthenticationId[{RawValue}]";
		}
	}


	public class UserAuthenticationId : IIdentity<Guid>
	{	
		[Obsolete("Serialization use only", true)]
		public UserAuthenticationId() { }

		public UserAuthenticationId(Guid id)
		{
			RawValue = id;
		}

		public Guid RawValue { get; set; }

		public static implicit operator UserAuthenticationId(Guid id)
		{
			return new UserAuthenticationId(id);
		}

		public static UserAuthenticationId FromString(string rawValue)
		{
						return Guid.Parse(rawValue);
					}

		public override bool Equals(object obj)
		{
			if(obj is UserAuthenticationId)
			{
				return RawValue.Equals(((UserAuthenticationId)obj).RawValue);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		public static bool operator ==(UserAuthenticationId a, UserAuthenticationId b)
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

		public static bool operator !=(UserAuthenticationId a, UserAuthenticationId b)
		{
			return !(a == b);
		}

		
        public override int GetHashCode()
        {
            return RawValue.GetHashCode();
        }

		public override string ToString()
		{
			return $"UserAuthenticationId[{RawValue}]";
		}
	}


	public class UserSessionId : IIdentity<Guid>
	{	
		[Obsolete("Serialization use only", true)]
		public UserSessionId() { }

		public UserSessionId(Guid id)
		{
			RawValue = id;
		}

		public Guid RawValue { get; set; }

		public static implicit operator UserSessionId(Guid id)
		{
			return new UserSessionId(id);
		}

		public static UserSessionId FromString(string rawValue)
		{
						return Guid.Parse(rawValue);
					}

		public override bool Equals(object obj)
		{
			if(obj is UserSessionId)
			{
				return RawValue.Equals(((UserSessionId)obj).RawValue);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		public static bool operator ==(UserSessionId a, UserSessionId b)
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

		public static bool operator !=(UserSessionId a, UserSessionId b)
		{
			return !(a == b);
		}

		
        public override int GetHashCode()
        {
            return RawValue.GetHashCode();
        }

		public override string ToString()
		{
			return $"UserSessionId[{RawValue}]";
		}
	}


	public class EventEntryId : IIdentity<Guid>
	{	
		[Obsolete("Serialization use only", true)]
		public EventEntryId() { }

		public EventEntryId(Guid id)
		{
			RawValue = id;
		}

		public Guid RawValue { get; set; }

		public static implicit operator EventEntryId(Guid id)
		{
			return new EventEntryId(id);
		}

		public static EventEntryId FromString(string rawValue)
		{
						return Guid.Parse(rawValue);
					}

		public override bool Equals(object obj)
		{
			if(obj is EventEntryId)
			{
				return RawValue.Equals(((EventEntryId)obj).RawValue);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		public static bool operator ==(EventEntryId a, EventEntryId b)
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

		public static bool operator !=(EventEntryId a, EventEntryId b)
		{
			return !(a == b);
		}

		
        public override int GetHashCode()
        {
            return RawValue.GetHashCode();
        }

		public override string ToString()
		{
			return $"EventEntryId[{RawValue}]";
		}
	}


	public class EventActorId : IIdentity<Guid>
	{	
		[Obsolete("Serialization use only", true)]
		public EventActorId() { }

		public EventActorId(Guid id)
		{
			RawValue = id;
		}

		public Guid RawValue { get; set; }

		public static implicit operator EventActorId(Guid id)
		{
			return new EventActorId(id);
		}

		public static EventActorId FromString(string rawValue)
		{
						return Guid.Parse(rawValue);
					}

		public override bool Equals(object obj)
		{
			if(obj is EventActorId)
			{
				return RawValue.Equals(((EventActorId)obj).RawValue);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		public static bool operator ==(EventActorId a, EventActorId b)
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

		public static bool operator !=(EventActorId a, EventActorId b)
		{
			return !(a == b);
		}

		
        public override int GetHashCode()
        {
            return RawValue.GetHashCode();
        }

		public override string ToString()
		{
			return $"EventActorId[{RawValue}]";
		}
	}


	public class ViewId : IIdentity<Guid>
	{	
		[Obsolete("Serialization use only", true)]
		public ViewId() { }

		public ViewId(Guid id)
		{
			RawValue = id;
		}

		public Guid RawValue { get; set; }

		public static implicit operator ViewId(Guid id)
		{
			return new ViewId(id);
		}

		public static ViewId FromString(string rawValue)
		{
						return Guid.Parse(rawValue);
					}

		public override bool Equals(object obj)
		{
			if(obj is ViewId)
			{
				return RawValue.Equals(((ViewId)obj).RawValue);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		public static bool operator ==(ViewId a, ViewId b)
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

		public static bool operator !=(ViewId a, ViewId b)
		{
			return !(a == b);
		}

		
        public override int GetHashCode()
        {
            return RawValue.GetHashCode();
        }

		public override string ToString()
		{
			return $"ViewId[{RawValue}]";
		}
	}


}