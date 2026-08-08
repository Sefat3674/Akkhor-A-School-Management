export interface TeacherClass {
  assignmentId: string;

  teacherId: string;

  academicYearId: string;
  academicYearName: string;

  classId: string;
  className: string;

  sectionId: string | null;
  sectionName: string | null;

  roomNumber: string | null;

  courseId: string;
  courseName: string;

  subjectId: string;
  subjectName: string;

  isPrimary: boolean;
  isActive: boolean;
}