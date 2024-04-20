using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dao.DTOs;

namespace dao.Persistence.DAO
{
    public interface IConsumAiguaDAO
    {
        ConsumAiguaDTO GetConsumAiguaDTOById(int id);
        public IEnumerable<ConsumAiguaDTO> GetAllConsumAiguaDTO();
        void AddConsumAiguaDTO(ConsumAiguaDTO consumAigua);
        void UpdateConsumAiguaDTO(ConsumAiguaDTO consumAigua);
        void DeleteConsumAiguaDTO(int id);
    }
}
