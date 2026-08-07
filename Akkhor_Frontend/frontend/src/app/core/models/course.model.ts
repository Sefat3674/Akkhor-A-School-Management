export interface CourseModel {

  id: string;

  classId: string;

  className: string;

  courseName: string;

  courseCode: string;

  description?: string;

  durationMonths?: number;

  isActive: boolean;

  subjectCount: number;

  studentCount: number;

  createdAt: string;
}



export interface CreateCourse {

  classId: string;

  courseName: string;

  courseCode: string;

  description?: string;

  durationMonths?: number;

}



export interface UpdateCourse {

  courseName: string;

  courseCode: string;

  description?: string;

  durationMonths?: number;

  isActive: boolean;

}