using System;

namespace Akkhor.Application.DTOs.Assignments;

public class EvaluateAssignmentSubmissionDto
{
    public decimal MarksObtained { get; set; }

    public string? Feedback { get; set; }
}