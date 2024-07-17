﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.LoginAttemptCollaborator
{
    public class LoginAttemptCollaborator : ILoginAttemptCollaborator
    {
        public int CollaboratorId { get; set; }
        public bool IsSuccess { get; set; }
        public IPAddress IpAddress { get; set; }
        public string ApplicationName { get; set; }
    }
}
