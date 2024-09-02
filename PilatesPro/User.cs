using System;
using System.Text.RegularExpressions;

namespace Domain
{
    public class User
    {
        public int Id { get; set; }
        public enum rol { GOD, ADMIN, PROFE, RECEPCION, ALUMNO }
        public rol Rol { get; set; }
        public bool ChangePassword { get; set; }
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.ValidateEmptyString(value, "El nombre del usuario no puede ser vacio o nulo.");
                this.name = value;
            }
        }
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.ValidateEmptyString(value, "El password del usuario no puede ser vacio o nulo.");
                this.password = value;
            }
        }
        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.ValidateEmptyString(value, "El email del usuario no puede ser vacio o nulo.");
                this.ValidateEmailFormat(value);
                this.email = value;
            }
        }
        public Guid Token { get; set; }
        private string name;
        private string password;
        private string email;
        private const string EMAIL_REGEX = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$";
        public User()
        {
            this.ChangePassword = true;
        }
        public User(string name, string email, string password, rol rol, bool ChangePassword)
        {
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.Token = Guid.NewGuid();
            this.Rol = rol;
            this.ChangePassword = ChangePassword;
        }
        private void ValidateEmptyString(string value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(message);
            }
        }
        private void ValidateEmailFormat(string value)
        {
            Regex regex = new Regex(EMAIL_REGEX);

            if (!regex.Match(value).Success)
            {
                throw new ArgumentException("El mail del usuario tiene que ser del formato 'mail@mail.com'.");
            }
        }
    }
}
