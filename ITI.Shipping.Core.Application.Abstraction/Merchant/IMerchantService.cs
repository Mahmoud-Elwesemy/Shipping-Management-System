using ITI.Shipping.Core.Application.Abstraction.Merchant.Model;

namespace ITI.Shipping.Core.Application.Abstraction.Merchant;
public interface IMerchantService
{
    Task<IEnumerable<MerchantDTO>> GetAllMerchantAsync();
    Task<MerchantDTO> GetMerchantByIdAsync(string id);
    Task UpdateMerchantAsync(UpdateMerchantDTO merchantUpdate);
    Task DeleteMerchantAsync(string id);
}
