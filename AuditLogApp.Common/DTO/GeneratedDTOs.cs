
using System;
using System.Collections.Generic;
using AuditLogApp.Common.Enums;
using AuditLogApp.Common.Identity;

namespace AuditLogApp.Common.DTO
{


	public partial class CustomerDTO
	{	
		[Obsolete("Serialization use only", true)]
		public CustomerDTO() { }

		public CustomerDTO(CustomerId id, string displayname)
		{
		
			Id = id;
			DisplayName = displayname;
		}

		
		public CustomerId Id { get; set; }
			
		public string DisplayName { get; set; }
			
		
	}


	public partial class UserDTO
	{	
		[Obsolete("Serialization use only", true)]
		public UserDTO() { }

		public UserDTO(UserId id, CustomerId customerid, string displayname, string username, string emailaddress, bool isenabled = true)
		{
		
			Id = id;
			CustomerId = customerid;
			DisplayName = displayname;
			Username = username;
			EmailAddress = emailaddress;
			IsEnabled = isenabled;
		}

		
		public UserId Id { get; set; }
			
		public CustomerId CustomerId { get; set; }
			
		public string DisplayName { get; set; }
			
		public string Username { get; set; }
			
		public string EmailAddress { get; set; }
			
		public bool IsEnabled { get; set; }
			
		
	}


	public partial class SecretUserDTO
	{	
		[Obsolete("Serialization use only", true)]
		public SecretUserDTO() { }

		public SecretUserDTO(UserId id, CustomerId customerid, string displayname, string username, string emailaddress, bool isenabled = true, Guid? passwordresetkey = null, DateTime? passwordresetgooduntil = null)
		{
		
			Id = id;
			CustomerId = customerid;
			DisplayName = displayname;
			Username = username;
			EmailAddress = emailaddress;
			IsEnabled = isenabled;
			PasswordResetKey = passwordresetkey;
			PasswordResetGoodUntil = passwordresetgooduntil;
		}

		
		public UserId Id { get; set; }
			
		public CustomerId CustomerId { get; set; }
			
		public string DisplayName { get; set; }
			
		public string Username { get; set; }
			
		public string EmailAddress { get; set; }
			
		public bool IsEnabled { get; set; }
			
		public Guid? PasswordResetKey { get; set; }
			
		public DateTime? PasswordResetGoodUntil { get; set; }
			
		
	}


	public partial class UserAuthenticationDTO
	{	
		[Obsolete("Serialization use only", true)]
		public UserAuthenticationDTO() { }

		public UserAuthenticationDTO(UserAuthenticationId id, UserId userid, CredentialType credentialtype, string secret, string displayname, DateTime creationtime, bool isrevoked = false, DateTime? revoketime = null)
		{
		
			Id = id;
			UserId = userid;
			CredentialType = credentialtype;
			Secret = secret;
			DisplayName = displayname;
			CreationTime = creationtime;
			IsRevoked = isrevoked;
			RevokeTime = revoketime;
		}

		
		public UserAuthenticationId Id { get; set; }
			
		public UserId UserId { get; set; }
			
		public CredentialType CredentialType { get; set; }
			
		public string Secret { get; set; }
			
		public string DisplayName { get; set; }
			
		public DateTime CreationTime { get; set; }
			
		public bool IsRevoked { get; set; }
			
		public DateTime? RevokeTime { get; set; }
			
		
	}


	public partial class UserSessionDTO
	{	
		[Obsolete("Serialization use only", true)]
		public UserSessionDTO() { }

		public UserSessionDTO(UserSessionId id, UserId userid, DateTime creationtime, DateTime? logouttime)
		{
		
			Id = id;
			UserId = userid;
			CreationTime = creationtime;
			LogoutTime = logouttime;
		}

		
		public UserSessionId Id { get; set; }
			
		public UserId UserId { get; set; }
			
		public DateTime CreationTime { get; set; }
			
		public DateTime? LogoutTime { get; set; }
			
		
	}


}