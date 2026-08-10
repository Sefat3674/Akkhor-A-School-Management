export interface AdminDashboard {

  totalUsers: number;

  totalStudents: number;

  totalTeachers: number;

  totalAdmins: number;

  totalClasses: number;

  totalSections: number;

  totalCourses: number;

  totalSubjects: number;

  totalCourseSubjects: number;

  totalEnrollments: number;

  totalTeacherAssignments: number;

  totalAssignments: number;

  totalSubmissions: number;

  activeAcademicYear:
    AdminAcademicYear | null;

  recentAssignments:
    AdminAssignmentSummary[];

  recentSubmissions:
    AdminSubmissionSummary[];
}


export interface AdminAcademicYear {

  id: string;

  name: string;

  startDate: string;

  endDate: string;

  isActive: boolean;
}


export interface AdminAssignmentSummary {

  id: string;

  title?: string;

  description?: string;

  dueDate?: string | null;

  isActive: boolean;

  submissionCount: number;
}


export interface AdminSubmissionSummary {

  id: string;

  assignmentId: string;

  assignmentTitle?: string;

  studentId?: string;

  studentName?: string;

  submittedAt?: string | null;

  status?: string;

  marks?: number | null;
}