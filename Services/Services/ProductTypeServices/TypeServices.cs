using AutoMapper;
using Core.Entity;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using Services.Dtos.Response;
using Services.Dtos.TypeDto;
using Services.HandleResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.ProductTypeServices
{
    public class TypeServices : ITypeServices
    {
        private readonly IUnitOfWork _unitOf;
        private readonly IMapper mapper;

        public TypeServices(IUnitOfWork unitOf,IMapper mapper)
        {
            this._unitOf = unitOf;
            this.mapper = mapper;
        }
       
        public async Task<ResponseDto> GetAll()
        {
            var type =await _unitOf.Repository<ProductType>().GetAll();
            if (type != null || type.Count() > 0)
            {
                var map=mapper.Map<List<TypeDto>>(type)
                    .OrderByDescending(t=>t.TypeName);
                return new ResponseDto
                {
                    Status = 200,
                    IsSucceeded = true,
                    Models = map
                };
            }

            return new ResponseDto
            {
                Status = 400,
                Message = "An error occured or there is no ProductType in stock!"
            };

        }

        public async Task<ResponseDto> GetById(int id)
        {
            var type = await _unitOf.Repository<ProductType>().FindAsync(c => c.Id == id);
            if (type != null)
            {
                var map = mapper.Map<TypeDto>(type);
                return new ResponseDto
                {
                    Status = 200,
                    Model = map,
                    IsSucceeded=true
                };
            }
            return new ResponseDto
            {
                Status = 400,
                Message = "this Type  does not exist"
            };
        }
        public async Task<ResponseDto> Create(CreateOrUpdateTypeDto dto)
        {
            var response = new ResponseDto();
            if (await _unitOf.Repository<ProductType>().FindAsync(c => c.Name == dto.TypeName) != null)
            {
                response.Status = 400;
                response.Message = "Type Already  Exist !";
                return response;
            }
            var typemap=mapper.Map<ProductType>(dto);
            await _unitOf.Repository<ProductType>().AddAsync(typemap);
            await _unitOf.Complete();
            response.Status = 200;
            response.IsSucceeded = true;
            response.Model= typemap;

            return response;

        }

        public async Task<ResponseDto> Update(int id, CreateOrUpdateTypeDto dto)
        {
            var response = new ResponseDto();
            var type = await _unitOf.Repository<ProductType>().FindAsync(c => c.Id == id);
            if (type==null||await _unitOf.Repository<ProductType>().FindAsync(t => t.Name == dto.TypeName) != null)
            {
                response.Status = 400;
                response.Message = "Type Updated Faild!";
                return response;
            }
            type.Name = dto.TypeName;
            _unitOf.Repository<ProductType>().Update(type);
            await _unitOf.Complete();
            response.Status = 200;
            response.Message = "Type Updated Successufuly";
            response.IsSucceeded = true;
            return response;
 


        }
        public async Task<ResponseDto> DeleteById(int id)
        {
            var type = await _unitOf.Repository<ProductType>().FindAsync(c => c.Id == id);
            if (type == null)
            {
                return new ResponseDto
                {
                    Status = 400,
                    Message = "Type Deletion Faild !"
                };
            }
            _unitOf.Repository<ProductType>().Delete(type);
            await _unitOf.Complete();
            return new ResponseDto
            {
                Status = 200,
                Message = "Type Deleted Successfuly",
                IsSucceeded = true
            };
        
        }
    }
}
