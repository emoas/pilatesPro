using Dto.DashBoard;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
    public interface IDashBoardService
    {
        DashBoardDTO GetHome();
    }
}
