using AutoMapper;
using ITI.Shipping.Core.Application.Abstraction.Product;
using ITI.Shipping.Core.Application.Abstraction.Product.Model;
using ITI.Shipping.Core.Domin.Entities;
using ITI.Shipping.Core.Domin.Pramter_Helper;
using ITI.Shipping.Core.Domin.UnitOfWork.Contract;

namespace ITI.Shipping.Core.Application.Services.ProductServices
{
    internal class ProductService:IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // Get All Products
        public async Task<IEnumerable<ProductDTO>> GetProductsAsync(Pramter pramter)
        {
            return _mapper.Map<IEnumerable<ProductDTO>>(await _unitOfWork.GetProductRepository().GetAllAsync(pramter));
        }
        // Get Products By Order Id
        public async Task<IEnumerable<ProductDTO>> GetProductsByOrderIdAsync(int orderId)
        {
            var products = await _unitOfWork.GetProductRepository().GetProductsWithOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }
        // Get Product By Id
        public async Task<ProductDTO> GetProductAsync(int id)
        {
            return _mapper.Map<ProductDTO>(await _unitOfWork.GetProductRepository().GetByIdAsync(id));
        }
        // Add Product  
        public async Task AddAsync(ProductDTO DTO)
        {
            await _unitOfWork.GetProductRepository().AddAsync(_mapper.Map<Product>(DTO));
        }
        // Update Product
        public async Task UpdateAsync(UpdateProductDTO DTO)
        {
            var ProductRepo = _unitOfWork.GetProductRepository();

            var existingProduct = await ProductRepo.GetByIdAsync(DTO.Id);
            if(existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {DTO.Id} not found.");

            _mapper.Map(DTO,existingProduct); // Update existing entity with DTO data

            ProductRepo.UpdateAsync(existingProduct);
            await _unitOfWork.CompleteAsync();
        }
        // Delete Product
        public async Task DeleteAsync(int id)
        {
            var ProductRepo = _unitOfWork.GetProductRepository();

            var existingProduct = await ProductRepo.GetByIdAsync(id);
            if(existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            await ProductRepo.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

       
    }
}
