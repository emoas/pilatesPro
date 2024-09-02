using AutoMapper;
using DataAccessInterface.Repositories;
using Domain;
using Dto;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class PatologiaService : IPatologiaService
    {
        private IRepository<Patologia> patologiaRepository;
        private IMapper mapper;
        public PatologiaService(IMapper mapper, IRepository<Patologia> patologiaRepository)
        {
            this.patologiaRepository = patologiaRepository;
            this.mapper = mapper;
        }
        public PatologiaDTO Add(PatologiaDTO patologiaDTO)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PatologiaDTO> GetAll()
        {
            var patologias = this.patologiaRepository.List();
            return this.mapper.Map<IEnumerable<PatologiaDTO>>(patologias);
        }

        public PatologiaDTO GetId(int patologiaId)
        {
            throw new NotImplementedException();
        }

        public void Remove(int patologiaId)
        {
            throw new NotImplementedException();
        }

        public PatologiaDTO Update(PatologiaDTO patologiaDTO)
        {
            throw new NotImplementedException();
        }
    }
}
