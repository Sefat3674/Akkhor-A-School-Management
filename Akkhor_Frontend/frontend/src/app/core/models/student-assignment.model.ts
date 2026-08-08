// =====================================================
// STUDENT ASSIGNMENT MODEL
// =====================================================

export interface StudentAssignment {

  // ---------------------------------------------------
  // ID
  // ---------------------------------------------------

  id: string;


  // ---------------------------------------------------
  // Teacher
  // ---------------------------------------------------

  teacherId: string;
  teacherName?: string | null;


  // ---------------------------------------------------
  // Academic Year
  // ---------------------------------------------------

  academicYearId: string;
  academicYearName?: string | null;


  // ---------------------------------------------------
  // Class
  // ---------------------------------------------------

  classId: string;
  className?: string | null;


  // ---------------------------------------------------
  // Section
  // ---------------------------------------------------

  sectionId?: string | null;
  sectionName?: string | null;


  // ---------------------------------------------------
  // Course
  // ---------------------------------------------------

  courseId: string;
  courseName?: string | null;


  // ---------------------------------------------------
  // Subject
  // ---------------------------------------------------

  subjectId: string;
  subjectName?: string | null;


  // ---------------------------------------------------
  // Assignment Information
  // ---------------------------------------------------

  title: string;

  description?: string | null;

  deadline: string;

  maximumMarks: number;


  // ---------------------------------------------------
  // Attachment
  // ---------------------------------------------------

  attachmentUrl?: string | null;

  attachmentFileName?: string | null;

  attachmentContentType?: string | null;

  attachmentFileSize?: number | null;


  // ---------------------------------------------------
  // Publication
  // ---------------------------------------------------

  isPublished: boolean;

  publishedAt?: string | null;


  // ---------------------------------------------------
  // Active
  // ---------------------------------------------------

  isActive: boolean;


  // ---------------------------------------------------
  // Audit
  // ---------------------------------------------------

  createdAt: string;

  updatedAt?: string | null;


  // ---------------------------------------------------
  // Submission
  // ---------------------------------------------------

  submissionCount: number;
}


// =====================================================
// ALIAS
// =====================================================
// This allows existing code using Assignment to work.
// =====================================================

export type Assignment = StudentAssignment;