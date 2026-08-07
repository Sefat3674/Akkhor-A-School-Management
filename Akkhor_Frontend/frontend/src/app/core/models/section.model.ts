export interface SectionModel {

  id: string;

  classId: string;

  className: string;

  sectionName: string;

  roomNumber?: string;

  capacity?: number;

  isActive: boolean;

  studentCount: number;

  createdAt: string;

  updatedAt?: string;
  
  academicYearName: string;

}

export interface CreateSection {

  classId: string;

  sectionName: string;

  roomNumber?: string;

  capacity?: number;

}

export interface UpdateSection {

  sectionName: string;

  roomNumber?: string;

  capacity?: number;

  isActive: boolean;

}