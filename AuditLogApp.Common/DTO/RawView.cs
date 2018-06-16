using AuditLogApp.Common.Identity;
using System;

namespace AuditLogApp.Common.DTO
{
    // This file has to be in the Common project. If you put it in the 
    //  PersistenceSQLServer one where it started, PetaPoco throws an
    //  error requiring IConvertible to be implemented. Copy this file
    //  down there (don't even change the namespace) and you will see.
    //
    //  It's something to do with the Identities
    //
    //  No clue what's happening.

    public class RawView
    {
        public ViewId Id { get; set; }
        public CustomerId CustomerId { get; set; }
        public string AccessKey { get; set; }
        public string Content { get; set; }
    }
}
