export interface AuthResponse {

  token: string;

  expiresAt: string;

  fullName: string;

  email: string;

  userId: string;

  role: string;

}



export interface CurrentUser {

  userId: string;

  fullName: string;

  email: string;

  role?: string;

}



export interface RegisterUser {

  fullName: string;

  email: string;

  password: string;

}