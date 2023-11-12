export type ApiResponse<T> = {
    success: boolean;
    error?: string;
    data?: T;
}