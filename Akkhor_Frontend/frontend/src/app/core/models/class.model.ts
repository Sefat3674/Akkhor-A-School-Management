export interface ClassModel {

  id: string;

  academicYearId: string;

  academicYearName: string;

  name: string;

  code: string;

  description?: string;

  displayOrder: number;

  isActive: boolean;

  sectionCount: number;

  createdAt: string;
}



export interface CreateClass {

  academicYearId: string;

  name: string;

  code: string;

  description?: string;

  displayOrder: number;

}



export interface UpdateClass {

  name: string;

  code: string;

  description?: string;

  displayOrder: number;

  isActive: boolean;

}