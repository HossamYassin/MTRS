using AutoMapper;
using MTRS.Core.DTOs;
using MTRS.Core.Entities;
using MTRS.DAL.Interfaces;
using MTRS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MTRS.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        public List<CustomerDto> GetAll()
        {
            return _mapper.Map<List<CustomerDto>>(_customerRepository.GetAll());
        }

        public CustomerDto GetById(long id)
        {
            return _mapper.Map<CustomerDto>(_customerRepository.GetById(id));
        }

        public void Add(CustomerDto customerDto)
        {
            var entity = _mapper.Map<Customer>(customerDto);
            _customerRepository.Add(entity);
        }

        public IList<CustomerDto> Get(Expression<Func<Customer, bool>> expression, string sortColumn, bool isSortAscending, int page, int pageSize)
        {
            return _mapper.Map<List<CustomerDto>>(_customerRepository.Find(expression, sortColumn, isSortAscending, page, pageSize).ToList());
        }

        public void Remove(CustomerDto customerDto)
        {
            var entity = _mapper.Map<Customer>(customerDto);
            _customerRepository.Remove(entity);
        }

        public void Update(CustomerDto customerDto)
        {
            var customer = _customerRepository.GetById(customerDto.Id);
            _mapper.Map(customerDto, customer);

            _customerRepository.Update();
        }

        public int Count()
        {
            return _customerRepository.Count();
        }
    }
}
