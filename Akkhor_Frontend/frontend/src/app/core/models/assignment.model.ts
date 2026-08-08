export interface Assignment {
  id: string;

  teacherId: string;
  teacherName?: string;

  academicYearId: string;
  academicYearName?: string;

  classId: string;
  className?: string;

  sectionId?: string | null;
  sectionName?: string;

  courseId: string;
  courseName?: string;

  subjectId: string;
  subjectName?: string;

  title: string;
  description?: string | null;

  deadline: string;

  maximumMarks: number;

  attachmentUrl?: string | null;
  attachmentFileName?: string | null;
  attachmentContentType?: string | null;
  attachmentFileSize?: number | null;

  isPublished: boolean;
  publishedAt?: string | null;

  isActive: boolean;

  createdAt: string;
  updatedAt?: string | null;

  submissionCount: number;
}
export interface CreateAssignment {
  academicYearId: string;
  classId: string;
  sectionId?: string | null;

  courseId: string;
  subjectId: string;

  title: string;
  description?: string;

  deadline: string;

  maximumMarks: number;

  attachmentUrl?: string | null;
  attachmentFileName?: string | null;
  attachmentContentType?: string | null;
  attachmentFileSize?: number | null;

  isPublished: boolean;
}

export interface UpdateAssignment {
  academicYearId: string;
  classId: string;
  sectionId?: string | null;

  courseId: string;
  subjectId: string;

  title: string;
  description?: string;

  deadline: string;

  maximumMarks: number;

  attachmentUrl?: string | null;
  attachmentFileName?: string | null;
  attachmentContentType?: string | null;
  attachmentFileSize?: number | null;

  isPublished: boolean;

  isActive: boolean;
}