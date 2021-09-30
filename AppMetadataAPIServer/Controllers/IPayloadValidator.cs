namespace AppMetadataAPIServer.Controllers
{
    public interface IPayloadValidator<T>
    {
        void Validate(T appMetadata);
    }
}