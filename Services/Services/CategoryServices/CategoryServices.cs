using AutoMapper;
using Core.Entity;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using Services.Dtos.CatgegoryDto;
using Services.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.CategoryServices
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUnitOfWork _unitOf;
        private readonly IMapper mapper;

        public CategoryServices(IUnitOfWork unitOf,IMapper mapper)
        {
            _unitOf = unitOf;
            this.mapper = mapper;
        }
      
        public async Task<ResponseDto> GetAll()
        {
            var category =await _unitOf.Repository<Category>().GetAll();
            if (category != null||category.Count()>0)
            {
                var Map=mapper.Map<List<CategoryDetailsDto>>(category).OrderByDescending(c=>c.CategoryName);
                return new ResponseDto
                {
                    Status = 200,
                    IsSucceeded = true,
                    Models = Map
                };
            }
            return new ResponseDto
            {
                Status = 400,
                Message = "An error occured or there is no Category in stock!"
            };

        }

        public async Task<ResponseDto> GetByid(int id)
        { 
            var category =await _unitOf.Repository<Category>().FindAsync(c=>c.Id==id);
            if (category != null)
            {
                var map = mapper.Map<CategoryDetailsDto>(category);
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
                Message = "this Category  does not exist"
            };

        }
        public async Task<ResponseDto> Create(CreateOrUpdateCategory dto)
        {
            var response = new ResponseDto();
            if (await _unitOf.Repository<Category>().FindAsync(c => c.Name == dto.CategoryName) != null)
            {
                response.Status = 400;
                response.Message = "Catgeory Already Exist!";
                return response;
            }
            var CategoryMap = mapper.Map<Category>(dto);
            await _unitOf.Repository<Category>().AddAsync(CategoryMap);
            await _unitOf.Complete();
            response.Status = 200;
            response.IsSucceeded = true;
            response.Model = CategoryMap;
            return response;

        }

       

        public async Task<ResponseDto> Update(int id, CreateOrUpdateCategory dto)
        {
            var response = new ResponseDto();
            var category =await _unitOf.Repository<Category>().FindAsync(c=>c.Id == id);
           if (category == null||await _unitOf.Repository<Category>().FindAsync(c => c.Name == dto.CategoryName) != null)
           {
                response.Status = 400;
                response.Message = "Category Updated Faild!";
                return response;
           }
           category.Name= dto.CategoryName;
            _unitOf.Repository<Category>().Update(category);
            await _unitOf.Complete();
            response.Status= 200;
            response.IsSucceeded= true;
            response.Message = "Category Updated Successfult";
            return response;

        }
        public async Task<ResponseDto> Delete(int id)
        {
            var category =await _unitOf.Repository<Category>().FindAsync(c=> c.Id == id);
            if (category == null)
            {
                return new ResponseDto
                {
                    Status = 400,
                    Message = "Category Deletion Faild !"
                };
            }
            _unitOf.Repository<Category>().Delete(category);
            await _unitOf.Complete();
            return new ResponseDto
            {
                Status = 200,
                IsSucceeded = true,
                Message = "Category Deleted Successfuly"
            };

       

        }
    }
}
