﻿using Microsoft.AspNetCore.Identity;

namespace API.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Ip { get; set; } = "";

        public Carrello Carrello { get; set; }
    }
}
