    using Akkhor.Application.DTOs.Assignments;
    using Akkhor.Application.Interfaces.Repositories;
    using Akkhor.Application.Interfaces.Services;
    using Akkhor.Domain.Entities;

    namespace Akkhor.Application.Services;

    public class AssignmentSubmissionService
        : IAssignmentSubmissionService
    {
        private readonly IAssignmentSubmissionRepository _repository;

        public AssignmentSubmissionService(
            IAssignmentSubmissionRepository repository)
        {
            _repository = repository;
        }


        // =====================================================
        // GET ALL SUBMISSIONS
        // =====================================================

        public async Task<IEnumerable<AssignmentSubmissionDto>>
            GetAllAsync()
        {
            var submissions =
                await _repository.GetAllAsync();

            return submissions.Select(MapToDto);
        }


        // =====================================================
        // GET SUBMISSION BY ID
        // =====================================================

        public async Task<AssignmentSubmissionDto?>
            GetByIdAsync(Guid id)
        {
            var submission =
                await _repository.GetByIdAsync(id);

            if (submission == null)
            {
                return null;
            }

            return MapToDto(submission);
        }


        // =====================================================
        // GET SUBMISSIONS BY ASSIGNMENT
        // =====================================================

        public async Task<IEnumerable<AssignmentSubmissionDto>>
            GetByAssignmentAsync(
                Guid assignmentId)
        {
            var submissions =
                await _repository.GetByAssignmentIdAsync(
                    assignmentId);

            return submissions.Select(MapToDto);
        }


        // =====================================================
        // GET SUBMISSION BY ASSIGNMENT + STUDENT
        // =====================================================

        public async Task<AssignmentSubmissionDto?>
            GetByAssignmentAndStudentAsync(
                Guid assignmentId,
                string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
            {
                throw new ArgumentException(
                    "Student ID is required.");
            }

            var submission =
                await _repository
                    .GetByAssignmentAndStudentAsync(
                        assignmentId,
                        studentId);

            if (submission == null)
            {
                return null;
            }

            return MapToDto(submission);
        }


        // =====================================================
        // GET MY SUBMISSIONS
        // =====================================================

        public async Task<IEnumerable<AssignmentSubmissionDto>>
            GetMySubmissionsAsync(
                string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
            {
                throw new ArgumentException(
                    "Student ID is required.");
            }

            var submissions =
                await _repository.GetByStudentIdAsync(
                    studentId);

            return submissions.Select(MapToDto);
        }


        // =====================================================
        // CREATE / SUBMIT ASSIGNMENT
        // =====================================================

        public async Task<AssignmentSubmissionDto>
            CreateAsync(
                CreateAssignmentSubmissionDto dto,
                string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
            {
                throw new ArgumentException(
                    "Student ID is required.");
            }


            // -------------------------------------------------
            // Validate Assignment
            // -------------------------------------------------

            if (dto.AssignmentId == Guid.Empty)
            {
                throw new ArgumentException(
                    "Assignment ID is required.");
            }


            // -------------------------------------------------
            // Validate Submission
            // -------------------------------------------------

            if (
                string.IsNullOrWhiteSpace(
                    dto.SubmissionText)
                &&
                string.IsNullOrWhiteSpace(
                    dto.AttachmentUrl)
            )
            {
                throw new ArgumentException(
                    "Submission text or attachment is required.");
            }


            // -------------------------------------------------
            // Check Existing Submission
            // -------------------------------------------------

            var existing =
                await _repository
                    .GetByAssignmentAndStudentAsync(
                        dto.AssignmentId,
                        studentId);

            if (existing != null)
            {
                throw new InvalidOperationException(
                    "You have already submitted this assignment.");
            }


            // -------------------------------------------------
            // Create Entity
            // -------------------------------------------------

            var submission =
                new AssignmentSubmission
                {
                    Id = Guid.NewGuid(),

                    AssignmentId =
                        dto.AssignmentId,

                    StudentId =
                        studentId,


                    // Submission
                    SubmissionText =
                        dto.SubmissionText?.Trim(),


                    // Attachment
                    FileUrl =
                        dto.AttachmentUrl,

                    FileName =
                        dto.AttachmentFileName,

                    ContentType =
                        dto.AttachmentContentType,

                    FileSize =
                        dto.AttachmentFileSize,


                    // Submission Date
                    SubmittedAt =
                        DateTime.UtcNow,


                    // Initial Status
                    Status =
                        "Submitted",


                    // Audit
                    CreatedAt =
                        DateTime.UtcNow
                };


            var created =
                await _repository.CreateAsync(
                    submission);


            return MapToDto(created);
        }


        // =====================================================
        // UPDATE SUBMISSION
        // =====================================================

        public async Task<AssignmentSubmissionDto?>
            UpdateAsync(
                Guid id,
                UpdateAssignmentSubmissionDto dto,
                string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
            {
                throw new ArgumentException(
                    "Student ID is required.");
            }


            // -------------------------------------------------
            // Find Student Submission
            // -------------------------------------------------

            var submission =
                await _repository.GetByIdAsync(id);

            if (submission == null)
            {
                return null;
            }


            // -------------------------------------------------
            // Security Check
            // -------------------------------------------------

            if (submission.StudentId != studentId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to update this submission.");
            }


            // -------------------------------------------------
            // Prevent Updating Evaluated Submission
            // -------------------------------------------------

            if (
                submission.Status.Equals(
                    "Evaluated",
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                throw new InvalidOperationException(
                    "An evaluated submission cannot be modified.");
            }


            // -------------------------------------------------
            // Validate Submission
            // -------------------------------------------------

            if (
                string.IsNullOrWhiteSpace(
                    dto.SubmissionText)
                &&
                string.IsNullOrWhiteSpace(
                    dto.AttachmentUrl)
                &&
                string.IsNullOrWhiteSpace(
                    submission.FileUrl)
            )
            {
                throw new ArgumentException(
                    "Submission text or attachment is required.");
            }


            // -------------------------------------------------
            // Update Submission
            // -------------------------------------------------

            submission.SubmissionText =
                dto.SubmissionText?.Trim();


            // -------------------------------------------------
            // Attachment
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                dto.AttachmentUrl))
            {
                submission.FileUrl =
                    dto.AttachmentUrl;

                submission.FileName =
                    dto.AttachmentFileName;

                submission.ContentType =
                    dto.AttachmentContentType;

                submission.FileSize =
                    dto.AttachmentFileSize;
            }


            // -------------------------------------------------
            // Submission Date
            // -------------------------------------------------

            submission.SubmittedAt =
                DateTime.UtcNow;


            // -------------------------------------------------
            // Status
            // -------------------------------------------------

            submission.Status =
                "Submitted";


            // -------------------------------------------------
            // Audit
            // -------------------------------------------------

            submission.UpdatedAt =
                DateTime.UtcNow;


            var updated =
                await _repository.UpdateAsync(
                    submission);


            return MapToDto(updated);
        }


        // =====================================================
        // DELETE SUBMISSION
        // =====================================================

        public async Task<bool>
            DeleteAsync(
                Guid id,
                string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
            {
                throw new ArgumentException(
                    "Student ID is required.");
            }


            // -------------------------------------------------
            // Find Submission
            // -------------------------------------------------

            var submission =
                await _repository.GetByIdAsync(id);

            if (submission == null)
            {
                return false;
            }


            // -------------------------------------------------
            // Security Check
            // -------------------------------------------------

            if (submission.StudentId != studentId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to delete this submission.");
            }


            // -------------------------------------------------
            // Prevent Deletion After Evaluation
            // -------------------------------------------------

            if (
                submission.Status.Equals(
                    "Evaluated",
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                throw new InvalidOperationException(
                    "An evaluated submission cannot be deleted.");
            }


            return await _repository.DeleteAsync(id);
        }


        // =====================================================
        // EVALUATE / GRADE SUBMISSION
        // =====================================================

        // =====================================================
        // EVALUATE / GRADE SUBMISSION
        // =====================================================

        public async Task<AssignmentSubmissionDto?>
            EvaluateAsync(
                Guid id,
                EvaluateAssignmentSubmissionDto dto,
                string teacherId)
        {
            if (string.IsNullOrWhiteSpace(teacherId))
            {
                throw new ArgumentException(
                    "Teacher ID is required.");
            }


            // -------------------------------------------------
            // Find Submission
            // -------------------------------------------------

            var submission =
                await _repository.GetByIdAsync(id);

            if (submission == null)
            {
                return null;
            }


            // -------------------------------------------------
            // Validate Marks
            // -------------------------------------------------

            if (dto.MarksObtained < 0)
            {
                throw new ArgumentException(
                    "Marks cannot be negative.");
            }


            // -------------------------------------------------
            // Check Maximum Marks
            // -------------------------------------------------

            if (
                submission.Assignment != null
                &&
                dto.MarksObtained >
                submission.Assignment.MaximumMarks
            )
            {
                throw new ArgumentException(
                    "Obtained marks cannot exceed maximum marks.");
            }


            // -------------------------------------------------
            // Update Marks
            // -------------------------------------------------

            submission.Marks =
                dto.MarksObtained;


            // -------------------------------------------------
            // Feedback
            // -------------------------------------------------

            submission.Feedback =
                dto.Feedback?.Trim();


            // -------------------------------------------------
            // Evaluation Date
            // -------------------------------------------------

            submission.EvaluatedAt =
                DateTime.UtcNow;


            // -------------------------------------------------
            // Evaluated By
            // -------------------------------------------------

            submission.EvaluatedBy =
                teacherId;


            // -------------------------------------------------
            // Status
            // -------------------------------------------------

            submission.Status =
                "Evaluated";


            // -------------------------------------------------
            // Audit
            // -------------------------------------------------

            submission.UpdatedAt =
                DateTime.UtcNow;


            // -------------------------------------------------
            // Save
            // -------------------------------------------------

            var updated =
                await _repository.UpdateAsync(
                    submission);


            // -------------------------------------------------
            // Return DTO
            // -------------------------------------------------

            return MapToDto(
                updated);
        }


        // =====================================================
        // GET SUBMISSION COUNT
        // =====================================================

        public async Task<int>
            GetSubmissionCountAsync(
                Guid assignmentId)
        {
            return await _repository
                .GetSubmissionCountAsync(
                    assignmentId);
        }


        // =====================================================
        // GET PENDING SUBMISSION COUNT
        // =====================================================

        public async Task<int>
            GetPendingSubmissionCountAsync(
                Guid assignmentId)
        {
            return await _repository
                .GetPendingSubmissionCountAsync(
                    assignmentId);
        }


        // =====================================================
        // ENTITY → DTO
        // =====================================================

        private static AssignmentSubmissionDto
            MapToDto(
                AssignmentSubmission submission)
        {
            return new AssignmentSubmissionDto
            {
                // -------------------------------------------------
                // Submission
                // -------------------------------------------------

                Id =
                    submission.Id,

                AssignmentId =
                    submission.AssignmentId,

                AssignmentTitle =
                    submission.Assignment?.Title,


                // -------------------------------------------------
                // Student
                // -------------------------------------------------

                StudentId =
                    submission.StudentId,

                StudentName =
                    submission.Student?.FullName,


                // -------------------------------------------------
                // Submission
                // -------------------------------------------------

                SubmittedAt =
                    submission.SubmittedAt
                    ?? submission.CreatedAt,

                SubmissionText =
                    submission.SubmissionText,


                // -------------------------------------------------
                // Attachment
                // -------------------------------------------------

                AttachmentUrl =
                    submission.FileUrl,

                AttachmentFileName =
                    submission.FileName,

                AttachmentContentType =
                    submission.ContentType,

                AttachmentFileSize =
                    submission.FileSize,


                // -------------------------------------------------
                // Marks & Feedback
                // -------------------------------------------------

                MarksObtained =
                    submission.Marks,

                Feedback =
                    submission.Feedback,


                // -------------------------------------------------
                // Status
                // -------------------------------------------------

                Status =
                    submission.Status,


                // -------------------------------------------------
                // Audit
                // -------------------------------------------------

                GradedAt =
                    submission.EvaluatedAt,

                GradedBy =
                    submission.EvaluatedBy
            };
        }




    }