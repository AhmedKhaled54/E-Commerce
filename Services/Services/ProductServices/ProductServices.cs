using AutoMapper;
using Core.Data;
using Core.Entity;
using Infrastructure.Interfaces;
using Infrastructure.Specifications.ProductSpecifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Services.Dtos.ProductsDto;
using Services.Dtos.Response;
using Services.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.ProductServices
{
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork _unitOf;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHost;
        private readonly AppDBContext context;
        private new List<string> allowextention = new List<string>() { ".jpg", ".png", "jpeg" };
        private long maxsizeimage = 109951163;//2MB


        public ProductServices(IUnitOfWork unitOf,IMapper mapper,IWebHostEnvironment webHost,AppDBContext context)
        {
            _unitOf = unitOf;
            this.mapper = mapper;
            this.webHost = webHost;
            this.context = context;
        }

     
        public async Task<IEnumerable<ProductDto>> GetAllProduct(ProductSpecification specification)
        {
            var specs =new ProductWithCatgeoryAndBrandAndTypeSpecs(specification);
            var products = await _unitOf.Repository<Product>().GetAllEntityWithSpecs(specs);
            var ProductMap=mapper.Map<List<ProductDto>>(products);
            return ProductMap;
        }

        public async Task<Pagination<ProductDto>> GetAllProductwithpagination(ProductSpecification specification)
        {
            var specs = new ProductWithCatgeoryAndBrandAndTypeSpecs(specification);
            var products = await _unitOf.Repository<Product>().GetAllEntityWithSpecs(specs);
            var count =await  _unitOf.Repository<Product>().GetCount(specs);
            var productMap =mapper.Map<List<ProductDto>>(products);
            return new Pagination<ProductDto>
                (specification.PageSize, specification.PageIndex, count, productMap);

        }

        public async Task<ResponseDto>GetProductById(int? id)
        {
            var specs = new ProductWithCatgeoryAndBrandAndTypeSpecs(id);
            var product=await _unitOf.Repository<Product>().GetEntityWithSpecs(specs);
            var ProductMap=mapper.Map<ProductDetailsDto>(product);
            if (product == null)
            {
                return new ResponseDto
                {
                    Status = 400,
                    Message = "Product Not Found !"
                };
            }
            return new ResponseDto
            {
                Status = 200,
                IsSucceeded = true,
                Model = ProductMap
            };

        }
        public async Task<ResponseDto> CreateProduct(CreateProductDto dto)
        {
            var response = new ResponseDto();
            if (!allowextention.Contains(Path.GetExtension(dto.Image.FileName.ToLower())))
            {
                response.Status = 400;
                response.Message = "Must Be Image Allawed (.png) / (.jpg) / (.jpeg) !";
                return response;
            }
            if (dto.Image.Length > maxsizeimage)
            {
                response.Status = 400;
                response.Message = "Must be Allawed Image Size 2MB";
                return response;
            }
            if (await _unitOf.Repository<Product>().FindAsync(c => c.Name == dto.ProductName) != null)
            {
                response.Status = 400;
                response.Message = "Product Already Exsit!";
                return response;
            }
            if (!await _unitOf.Repository<Category>().IsValid(dto.CategoryId))
            {
                response.Status = 400;
                response.Message = "Category Not Found!";
                return response;
            }
            if (!await _unitOf.Repository<ProductType>().IsValid(dto.productType_Id))
            {
                response.Status = 400;
                response.Message = "Type Not Found!";
                return response;
            }
            if (!await _unitOf.Repository<ProductBrand>().IsValid(dto.Productbrand_Id))
            {
                response.Status = 400;
                response.Message = "Brand Not Found!";
                return response;
            }


            var uploadfile = Path.Combine(webHost.WebRootPath, "Images/Products");
            var uniquefile = Guid.NewGuid().ToString() + "_" + dto.Image.FileName;
            var PathFile = Path.Combine(uploadfile, uniquefile);
            using var stream = new FileStream(PathFile, FileMode.Create);
            var product = mapper.Map<Product>(dto);
            await dto.Image.CopyToAsync(stream);
            stream.Close();
            product.Image = "Images/Products/" + uniquefile.ToString();
            await _unitOf.Repository<Product>().AddAsync(product);
            await _unitOf.Complete();
           
            response.Status = 200;
            response.IsSucceeded = true;
            response.Model = product;
            response.Message = "Product Created Succesfuly ";
            return response;
        }

       
        public async Task<ResponseDto> UpdateProduct(int id, UpdateProductDto dto)
        {
            var response=new ResponseDto();
            var product=await _unitOf.Repository<Product>().FindAsync(c=>c.Id==id);
          
            if (product == null)
            {
                response.Status = 400;
                response.Message = " preoduct Not Found!";
                return response;
            }
            if (dto.Image != null)
            {
                if (!allowextention.Contains(Path.GetExtension(dto.Image.FileName.ToLower())))
                {
                    response.Status = 400;
                    response.Message = "Must Be Image Allawed (.png) / (.jpg) / (.jpeg) !";
                    return response;
                }
                if (dto.Image.Length > maxsizeimage)
                {
                    response.Status = 400;
                    response.Message = "Must be Allawed Image Size 2MB";
                    return response;
                }
                var pathfile = Path.Combine(webHost.WebRootPath, "Images/Products");
                var uniquefile= Guid.NewGuid().ToString()+"_" +dto.Image.FileName;
                var File= Path.Combine(pathfile, uniquefile);
                using var stream =new FileStream(File, FileMode.Create);
                await dto.Image.CopyToAsync(stream);
                stream.Close();
                product.Image = "Images/Products/" + uniquefile.ToString();
            }
            if (await _unitOf.Repository<Product>().FindAsync(c => c.Name == dto.ProductName) != null)
            {
                response.Status = 400;
                response.Message = "Product Already Exsit!";
                return response;
            }
            if (!await _unitOf.Repository<Category>().IsValid(dto.CategoryId))
            {
                response.Status = 400;
                response.Message = "Category Not Found!";
                return response;
            }
            if (!await _unitOf.Repository<ProductType>().IsValid(dto.productType_Id))
            {
                response.Status = 400;
                response.Message = "Type Not Found!";
                return response;
            }
            if (!await _unitOf.Repository<ProductBrand>().IsValid(dto.Productbrand_Id))
            {
                response.Status = 400;
                response.Message = "Brand Not Found!";
                return response;
            }

            product.Name = dto.ProductName;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.CategoryId= dto.CategoryId;
            product.Productbrand_Id = dto.Productbrand_Id;
            product.productType_Id=dto.productType_Id;

            _unitOf.Repository<Product>().Update(product);
            await _unitOf.Complete();
            response.Status = 200;
            response.Message = "product Updated Successfuly";
            response.IsSucceeded = true;
            return response;
        }
        public async Task<ResponseDto> DeleteProduct(int id)
        {
            var product=await _unitOf.Repository<Product>().FindAsync(c=>c.Id==id);
            if (product==null)
            {
                return new ResponseDto
                {
                    Status = 400,
                    Message = "Product Deletion Faild { Could not  be Found }!"
                };
            }
            _unitOf.Repository<Product>().Delete(product);
            await _unitOf.Complete();
            return new ResponseDto
            {
                Status = 200,
                IsSucceeded = true,
                Message = "Product Deletion Succeeded"
            };
        }


    }
}
