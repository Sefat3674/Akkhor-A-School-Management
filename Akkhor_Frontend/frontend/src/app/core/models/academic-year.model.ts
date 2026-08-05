export interface AcademicYear {

  id: string;

  name: string;

  startDate: string;

  endDate: string;

  isActive: boolean;

  createdAt: string;

}



export interface CreateAcademicYear {

  name: string;

  startDate: string;

  endDate: string;

  isActive: boolean;

}



export interface UpdateAcademicYear {

  name: string;

  startDate: string;

  endDate: string;

  isActive: boolean;

}