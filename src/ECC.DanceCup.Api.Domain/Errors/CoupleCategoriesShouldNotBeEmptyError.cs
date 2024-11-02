using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Errors;

public class CoupleCategoriesShouldNotBeEmptyError : DomainError
{
    public CoupleCategoriesShouldNotBeEmptyError()
        : base("Пара должна зарегистрироваться хотя бы в 1 категорию")
    {
    }
}