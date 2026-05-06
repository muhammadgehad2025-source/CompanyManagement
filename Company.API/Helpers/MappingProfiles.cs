using AutoMapper;
using Company.Core.DTOs;
using Company.Core.DTOs.Identity;
using Company.Core.DTOs.Order;
using Company.Core.Entities;
using Company.Core.Entities.Order;

namespace Company.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Employee
            CreateMap<Employee, EmployeeDto>()
                .ForMember(
                    d => d.Department,
                    o => o.MapFrom(s => s.Department.Name)
                );

            // Address
            CreateMap<Address, AddressDto>();

            // OrderItem
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
                .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity));

            // Order
            CreateMap<Order, OrderDto>()
                .ForMember(d => d.Date, o => o.MapFrom(s => s.OrderDate))
                .ForMember(d => d.Total, o => o.MapFrom(s => s.Subtotal))
                .ForMember(d => d.ShippingAddress, o => o.MapFrom(s => s.ShipToAddress))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.OrderItems))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status));
        }
    }
}