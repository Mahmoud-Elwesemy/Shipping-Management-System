using AutoMapper;
using ITI.Shipping.Core.Application.Abstraction.Merchant;
using ITI.Shipping.Core.Application.Abstraction.Merchant.Model;
using ITI.Shipping.Core.Application.Abstraction.SpecialCityCost.Model;
using ITI.Shipping.Core.Domin.Entities;
using ITI.Shipping.Core.Domin.Entities_Helper;
using ITI.Shipping.Core.Domin.UnitOfWork.Contract;
using Microsoft.AspNetCore.Identity;

namespace ITI.Shipping.Core.Application.Services.MerchantServices;
internal class MerchantService:IMerchantService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public MerchantService(IUnitOfWork unitOfWork , 
        IMapper mapper ,
        UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }
    //---------------------------------------------------------------------------
    private async Task UpdateSpecialCitiesAsync(string merchantId,List<SpecialCityCostToUpdateMerchantDTO>? specialCities)
    {
        if(specialCities == null)
            return;
        var currentCities = (await _unitOfWork.GetSpecialCityCostRepository().GetSpecialCityOfMerchant(merchantId)).ToList();

        var citiesToRemove = currentCities.Where(c => !specialCities.Any(sc => sc.CitySettingId == c.CitySettingId)).ToList();
        foreach(var city in citiesToRemove)
        {
            await _unitOfWork.GetSpecialCityCostRepository().DeleteAsync(city.Id);
        }
        foreach(var cityDto in specialCities)
        {
            var existingCity = currentCities.FirstOrDefault(c => c.CitySettingId == cityDto.CitySettingId);
            if(existingCity != null)
            {
                existingCity.Price = cityDto.Price;
                _unitOfWork.GetSpecialCityCostRepository().UpdateAsync(existingCity);
            }
            else
            {
                var newCity = new SpecialCityCost
                {
                    MerchantId = merchantId,
                    CitySettingId = cityDto.CitySettingId,
                    Price = cityDto.Price
                };
                await _unitOfWork.GetSpecialCityCostRepository().AddAsync(newCity);
            }
        }
    }
    //---------------------------------------------------------------------------
    public async Task<IEnumerable<MerchantDTO>> GetAllMerchantAsync()
    {
        var merchants = await _unitOfWork.GetMerchantRepository().GetAllMerchantAsync();
        var merchantDTOs = _mapper.Map<IEnumerable<MerchantDTO>>(merchants);

        foreach(var merchant in merchantDTOs)
        {
            merchant.SpecialCities = _mapper.Map<IEnumerable<SpecialCityCostDTO>>(
                await _unitOfWork.GetSpecialCityCostRepository().GetSpecialCityOfMerchant(merchant.Id)
            );
        }

        return merchantDTOs;
    }
    //---------------------------------------------------------------------------
    public async Task<MerchantDTO> GetMerchantByIdAsync(string id)
    {
        var merchant = await _userManager.FindByIdAsync(id);
        if(merchant == null || !(await _userManager.IsInRoleAsync(merchant,DefaultRole.Merchant)))
            throw new KeyNotFoundException($"Merchant with ID {id} not found.");

        var merchantDTO = _mapper.Map<MerchantDTO>(merchant);
        merchantDTO.SpecialCities = _mapper.Map<IEnumerable<SpecialCityCostDTO>>(
            await _unitOfWork.GetSpecialCityCostRepository().GetSpecialCityOfMerchant(id)
        );

        return merchantDTO;
    }
    //---------------------------------------------------------------------------
    public async Task UpdateMerchantAsync(UpdateMerchantDTO merchantUpdate)
    {
        var merchant = await _userManager.FindByIdAsync(merchantUpdate.Id);
        if(merchant == null || !(await _userManager.IsInRoleAsync(merchant,DefaultRole.Merchant)))
            throw new KeyNotFoundException($"Merchant with ID {merchantUpdate.Id} not found.");

        merchant.FullName = merchantUpdate.FullName;
        merchant.PhoneNumber = merchantUpdate.PhoneNumber;
        merchant.Address = merchantUpdate.Address;
        merchant.BranchId = merchantUpdate.BranchId;
        merchant.RegionId = merchantUpdate.RegionId;
        merchant.CityId = merchantUpdate.CityId;
        merchant.StoreName = merchantUpdate.StoreName;

       
        await UpdateSpecialCitiesAsync(merchantUpdate.Id,merchantUpdate.SpecialCities?.ToList());

        var result = await _userManager.UpdateAsync(merchant);
        if(!result.Succeeded)
            throw new Exception($"Failed to update merchant: {string.Join(", ",result.Errors.Select(e => e.Description))}");
    }
    //---------------------------------------------------------------------------
    public async Task DeleteMerchantAsync(string id)
    {
        var merchant = await _userManager.FindByIdAsync(id);
        if(merchant == null || !(await _userManager.IsInRoleAsync(merchant,DefaultRole.Merchant)))
            throw new KeyNotFoundException($"Merchant with ID {id} not found.");

        var specialCities = await _unitOfWork.GetSpecialCityCostRepository().GetSpecialCityOfMerchant(id);
        foreach(var city in specialCities)
        {
            await _unitOfWork.GetSpecialCityCostRepository().DeleteAsync(city.Id);
        }
        var result = await _userManager.DeleteAsync(merchant);
        if(!result.Succeeded)
            throw new Exception($"Failed to delete merchant: {string.Join(", ",result.Errors.Select(e => e.Description))}");
    }
    //---------------------------------------------------------------------------
}
