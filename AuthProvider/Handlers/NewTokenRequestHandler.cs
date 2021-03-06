﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthProvider.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AuthProvider.Crypto;
using System.Text;
using System.Web;

namespace AuthProvider.Handlers
{
    public class NewTokenRequestHandler
    {
        private readonly string Username;
        private readonly string Password;
        private string Group;
        public NewTokenRequestHandler(Models.Requests.NewToken request)
        {
            Username = request.Username;
            Password = request.Password;
        }
        public bool VerifyUser()
        {
            //TODO: LDAP auth
            if(Username == "kablaa" && Password == "password")
            {
                Group = "admin";
                return true;
            }
            else
                return false;
        }
        public string GenerateJWT()
        {

            JWTHeader Header = new JWTHeader();
            JWTBody Payload = new JWTBody
            {
                Username = this.Username,
                Group = this.Group,
                Iat = DateTime.Now

            };

            string EncodedHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Header)));
            string EncodedPayload = Convert.ToBase64String(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Payload)));
            string Message = EncodedHeader + "." + EncodedPayload;
            //TODO: Put the file path in a config file somewhere
            try
            {
                
                return Message + "." + RS256.Sign(Encoding.ASCII.GetBytes(Message));
            }catch
            {
                return "fail!";

            }
            
        }
    }
}
