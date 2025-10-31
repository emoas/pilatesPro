using AutoMapper;
using DataAccessInterface.Repositories;
using Domain;
using Dto;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LocalService : ILocalService
    {
        private IRepository<Local> localRepository;
        private IMapper mapper;
        private readonly HttpClient _httpClient;
        public LocalService(IMapper mapper, IRepository<Local> localRepository,HttpClient httpClient)
        {
            this.localRepository = localRepository;
            this.mapper = mapper;
            _httpClient = httpClient;

        }
        public LocalDTO Add(LocalDTO localDTO)
        {
            Local local = new Local(localDTO.Nombre, localDTO.Direccion, localDTO.Pais, localDTO.Ciudad, localDTO.Email, localDTO.Telefono, localDTO.Celular, localDTO.Url);
            this.localRepository.AddAndSave(local);
            return this.mapper.Map<LocalDTO>(local);
        }

        public IEnumerable<LocalDTO> GetAll()
        {
            var locales = this.localRepository.List();
            return this.mapper.Map<IEnumerable<LocalDTO>>(locales);
        }

        public LocalDTO GetId(int localId)
        {
            var local = this.localRepository.List().FirstOrDefault(l => l.Id == localId);
            return this.mapper.Map<LocalDTO>(local);
        }

        public void Remove(int localId)
        {
            var local = this.localRepository.List().FirstOrDefault(l => l.Id == localId);
            if (local == null)
            {
                throw new Exception("No existe el local seleccionado.");
            }
            local.Activo = false;
            this.localRepository.Update(local);
        }

        public async Task SendMessageAsync(WhatsAppMessage message)
        {
      
            // La URL de la API de WhatsApp Business
            var apiUrl = "https://graph.facebook.com/v22.0/163481746855115/messages"; // Reemplaza con tu phone number ID.
            var token = "EAAOPw1zzi7YBOxI4sx7zivXxOerVd63PdTJLZCKZBvZA2PZAoOJejzt6tSq05MZBMFVsZAyUVFcEuTvOrmCjY3K1PntLEVdLp5Lc6XhuUSsAlFHAchiAjBnCpmyZBcKm8GNqWbVpLdTRBz6WAyjkkOxuD4gaOzAGgE9hxcWuwaP3ihMFGIWI3mZCdmprNlJzdN6C8WPhWpUEdcdDfNmrOyePJ7Ex";  // Reemplaza con tu token de acceso

            // Crea el cuerpo del mensaje con el template
            var content = new
            {
                messaging_product = "whatsapp",
                to = message.PhoneNumber,
                type = "template",
                template = new
                {
                    name = message.TemplateName,
                    language = new { code = message.LanguageCode },
                    components = new[]
                    {
                        new
                        {
                            type = "body",
                            parameters = new[]
                            {
                                 new { type = "text", text = "1" },  // Primer valor a reemplazar en la plantilla
                                 new { type = "text", text = "link de acceso" }  // Segundo valor a reemplazar en la plantilla  // Personaliza el texto
                            }
                        }
                    }
                }
            };

            var requestBody = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(content),
            Encoding.UTF8,
            "application/json");

            // Agregar el token de autorización en el encabezado
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            // Realizar la solicitud POST
            var response = await _httpClient.PostAsync(apiUrl, requestBody);
        }

        public LocalDTO Update(LocalDTO localDTOUpdate)
        {
            var localToUpdate = this.localRepository.List().FirstOrDefault(l => l.Id == localDTOUpdate.Id);
            localToUpdate.Nombre = localDTOUpdate.Nombre;
            localToUpdate.Direccion = localDTOUpdate.Direccion;
            localToUpdate.Pais = localDTOUpdate.Pais;
            localToUpdate.Ciudad = localDTOUpdate.Ciudad;
            localToUpdate.Email = localDTOUpdate.Email;
            localToUpdate.Telefono = localDTOUpdate.Telefono;
            localToUpdate.Celular = localDTOUpdate.Celular;
            localToUpdate.Url = localDTOUpdate.Url;
            this.localRepository.Update(localToUpdate);
            return this.mapper.Map<LocalDTO>(localToUpdate);
        }
    }
}
