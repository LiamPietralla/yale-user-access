export type YaleUserCode = {
    id: number;
    code: string;
    status: UserCodeStatus;
    isHome: boolean;
}

export enum UserCodeStatus {
    AVAILABLE = 0,
    ENABLED = 1,
    DISABLED = 2
}