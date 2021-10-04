namespace AppMetadataAPIServer.Validators
{
    public interface IPayloadValidator<T>
    {
        void Validate(T appMetadata);
    }
}