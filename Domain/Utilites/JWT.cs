﻿

namespace Domain.Utilites
{
    public class JWT
    {
        public string Key  { get; set; }
        public string  Issuer { get; set; } 
        public string Audience { get; set; }
        public int Expiry { get; set; }
    }
}
