// =====================================================
// Teacher Dropdown
// =====================================================

export interface TeacherDropdown {
  id: string;
  fullName: string | null;
  email: string | null;
}


// =====================================================
// Teacher Assignment
// =====================================================

export interface TeacherAssignment {
  id: string;

  // Teacher
  teacherId: string;
  teacherName: string | null;
  teacherEmail: string | null;

  // Academic Year
  academicYearId: string;
  academicYearName: string | null;

  // Class
  classId: string;
  className: string | null;

  // Section
  sectionId: string | null;
  sectionName: string | null;

  // Course
  courseId: string;
  courseName: string | null;

  // Subject
  subjectId: string;
  subjectName: string | null;

  // Assignment Settings
  isPrimary: boolean;
  isActive: boolean;

  // Audit
  createdAt: string;
  createdBy: string | null;

  updatedAt: string | null;
  updatedBy: string | null;
}


// =====================================================
// Create Teacher Assignment
// =====================================================

export interface CreateTeacherAssignment {
  teacherId: string;

  academicYearId: string;

  classId: string;

  sectionId: string | null;

  courseId: string;

  subjectId: string;

  isPrimary: boolean;

  isActive: boolean;

  createdBy?: string | null;
}


// =====================================================
// Update Teacher Assignment
// =====================================================

export interface UpdateTeacherAssignment {
  teacherId: string;

  academicYearId: string;

  classId: string;

  sectionId: string | null;

  courseId: string;

  subjectId: string;

  isPrimary: boolean;

  isActive: boolean;

  updatedBy?: string | null;
}