// =====================================================
// STUDENT DASHBOARD
// =====================================================

export interface StudentDashboard {

  // ===================================================
  // STUDENT INFORMATION
  // ===================================================

  studentId?: string;

  studentName?: string | null;

  email?: string | null;

  profileImageUrl?: string | null;

  className?: string | null;

  classId?: string | null;

  sectionName?: string | null;

  sectionId?: string | null;

  academicYearName?: string | null;


  // ===================================================
  // STATISTICS
  // ===================================================

  totalAssignments?: number;

  pendingAssignments?: number;

  submittedAssignments?: number;

  gradedAssignments?: number;

  overdueAssignments?: number;


  // ===================================================
  // LISTS
  // ===================================================

  recentAssignments?: StudentDashboardAssignment[];

  upcomingAssignments?: StudentDashboardAssignment[];

  recentSubmissions?: StudentDashboardSubmission[];

}


// =====================================================
// STATISTICS
// =====================================================

export interface StudentDashboardStatistics {

  totalAssignments?: number;

  pendingAssignments?: number;

  submittedAssignments?: number;

  gradedAssignments?: number;

  overdueAssignments?: number;

  submissionRate?: number;

  averageMarks?: number;

}


// =====================================================
// ASSIGNMENT
// =====================================================

export interface StudentDashboardAssignment {

  id?: string;

  title?: string | null;

  description?: string | null;

  courseName?: string | null;

  subjectName?: string | null;

  teacherName?: string | null;

  dueDate?: string | null;

  totalMarks?: number;

  isPublished?: boolean;

  isSubmitted?: boolean;

  isGraded?: boolean;

  isOverdue?: boolean;

  status?: string;

}


// =====================================================
// SUBMISSION
// =====================================================

export interface StudentDashboardSubmission {

  id?: string;

  assignmentId?: string;

  assignmentTitle?: string | null;

  submittedAt?: string | null;

  marksObtained?: number | null;

  totalMarks?: number;

  isGraded?: boolean;

  status?: string;

}