export interface StudentEnrollment {
  id: string;

  studentId: string;
  studentName: string;

  classId: string;
  className: string;

  courseId: string;
  courseName: string;

  sectionId?: string | null;
  sectionName?: string | null;

  rollNumber?: string | null;

  enrollmentDate: string;

  status: string;

  createdAt: string;
  updatedAt?: string | null;
}


export interface CreateStudentEnrollment {
  studentId: string;

  classId: string;

  courseId: string;

  sectionId?: string | null;

  rollNumber?: string | null;

  enrollmentDate: string;

  status: string;
}


export interface UpdateStudentEnrollment {
  classId: string;

  courseId: string;

  sectionId?: string | null;

  rollNumber?: string | null;

  status: string;
}