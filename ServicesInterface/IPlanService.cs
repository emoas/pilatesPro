using Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
    public interface IPlanService
    {
        IEnumerable<PlanDTO> GetAll();
        PlanDTO Add(PlanDTO planDTO);
        PlanDTO Update(PlanDTO planDTO);
        void Remove(int planId);
        PlanDTO GetId(int planId);
        PlanDTO Activar(int planId,bool activar);
    }
}
