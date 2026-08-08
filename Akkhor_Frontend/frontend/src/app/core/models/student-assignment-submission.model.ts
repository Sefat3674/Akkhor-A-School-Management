// =====================================================
// STUDENT ASSIGNMENT SUBMISSION MODEL
// =====================================================

export interface AssignmentSubmission {

  id: string;

  assignmentId: string;
  assignmentTitle?: string | null;

  studentId: string;
  studentName?: string | null;

  // ---------------------------------------------------
  // Submission
  // ---------------------------------------------------

  submittedAt: string;

  submissionText?: string | null;

  // ---------------------------------------------------
  // Attachment
  // ---------------------------------------------------

  attachmentUrl?: string | null;

  attachmentFileName?: string | null;

  attachmentContentType?: string | null;

  attachmentFileSize?: number | null;

  // ---------------------------------------------------
  // Marks / Feedback
  // ---------------------------------------------------

  marksObtained?: number | null;

  feedback?: string | null;

  // ---------------------------------------------------
  // Status
  // ---------------------------------------------------

  status: string;

  // ---------------------------------------------------
  // Evaluation
  // ---------------------------------------------------

  gradedAt?: string | null;

  gradedBy?: string | null;
}


// =====================================================
// CREATE SUBMISSION
// =====================================================

export interface CreateAssignmentSubmission {

  assignmentId: string;

  submissionText?: string | null;

  attachmentUrl?: string | null;

  attachmentFileName?: string | null;

  attachmentContentType?: string | null;

  attachmentFileSize?: number | null;
}


// =====================================================
// UPDATE SUBMISSION
// =====================================================

export interface UpdateAssignmentSubmission {

  submissionText?: string | null;

  attachmentUrl?: string | null;

  attachmentFileName?: string | null;

  attachmentContentType?: string | null;

  attachmentFileSize?: number | null;
}