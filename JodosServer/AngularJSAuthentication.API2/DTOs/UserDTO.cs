using JodosServer.Entities;
using JodosServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JodosServer.DTOs
{
    public class UserDTO
    {
        public string createDate { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public List<string> Mataks { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public UserDTO() { }

        public UserDTO(User user)
        {
            this.createDate = user.CreateDate.ToString("dd/MM/yyyy");
            this.UserName = user.UserName;
            this.Mataks = user.Mataks;
            this.Role = user.Roles[0];
            this.lastName = user.LastName;
            this.firstName = user.FirstName;
        }

        public UserDTO(UserModel user)
        {
            this.createDate = DateTime.Now.ToString("dd/MM/yyyy");
            this.UserName = user.UserName;
            this.Mataks = new List<string>();
            this.lastName = user.lastName;
            this.firstName = user.firstName;
        }
    }
}