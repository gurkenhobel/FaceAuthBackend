namespace FaceAuthService.Contract
{
    public enum MessageType
    {
        AuthRequest,
        AuthResponse,
        
        RegisterRequest,
        RegisterResponse,

        FileCatalogRequest,
        FileCatalogUpdate,

        FileCreateRequest,
        FileDeleteRequest,

        FileDownloadRequest,
        FileDownloadResponse
    }
}