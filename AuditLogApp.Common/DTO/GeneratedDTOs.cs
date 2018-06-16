
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


	public partial class CustomerAuthenticationDTO
	{	
		[Obsolete("Serialization use only", true)]
		public CustomerAuthenticationDTO() { }

		public CustomerAuthenticationDTO(CustomerAuthenticationId id, CustomerId customerid, CredentialType credentialtype, string secret, string displayname, DateTime creationtime, UserId createdby, bool isrevoked = false, DateTime? revoketime = null)
		{
		
			Id = id;
			CustomerId = customerid;
			CredentialType = credentialtype;
			Secret = secret;
			DisplayName = displayname;
			CreationTime = creationtime;
			CreatedBy = createdby;
			IsRevoked = isrevoked;
			RevokeTime = revoketime;
		}

		
		public CustomerAuthenticationId Id { get; set; }
			
		public CustomerId CustomerId { get; set; }
			
		public CredentialType CredentialType { get; set; }
			
		public string Secret { get; set; }
			
		public string DisplayName { get; set; }
			
		public DateTime CreationTime { get; set; }
			
		public UserId CreatedBy { get; set; }
			
		public bool IsRevoked { get; set; }
			
		public DateTime? RevokeTime { get; set; }
			
		
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


	public partial class EventEntryDTO
	{	
		[Obsolete("Serialization use only", true)]
		public EventEntryDTO() { }

		public EventEntryDTO(EventEntryId id, CustomerId customerid, DateTime receptiontime, string uuid, string client_id, string client_name, DateTime eventtime, string action, string description, string url, EventActorId actor_id, string actor_uuid, string actor_name, string actor_email, string context_client_ip, string context_client_browseragent, string context_server_serverid, string context_server_version, string target_type, string target_uuid, string target_label, string target_url)
		{
		
			Id = id;
			CustomerId = customerid;
			ReceptionTime = receptiontime;
			UUID = uuid;
			Client_Id = client_id;
			Client_Name = client_name;
			EventTime = eventtime;
			Action = action;
			Description = description;
			URL = url;
			Actor_Id = actor_id;
			Actor_UUID = actor_uuid;
			Actor_Name = actor_name;
			Actor_Email = actor_email;
			Context_Client_IP = context_client_ip;
			Context_Client_BrowserAgent = context_client_browseragent;
			Context_Server_ServerId = context_server_serverid;
			Context_Server_Version = context_server_version;
			Target_Type = target_type;
			Target_UUID = target_uuid;
			Target_Label = target_label;
			Target_URL = target_url;
		}

		
		public EventEntryId Id { get; set; }
			
		public CustomerId CustomerId { get; set; }
			
		public DateTime ReceptionTime { get; set; }
			
		public string UUID { get; set; }
			
		public string Client_Id { get; set; }
			
		public string Client_Name { get; set; }
			
		public DateTime EventTime { get; set; }
			
		public string Action { get; set; }
			
		public string Description { get; set; }
			
		public string URL { get; set; }
			
		public EventActorId Actor_Id { get; set; }
			
		public string Actor_UUID { get; set; }
			
		public string Actor_Name { get; set; }
			
		public string Actor_Email { get; set; }
			
		public string Context_Client_IP { get; set; }
			
		public string Context_Client_BrowserAgent { get; set; }
			
		public string Context_Server_ServerId { get; set; }
			
		public string Context_Server_Version { get; set; }
			
		public string Target_Type { get; set; }
			
		public string Target_UUID { get; set; }
			
		public string Target_Label { get; set; }
			
		public string Target_URL { get; set; }
			
		
	}


	public partial class EventActorDTO
	{	
		[Obsolete("Serialization use only", true)]
		public EventActorDTO() { }

		public EventActorDTO(EventActorId id, string uuid, string name, string email, bool isforgotten)
		{
		
			Id = id;
			UUID = uuid;
			Name = name;
			Email = email;
			IsForgotten = isforgotten;
		}

		
		public EventActorId Id { get; set; }
			
		public string UUID { get; set; }
			
		public string Name { get; set; }
			
		public string Email { get; set; }
			
		public bool IsForgotten { get; set; }
			
		
	}


	public partial class ViewDTO
	{	
		[Obsolete("Serialization use only", true)]
		public ViewDTO() { }

		public ViewDTO(ViewId id, CustomerId customerid, string accesskey, ViewCustomizationDTO customization, List<ViewColumnDTO> columns)
		{
		
			Id = id;
			CustomerId = customerid;
			AccessKey = accesskey;
			Customization = customization;
			Columns = columns;
		}

		
		public ViewId Id { get; set; }
			
		public CustomerId CustomerId { get; set; }
			
		public string AccessKey { get; set; }
			
		public ViewCustomizationDTO Customization { get; set; }
			
		public List<ViewColumnDTO> Columns { get; set; }
			
		
	}


	public partial class ViewCustomizationDTO
	{	
		[Obsolete("Serialization use only", true)]
		public ViewCustomizationDTO() { }

		public ViewCustomizationDTO(string url, string logo, string title, List<ViewCustomizationHeaderLinkDTO> headerlinks, string copyright)
		{
		
			URL = url;
			Logo = logo;
			Title = title;
			HeaderLinks = headerlinks;
			Copyright = copyright;
		}

		
		public string URL { get; set; }
			
		public string Logo { get; set; }
			
		public string Title { get; set; }
			
		public List<ViewCustomizationHeaderLinkDTO> HeaderLinks { get; set; }
			
		public string Copyright { get; set; }
			
		
	}


	public partial class ViewCustomizationHeaderLinkDTO
	{	
		[Obsolete("Serialization use only", true)]
		public ViewCustomizationHeaderLinkDTO() { }

		public ViewCustomizationHeaderLinkDTO(string label, string url)
		{
		
			Label = label;
			URL = url;
		}

		
		public string Label { get; set; }
			
		public string URL { get; set; }
			
		
	}


	public partial class ViewColumnDTO
	{	
		[Obsolete("Serialization use only", true)]
		public ViewColumnDTO() { }

		public ViewColumnDTO(int order, string label, List<ViewColumnLineDTO> lines)
		{
		
			Order = order;
			Label = label;
			Lines = lines;
		}

		
		public int Order { get; set; }
			
		public string Label { get; set; }
			
		public List<ViewColumnLineDTO> Lines { get; set; }
			
		
	}


	public partial class ViewColumnLineDTO
	{	
		[Obsolete("Serialization use only", true)]
		public ViewColumnLineDTO() { }

		public ViewColumnLineDTO(string field)
		{
		
			Field = field;
		}

		
		public string Field { get; set; }
			
		
	}


}