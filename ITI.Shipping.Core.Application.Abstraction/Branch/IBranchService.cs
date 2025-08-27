using ITI.Shipping.Core.Application.Abstraction.Branch.Models;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ITI.Shipping.Core.Application.Abstraction.Branch
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchDTO>> GetBranchesAsync ( Pramter pramter);
        Task<IEnumerable<BranchDTO>> GetBranchesByRegionIdAsync(int regionId);
        Task<IEnumerable<BranchDTO>> GetBranchesByCitySettingIdAsync(int citySettingId);
        Task<BranchDTO> GetBranchAsync(int id);
        Task AddAsync(BranchToAddDTO DTO);
        Task UpdateAsync(BranchToUpdateDTO DTO);
        Task DeleteAsync(int id);
    }
}
