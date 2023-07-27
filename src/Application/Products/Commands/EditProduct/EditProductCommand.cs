using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper.Internal;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.EditProduct;
public record EditProductCommand(Guid Id, Guid UserId, string Property, object Value) : IRequest;

public class EditProductCommandHandler : IRequestHandler<EditProductCommand>
{
    public EditProductCommandHandler(IApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IApplicationDbContext DbContext { get; }

    public async Task Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        if (entity.UserID != request.UserId) throw new UnauthorizedAccessException();

        var type = entity.GetType();
        var memberInfo = type.GetFieldOrProperty(request.Property) 
            ?? throw new MemberNotFoundException(type.Name, request.Property);
        var valueType = request.Value.GetType();

        if (valueType != memberInfo.ReflectedType) 
            throw new ArgumentException("Cannot assign value to member. Incorrect type");

        if (!memberInfo.CanBeSet())
            throw new MemberAccessException("Member cannot be set");

        memberInfo.SetMemberValue(entity, request.Value);

        entity.AddDomainEvent(new ProductEditedEvent(entity, request.Property));

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}

public class EditProductCommandValidator : AbstractValidator<EditProductCommand>
{
    public EditProductCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(c => c.UserId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(c => c.Property)
            .NotEmpty();
    }
}