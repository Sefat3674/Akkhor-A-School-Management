// =====================================================
// ASSIGNMENT SUBMISSION
// =====================================================

export interface AssignmentSubmission {

  // ---------------------------------------------------
  // PRIMARY KEY
  // ---------------------------------------------------

  id: string;

  // ---------------------------------------------------
  // ASSIGNMENT
  // ---------------------------------------------------

  assignmentId: string;

  assignmentTitle?: string;

  // ---------------------------------------------------
  // STUDENT
  // ---------------------------------------------------

  studentId: string;

  studentName?: string;

  // ---------------------------------------------------
  // SUBMISSION
  // ---------------------------------------------------

  submittedAt: string;

  submissionText?: string;

  attachmentUrl?: string;

  attachmentFileName?: string;

  attachmentContentType?: string;

  attachmentFileSize?: number;

  // ---------------------------------------------------
  // MARKS & FEEDBACK
  // ---------------------------------------------------

  marksObtained?: number | null;

  feedback?: string | null;

  // ---------------------------------------------------
  // STATUS
  // ---------------------------------------------------

  status: string;

  // ---------------------------------------------------
  // EVALUATION
  // ---------------------------------------------------

  gradedAt?: string | null;

  gradedBy?: string | null;
}


// =====================================================
// CREATE ASSIGNMENT SUBMISSION
// =====================================================

export interface CreateAssignmentSubmission {

  // ---------------------------------------------------
  // ASSIGNMENT
  // ---------------------------------------------------

  assignmentId: string;

  // ---------------------------------------------------
  // SUBMISSION
  // ---------------------------------------------------

  submissionText?: string;

  // ---------------------------------------------------
  // ATTACHMENT
  // ---------------------------------------------------

  attachmentUrl?: string;

  attachmentFileName?: string;

  attachmentContentType?: string;

  attachmentFileSize?: number;
}


// =====================================================
// UPDATE ASSIGNMENT SUBMISSION
// =====================================================

export interface UpdateAssignmentSubmission {

  // ---------------------------------------------------
  // SUBMISSION
  // ---------------------------------------------------

  submissionText?: string;

  // ---------------------------------------------------
  // ATTACHMENT
  // ---------------------------------------------------

  attachmentUrl?: string;

  attachmentFileName?: string;

  attachmentContentType?: string;

  attachmentFileSize?: number;
}


// =====================================================
// EVALUATE ASSIGNMENT SUBMISSION
// =====================================================

export interface EvaluateAssignmentSubmission {

  marksObtained: number;

  feedback?: string | null;
}