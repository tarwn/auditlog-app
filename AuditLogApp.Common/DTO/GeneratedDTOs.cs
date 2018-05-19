
using System;
using System.Collections.Generic;
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

		public UserDTO(UserId id, CustomerId customerid, string displayname)
		{
		
			Id = id;
			CustomerId = customerid;
			DisplayName = displayname;
		}

		
		public UserId Id { get; set; }
			
		public CustomerId CustomerId { get; set; }
			
		public string DisplayName { get; set; }
			
		
	}


}