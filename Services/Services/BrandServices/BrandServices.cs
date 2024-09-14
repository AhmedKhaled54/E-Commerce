using AutoMapper;
using Core.Entity;
using Infrastructure.Interfaces;
using Services.Dtos.BrandsDto;
using Services.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.BrandServices
{
    public class BrandServices : IBrandServices
    {
        private readonly IUnitOfWork _unitOf;
        private readonly IMapper mapper;

        public BrandServices(IUnitOfWork unitOf,IMapper mapper)
        {
            _unitOf = unitOf;
            this.mapper = mapper;
        }

       

        public async Task<ResponseDto> GetAll()
        {
            var brand=await _unitOf.Repository<ProductBrand>().GetAll();
            if (brand != null && brand.Count ()> 0)
            {
                var Map = mapper.Map<List<BrandDto>>(brand)
                     .OrderByDescending(c => c.BrandName);
                return new  ResponseDto
                {
                    Status = 200,
                    Models = Map,
                    IsSucceeded = true
                };
            }
            return new ResponseDto
            {
                Status = 400,
                Message = "An error occured or there is no ProductBrand in stock!"
            };
        }

        public async Task<ResponseDto> GetById(int id)
        {
            var brand =await _unitOf.Repository<ProductBrand>().GetById(id);
            if (brand != null)
            {
                var map = mapper.Map<BrandDto>(brand);
                return new ResponseDto
                {
                    Status = 200,
                    Model = map,
                    IsSucceeded = true
                };
            }
            return new ResponseDto
            {
                Status = 400,
                Message = "this Brand  does not exist!"
            };
        }

        public async Task<ResponseDto> Create(CreateOrUpdateBrandDto dto)
        {
            var response = new ResponseDto();
            if (await _unitOf.Repository<ProductBrand>().FindAsync(c => c.Name == dto.BrandName) != null)
            {
                response.Status = 400;
                response.Message = "Brand Already Exist!";
                return response;
            }
            var brandmap=mapper.Map<ProductBrand>(dto);
            await _unitOf.Repository<ProductBrand>().AddAsync(brandmap);
            await _unitOf.Complete();
            response.Status = 200;
            response.IsSucceeded = true;
            response.Model = brandmap;
            return response;
            
        }



        public async Task<ResponseDto> Update(int id, CreateOrUpdateBrandDto dto)
        {
            var brand =await _unitOf.Repository<ProductBrand>().FindAsync(c=>c.Id == id);
            if (brand==null||await _unitOf.Repository<ProductBrand>()
                .FindAsync(c => c.Name == dto.BrandName) != null)
            {
                return new ResponseDto
                {
                    Status=400,
                    Message="Brand Updated Faild!"
                };
            }
            brand.Name= dto.BrandName;
             _unitOf.Repository<ProductBrand>().Update(brand);
            await _unitOf.Complete();
            return new ResponseDto
            {
                Status = 200,
                IsSucceeded = true,
                Message = $"Brand{brand.Name} Update Secceeded"
            };
        }
        public async Task<ResponseDto> DeleteById(int id)
        { 
            var brand = await _unitOf.Repository<ProductBrand>().FindAsync(c=>c.Id==id);
            if (brand != null)
            {
                _unitOf.Repository<ProductBrand>().Delete(brand);
                await _unitOf.Complete();
                return new ResponseDto
                {
                    Status = 200,
                    IsSucceeded = true,
                    Message = "Brand Deleted Successfuly"
                };
            }
            return new ResponseDto
            {
                Status = 400,
                Message = "Brand Deletion Faild"
            };

        }
    }
}
