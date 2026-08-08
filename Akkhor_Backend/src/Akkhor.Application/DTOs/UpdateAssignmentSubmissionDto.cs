using System;

namespace Akkhor.Application.DTOs.Assignments;

public class UpdateAssignmentSubmissionDto
{
    public string? SubmissionText { get; set; }

    public string? AttachmentUrl { get; set; }

    public string? AttachmentFileName { get; set; }

    public string? AttachmentContentType { get; set; }

    public long? AttachmentFileSize { get; set; }
}