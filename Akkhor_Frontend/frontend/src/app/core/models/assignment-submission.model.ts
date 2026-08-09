// =====================================================
// ASSIGNMENT SUBMISSION MODEL
// =====================================================

export interface AssignmentSubmission {

  // ===================================================
  // PRIMARY KEY
  // ===================================================

  id: string;

  // ===================================================
  // ASSIGNMENT
  // ===================================================

  assignmentId: string;

  assignmentTitle?: string | null;

  // Original assignment file
  assignmentAttachmentUrl?: string | null;

  assignmentAttachmentFileName?: string | null;

  assignmentAttachmentContentType?: string | null;

  assignmentAttachmentFileSize?: number | null;

  // ===================================================
  // STUDENT
  // ===================================================

  studentId: string;

  studentName?: string | null;

  // ===================================================
  // SUBMISSION
  // ===================================================

  submittedAt: string;

  submissionText?: string | null;

  // Student answer script/file
  attachmentUrl?: string | null;

  attachmentFileName?: string | null;

  attachmentContentType?: string | null;

  attachmentFileSize?: number | null;

  // ===================================================
  // MARKS
  // ===================================================

  marksObtained?: number | null;

  feedback?: string | null;

  // ===================================================
  // STATUS
  // ===================================================

  status: string;

  // ===================================================
  // EVALUATION
  // ===================================================

  gradedAt?: string | null;

  gradedBy?: string | null;
}


// =====================================================
// CREATE
// =====================================================

export interface CreateAssignmentSubmission {

  assignmentId: string;

  submissionText?: string;

  attachmentUrl?: string;

  attachmentFileName?: string;

  attachmentContentType?: string;

  attachmentFileSize?: number;
}


// =====================================================
// UPDATE
// =====================================================

export interface UpdateAssignmentSubmission {

  submissionText?: string;

  attachmentUrl?: string;

  attachmentFileName?: string;

  attachmentContentType?: string;

  attachmentFileSize?: number;
}


// =====================================================
// EVALUATE
// =====================================================

export interface EvaluateAssignmentSubmission {

  marksObtained: number;

  feedback?: string | null;
}