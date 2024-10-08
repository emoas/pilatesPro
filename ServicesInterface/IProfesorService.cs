﻿using Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
    public interface IProfesorService
    {
        IEnumerable<ProfesorDTO> GetAll();
        ProfesorDTO Add(ProfesorDTO personalDTO);
        ProfesorDTO Update(ProfesorDTO personalDTOUpdate);
        void Remove(int personalId);
        ProfesorDTO GetId(int profeId);
    }
}
