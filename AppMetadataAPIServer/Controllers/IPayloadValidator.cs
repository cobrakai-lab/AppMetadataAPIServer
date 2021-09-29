namespace AppMetadataAPIServer.Controllers
{
    public interface IPayloadValidator<T>
    {
        T Validate(T payload);
    }
}