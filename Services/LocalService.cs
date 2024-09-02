using AutoMapper;
using DataAccessInterface.Repositories;
using Domain;
using Dto;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class LocalService : ILocalService
    {
        private IRepository<Local> localRepository;
        private IMapper mapper;
        public LocalService(IMapper mapper, IRepository<Local> localRepository)
        {
            this.localRepository = localRepository;
            this.mapper = mapper;

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
