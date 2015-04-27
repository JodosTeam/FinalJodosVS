using JodosServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JodosServer.DTOs
{
    public class UsersAndRolesDTO
    {
        public List<UserDTO> users { get; set; }
        public List<Role> roles { get; set; }

        public UsersAndRolesDTO(List<UserDTO> users, List<Role> roles)
        {
            this.users = users;
            this.roles = roles;
        }
    }
}