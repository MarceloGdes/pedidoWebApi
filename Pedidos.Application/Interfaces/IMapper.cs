
namespace Pedidos.Application.Interfaces;
public interface IMapper<TDto,TCreateDto, TEntity>
{
    TDto MapToDto(TEntity entity);

    TEntity MapToEntity(TCreateDto dto);
}
