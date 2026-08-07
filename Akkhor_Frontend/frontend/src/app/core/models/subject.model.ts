export interface SubjectModel {

  id: string;

  name: string;

  code: string;

  description?: string;

  creditHours?: number;

  isActive: boolean;

  createdAt: string;

  updatedAt?: string;

}



export interface CreateSubject {

  name: string;

  code: string;

  description?: string;

  creditHours?: number;

}



export interface UpdateSubject {

  name: string;

  code: string;

  description?: string;

  creditHours?: number;

  isActive: boolean;

}