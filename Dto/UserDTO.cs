using System;
using System.Collections.Generic;
using System.Text;

namespace Dto
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public enum rol { GOD, ADMIN, PROFE, RECEPCION, ALUMNO }
        public rol Rol { get; set; }
        public Guid Token { get; set; }

        public UserDTO()
        {
        }

        public override bool Equals(object obj)
        {
            var result = false;

            if (obj is UserDTO user)
            {
                result = this.Id == user.Id && this.Name.Equals(user.Name) && this.Email.Equals(user.Email)
                    && this.Password.Equals(user.Password);
            }

            return result;
        }
    }
}
